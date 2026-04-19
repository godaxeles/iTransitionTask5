using System.ComponentModel;
using System.Diagnostics;

namespace Task5.Services;

public class Mp3Encoder
{
    public async Task<byte[]> EncodeAsync(byte[] wavBytes, CancellationToken cancellationToken = default)
    {
        using var process = StartLameProcess();
        return await RunEncodingAsync(process, wavBytes, cancellationToken);
    }

    private static Process StartLameProcess()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "lame",
            Arguments = "--silent -h -b 128 - -",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        try
        {
            return Process.Start(psi)
                   ?? throw new InvalidOperationException("Failed to start lame process.");
        }
        catch (Win32Exception ex)
        {
            throw new InvalidOperationException(
                "The 'lame' executable was not found in PATH. " +
                "Install via: apt-get install lame (Linux), brew install lame (macOS), or choco install lame (Windows).",
                ex);
        }
    }

    private static async Task<byte[]> RunEncodingAsync(Process process, byte[] wavBytes, CancellationToken cancellationToken)
    {
        using var output = new MemoryStream();
        var readStdout = process.StandardOutput.BaseStream.CopyToAsync(output, cancellationToken);
        var readStderr = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.StandardInput.BaseStream.WriteAsync(wavBytes, cancellationToken);
        process.StandardInput.Close();

        await readStdout;
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
            throw new InvalidOperationException($"lame exited with {process.ExitCode}: {await readStderr}");

        return output.ToArray();
    }
}
