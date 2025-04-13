using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YouTubeBackup.Services;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices((context, services) =>
{
    services.AddScoped<IYouTubeBackupService, YouTubeBackupService>();
});

var host = builder.Build();

try
{
    var backupService = host.Services.GetRequiredService<IYouTubeBackupService>();
    backupService.Backup().Wait();

    Console.WriteLine();
    Console.WriteLine("Sync completed.");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.WriteLine("Press <Enter> to close.");
Console.ReadKey();

