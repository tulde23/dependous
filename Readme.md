# Dependous
*A lightweight cross platform dependency scanning library*

Pronounced *dee-pend-us*, 
Dependous grants you the power to inspect a collection of .NET assemblies and extract types implementing specific interfaces.  That's it.  Simple and easy.  

Dependous eliminates the need to explicitly describe  different types of dependencies you wish to locate.  Instead Dependeous allows you define your dependencies as you develop your 
classes with a "think about it once and forget about it" approach by employing discovery interfaces. 

A discovery interface can be any .NET interface.    Dependous implements five interfaces that are specific to IOC container lifetime management.

**ITransientDependency**
Designates an implementor as a discoverable dependency and asserts it's lifetime management should be transient.

**ISingletonDependency**
Designates an implementor as a discoverable dependency and asserts it's lifetime management should be singleton.
   
**IScopedDependency**
Designates an implementor as a discoverable dependency and asserts it's lifetime management should be scoped.

**ISelfTransient**
Makes a concrete type injectable through the IOC container and sets it's lifetime to trainsient

**ISelfSingleton**
Makes a concrete type injectable through the IOC container and sets it's lifetime to singleton

These five interfaces only assert your intent on how instances of your dependency should be handled.   This will be captured in the `DependencyMetadata` instance.
It's up to the IOC container integration to actually register your dependencies.

When declaring discovery interfaces, you must bind it to a specific lifetime.  Either Singleton, Transient or Scoped.

## Examples

##### The Most Basic Scanning Example #####
``` csharp
var scanner     = DependencyScannerFactory.Create();
var scanResult = scanner.Scan((f) => f.StartsWith("MyAssembly"));
```



##### Adding Additional Discovery Interfaces #####
``` csharp
Action<IDependousConfiguration> configurationBuilder = (config) =>
{
  config.AddAdditionalDiscoveryTypes(d => d.RegisterType<IMyService1>(ServiceLifetime.Singleton)
                                           .RegisterType<IMyService2>(ServiceLifetime.Singleton));
  configurationBuilder?.Invoke(config);
};
var scanner     = DependencyScannerFactory.Create();
var scanResult = scanner.Scan((f) => f.StartsWith("MyAssembly"),configurationBuilder );
```
In addition to the five default discovery interfaces, this example will register two additional discovery interfaces `IMyService1` and `IMyService2`.  Now when the scanner executes,
any class implementing `IMyService1` or `IMyService2` will be discovered as a dependency with a lifetime of singleton.

##### Register a concrete type #####
```
public class MyModel : ISelfTransient{
   
    public MyModel( ISomeDependency d){
    }
}

This registration will allow you to inject new instances of a concrete type.
```

##### Using A Default Discovery Interface #####

``` csharp
public class MyService : IMyService, ISingletonDependency{
}
```
MyService will be discovered as a dependency with a lifetime of singleton.

##### Targeting a specific range of interfaces #####
By default, the assembly scanning will extract all interfaces implemented by your dependency.  Often times this behavior is desirable; however, if you need
to target a specific subset of interfaces, use this attribute.

``` csharp
 [Target(typeof(IInterface1))]
public class MyDependency : IInterface1, IInterface2, ITransientDependency{
}
```
MyDependency will be discovered but it's list of implemented interfaces will only contain `IInterface1` 

##### Using Probing Paths For Plug-In based Architectures
By default, Dependous will only scan for assemblies located directly in your bin folder.  If you wish to expand your search horizons, add your probing paths to the dependous configuration.

``` csharp
    var instance = DependencyScannerFactory.Create();
    var result = instance.Scan((b) => b.StartsWith("Dependous.Probing"), (c) => c.AddProbingPaths(pb => pb.AddProbingPath("../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.0")));
    Assert.True(result.Metadata.Any());
```

Additionally, if you need to have a different assembly filter, you can pass it along with your probing path:
``` csharp
    var instance = DependencyScannerFactory.Create();
    var result = instance.Scan((b) => b.StartsWith("Dependous.Probing"), (c) => c.AddProbingPaths(pb => pb.AddProbingPath("../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.0"),cb=>cb.StartsWith("Plugins")));
    Assert.True(result.Metadata.Any());
```
##### Accessing Dependous scan results
In some scenarios, you may want to inspect the collected dependency metadata from a Dependous scan.  
``` csharp
var scanner     = DependencyScannerFactory.Create();
var scanResult = scanner.Scan((f) => f.StartsWith("MyAssembly"),cb=>cb.PersistScanResults=true );
//Now you can inject the results into any class

public class MyService{
    public MyService(DependencyScanResult dependencyScanResults){
    }
}
```

## IOC Container Integration ##
Now that we have a list of dependencies what do we do with them?  It's time to integrate an IOC container.  For this example, I chose [AutoFac](http://docs.autofac.org/en/latest/getting-started/index.html).

For more info on integrating Autofac into ASP.NET go [here](http://autofac.readthedocs.io/en/latest/integration/webapi.html#quick-start)

##### .NET Core 3.1
``` csharp

# Program.cs
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
 public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutoFacContainer(AssemblyPaths.From("Your assembly prefix"), logger: (o) => Console.WriteLine($"{o}"))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

#Statup.cs

 public void ConfigureServices(IServiceCollection services)
        {
             ...
            services.AddControllers();
            services.AddDependencyScanning();
        }


```
###### Configuring Probing Paths With AutoFac
``` csharp
    var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyScanning();
            container = serviceCollection.BuildAutoFacContainer((f) => f.StartsWith("Dependous"),
                logger: (item) => { output.WriteLine($"{item}"); Console.WriteLine(item); }, configurationBuilder: (cb) =>
                 {
                     cb.PersistScanResults = true;
                     cb.AddProbingPaths(pb => pb.AddProbingPath("../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.0"));
                 });
```

In this example, we instruct Dependous to additionally search `"../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.0"` for our assemblies.  We also set `PersistScanResults=true`, allowing you to inject all the metadata collection by the Dependous scan.


#### Named/Keyed Dependencies In AutoFac
Autofac supports quite a few features like `Metadata` which is not currently supported by Dependous.  It does however, support the usages of named/keyed dependencies.
[Documentation](http://autofaccn.readthedocs.io/en/latest/advanced/keyed-services.html)
So for example:
``` csharp
 public interface IMultipleConsumers{}
 [NamedDependency("A")]
 public class Consumer1 : IMultipleConsumers, ISingletonDependency{}
 [NamedDependency("B")]
 public class Consumer2 : IMultipleConsumers, ISingletonDependency{}

 
 public class MyService : IMyService{

    //IIndex is an Autofac construct used to resolve keyed dependencies.
    public MyService(IIndex<string,IMultipleConsumers> index){
      //actually get your service
      var myService = index["A"];
    }
}
```
### Implement a Decorator With AutoFac

Decoration can be achieved quite easily by implementing a specific interface, `IDecorator`

``` csharp
  public interface IDecoratableService : ISingletonDependency
    {
        string Method();
    }

 public class TrueService : IDecoratableService
    {
        public string Method()
        {
            return $"Hello";
        }
    }

public class DecoratorOfTrueService : IDecoratableService, IDecorator<IDecoratableService>
{   public DecoratorOfTrueService(IDecoratableService decoratableService)
        {
            DecoratableService = decoratableService;
        }

        public IDecoratableService DecoratableService { get; }

        public string Method()
        {
            var sb = new StringBuilder();
            sb.Append("DecoratorOfTrueService.Before");
            sb.Append(DecoratableService.Method());
            sb.Append("DecoratorOfTrueService.After");
            return sb.ToString();
        }
}

```

##### .NET 6 Web Application

``` csharp

#Program.cs
global using Dependous;
global using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseDependousAutofac(AssemblyPaths.From("Your Pattern"), logger: (o) => Console.WriteLine($"{o}"));
...
builder.Services.AddDependencyScanning();

```


##### .NET 6 Generic Host

``` csharp

#Program.cs
global using Dependous;
global using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

            
var builder = Host.CreateDefaultBuilder(args)
                  .UseAutoFacContainer(AssemblyPaths.From("Your Pattern"), logger: (o) => Console.WriteLine($"{o}"));
                  
...
builder.Services.AddDependencyScanning();

```

##### .NET 6 Generic Host Custom Container Registration

``` csharp

#Program.cs
global using Dependous;
global using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

            
var builder = Host.CreateDefaultBuilder(args)
                  .UseAutoFacContainer(AssemblyPaths.From("Your Pattern"), logger: (o) => Console.WriteLine($"{o}"),
                  containerBuilder: (c) =>{
               
                        //now you have full access to the AutoFac container and can do whatever you like.
                    
                     });
                  
...
builder.Services.AddDependencyScanning();

```