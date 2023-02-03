// See https://aka.ms/new-console-template for more information
global using Dependous;
using ExamplesCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var tokenSource = new CancellationTokenSource();

IHost host = Host.CreateDefaultBuilder(args)
    .UseAutoFacContainer(AssemblyPaths.From("ConsoleExample", "ExamplesCommon"),
    configurationBuilder: (c) => c.PersistScanResults = true, logger: (e) => Console.WriteLine($"{e}"))
    .ConfigureServices(services =>
    {
        services.AddDependencyScanning();
    })
    .Build();

var service = host.Services.GetService<IMyService>();

var mySingleton = host.Services.GetService<MySingleton>();

while (!tokenSource.Token.IsCancellationRequested)
{
    var message = await service.ExecuteAsync();
    Console.WriteLine($"MyService {message}");
    Console.WriteLine($"Scan Results: {mySingleton.ScanResults.ToString()}");

    Console.Write("Press any key to continue.  Or exit to quit.  ");
    var response = Console.ReadLine();
    if (response?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
    {
        tokenSource.Cancel();
    }
}