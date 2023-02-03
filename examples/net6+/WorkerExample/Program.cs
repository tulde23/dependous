global using Dependous;
using WorkerExample;

IHost host = Host.CreateDefaultBuilder(args)
    .UseAutoFacContainer(AssemblyPaths.From("WorkerExample", "ExamplesCommon"), 
    configurationBuilder:(c)=> c.PersistScanResults = true, logger: (e) => Console.WriteLine($"{e}"))
    .ConfigureServices(services =>
    {
        services.AddDependencyScanning();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();