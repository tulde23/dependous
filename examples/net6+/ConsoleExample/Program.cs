// See https://aka.ms/new-console-template for more information
global using Dependous;
using System.Text;
using Autofac;
using ExamplesCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var tokenSource = new CancellationTokenSource();


//var interfaceTemlate = """
//    public interface IExampleInterface{index}{ }
//    """;

//var classTemplate = """
//    public class ExampleImplementation{index} :  IExampleInterface{index}, {Lifetime} { }
//    """;

//var lifetime = new List<string>
//{
//    nameof(ISingletonDependency),
//    nameof(ITransientDependency),
//    nameof(IScopedDependency),
//    nameof(ISelfSingleton),
//    nameof(ISelfTransient)
//};

//var sb = new StringBuilder("using Dependous;");
//sb.AppendLine();

//var rand = new Random();
//Enumerable.Range(1, 1000).ToList().ForEach(x =>
//{
//    var l = lifetime[rand.Next(0, lifetime.Count - 1)];

//    sb.AppendLine(interfaceTemlate.Replace("{index}", x.ToString()));
//    sb.AppendLine(classTemplate.Replace("{index}", x.ToString()).Replace("{Lifetime}", l));
//});

//File.WriteAllText(@"C:\Development-Personal\dependous\examples\net6+\ExamplesCommon\Types.cs", sb.ToString());
IHost host = Host.CreateDefaultBuilder(args)
    .UseAutoFacContainer(AssemblyPaths.From("ConsoleExample", "ExamplesCommon"),
    configurationBuilder: (c) => c.PersistScanResults = true, logger: (e) => Console.WriteLine($"{e}"),
   containerBuilder: (builder) =>
    {
        builder.RegisterType<ExampleImplementation8>().OwnedByLifetimeScope().As<IExampleInterface8>();
    } )
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