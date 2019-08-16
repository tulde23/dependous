# Dependous
*A lightweight cross platform dependency scanning library*

Dependous grants you the power to insepct a collection of .NET assemblies and extract types implementing specific interfaces.  That's it.  Simple and easy.  

Dependous eliminates the need to explictly describe  different types of dependencies you wish to locate.  Instead Dependeous allows you define your dependencies as you develop your 
classes with a "think about it once and forget about it" approach by employing discovery interfaces. 

A discovery interface can be any .NET interface.  Dependous implements three intefaces that are specific to IOC container lifetime management.

**ITransientDependency**
Designates an implementor as a discoverable dependency and asserts it's lifetime management should be transient.

**ISingletonDependency**
Designates an implementor as a discoverable dependency and asserts it's lifetime management should be singleton.
   
**IScopedDependency**
Designates an implementor as a discoverable dependency and asserts it's lifetime management should be scoped.

These three interfaces only assert your intent on how instances of your dependency should be handled.   This will be captured in the `DependencyMetadata` instance.
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
  config.AddAdditionalDiscoveryTypes(d => d.RegisterType<IModule>(ServiceLifetime.Singleton)
                                           .RegisterType<IRegistrationSource>(ServiceLifetime.Singleton));
  configurationBuilder?.Invoke(config);
};
var scanner     = DependencyScannerFactory.Create();
var scanResult = scanner.Scan((f) => f.StartsWith("MyAssembly"),configurationBuilder );
```
In addition to the three default discovery interfaces, this example will register two additional discovery interfaces `IModule` and `IRegistrationSource`.  Now when the scanner executes,
any class implementing `IModule` or `IRegistrationSource` will be discovered as a dependency.

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
In some scenarios, you may want to inspect the collected dependcy metadata from a Dependous scan.  
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

##### .NET 4.5 
``` csharp 

public static void Main(string[] args){
    var container = Dependous.Autofac.AutofacContainerFactory.BuildContainer(
        (f) => f.StartsWith("Dependous"),
        logger: (item) => { Console.WriteLine(item); }
     );
}

```

##### ASP.NET 
``` csharp 


public class Global : HttpApplication
{
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
          var container = Dependous.Autofac.AutofacContainerFactory.BuildContainer(
        (f) => f.StartsWith("Dependous"),
        logger: (item) => { Console.WriteLine(item); }
     );
    }
}

```
For more info on integrating Autofac into ASP.NET go [here](http://autofac.readthedocs.io/en/latest/integration/webapi.html#quick-start)

##### .NET Core
``` csharp
 /// <summary>
/// Configures the services.
/// </summary>
/// <param name="services">The services.</param>
public IServiceProvider ConfigureServices(IServiceCollection services)
{
   services.AddDependencyScanning();
  container = services.BuildAutoFacContainer(
        (f) => f.StartsWith("Dependous"), 
        logger: (item) => { Console.WriteLine(item); });
  return container.Resolve<IServiceProvider>();
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
If you don't like using framework dependent constructs like `IIndex`, you can always implement a chain of command pattern using a custom selection strategy like so:
``` csharp
 public interface IMultipleConsumers{
    bool CanHandle(object data);
 }
 public class Consumer1 : IMultipleConsumers, ISingletonDependency{
    public bool CanHandle(object data){
        if( data is int){
          return true; 
    }
        return false;
   }
 }
 public class Consumer2 : IMultipleConsumers, ISingletonDependency{
    public bool CanHandle(object data){
        if( data is string){
        return true; 
    }
return false;
    }
}

public class ConsumerFactory : IConsumerFactory{
    
            private readonly IEnumerable<Func<IMultipleConsumers>> consumerResolver;
       public ConsumerFactory( IEnumerable<Func<IMultipleConsumers>> consumerResolver){
            //passing a func<T> allows you to lazy load your dependency
            this.consumerResolver = consumerResolver;
        }
    public IMultipleConsumers Resolve(object data){
             return this.consumerResolver.Find(x=>x().CanHandle(data));
    }
}
```



### Implement a Decorator With AutoFac
Unfortunately, Dependous can't scan and automatically register decorators.  However, with a bit of configure code you can achieve this.
First of, let's create a `RegistrationModule' that will abstract all of our custom AutoFac registrations.
``` csharp
public class RegistrationModule
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        public static void Load(ContainerBuilder builder)
        {
            //decorates or proxies the real implementation of INationalAffiliationLookupService. we are doing this to provide AOP style caching
            builder.RegisterType<SqlNationalAffiliationLookupService>().Named<INationalAffiliationLookupService>("decoratable");
            builder.RegisterDecorator<INationalAffiliationLookupService>(
                    (c, inner) => new NationalAffiliationLookupServiceDecorator(inner, c.Resolve<ICache>()),
                    fromKey: "decoratable"
                ).As<IExpirable>().As<INationalAffiliationLookupService>().SingleInstance();


        }
    }

//this is your decorator.  Now when you request an instance of `INationalAffiliationLookupService`, this will be returned and the
true implemenation SqlNationalAffiliationLookupService will be injected into your decorator.  
  public class NationalAffiliationLookupServiceDecorator : INationalAffiliationLookupService, IExpirable
    {
        private const int CacheTimeout = 18000;
        private static ConcurrentDictionary<string, string> _keyCache = new ConcurrentDictionary<string, string>();
        private readonly ICache _cache;

        //5 hours
        private readonly INationalAffiliationLookupService _nationalAffiliationLookupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NationalAffiliationLookupServiceDecorator" /> class.
        /// </summary>
        /// <param name="nationalAffiliationLookupService">The national affiliation lookup service.</param>
        /// <param name="cache">The cache.</param>
        public NationalAffiliationLookupServiceDecorator(INationalAffiliationLookupService nationalAffiliationLookupService,
            ICache cache)
        {
            _nationalAffiliationLookupService = nationalAffiliationLookupService;
            _cache = cache;
        }
}

//Next go to Startup.cs and pass an overload to BuildAutoFacContainer
    ApplicationContainer = services.BuildAutoFacContainer(assemblyNameFilter, containerBuilder: (cb) => RegistrationModule.Load(cb));
```