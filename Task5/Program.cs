using Task5.Services;
using Task5.Services.Audio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<LocaleDataService>();
builder.Services.AddSingleton<LikesCalculator>();
builder.Services.AddSingleton<DataGeneratorService>();
builder.Services.AddSingleton<CoverGeneratorService>();

builder.Services.AddSingleton<MusicParamsFactory>();
builder.Services.AddSingleton<MelodyComposer>();
builder.Services.AddSingleton<BassComposer>();
builder.Services.AddSingleton<AudioSynthesizer>();
builder.Services.AddSingleton<WavEncoder>();
builder.Services.AddSingleton<AudioGeneratorService>();

builder.Services.AddSingleton<Mp3Encoder>();
builder.Services.AddSingleton<SongPackager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
