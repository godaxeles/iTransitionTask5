using System.ComponentModel;
using System.Diagnostics;

namespace Task5.Services;

public class Mp3Encoder
{
    public byte[] Encode(byte[] wavBytes)
    {
        using var process = StartLameProcess();
        return RunEncoding(process, wavBytes);
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

    private static byte[] RunEncoding(Process process, byte[] wavBytes)
    {
        using var output = new MemoryStream();
        var readStdout = process.StandardOutput.BaseStream.CopyToAsync(output);
        var readStderr = process.StandardError.ReadToEndAsync();

        process.StandardInput.BaseStream.Write(wavBytes);
        process.StandardInput.Close();

        readStdout.Wait();
        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new InvalidOperationException($"lame exited with {process.ExitCode}: {readStderr.Result}");

        return output.ToArray();
    }
}
