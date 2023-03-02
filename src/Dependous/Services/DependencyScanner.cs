namespace Dependous
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Dependous.Attributes;
    using Dependous.Extensions;
    using Dependous.Models;

    /// <summary>
    /// The default implementation of IDependencyScanner
    /// </summary>
    /// <seealso cref="Dependous.IDependencyScanner" />
    internal sealed class DependencyScanner : IDependencyScanner
    {
        private readonly IAssemblyLocator assemblyLocator;
        private readonly IAssemblyTypeService assemblyTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyScanner" /> class by injecting an instance of IContainerRegistrationService
        /// </summary>
        /// <param name="assemblyLocator">The assembly locater.</param>
        /// <param name="assemblyTypeService">The assembly type service.</param>
        internal DependencyScanner(IAssemblyLocator assemblyLocator,
                                             IAssemblyTypeService assemblyTypeService)
        {
            this.assemblyLocator = assemblyLocator;
            this.assemblyTypeService = assemblyTypeService;
        }

        /// <summary>
        /// Attempts to scan through a collection of assemblies looking for dependencies to register with the current IOC container.
        /// </summary>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// Enumerable of Dependency
        /// </returns>

        public DependencyScanResult Scan(
                               Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter = null,
                               Action<IDependousConfiguration> configurationBuilder = null, Action<object> logger = null)
        {
            var stopwatch = new Stopwatch();
            var serviceLifetime = ServiceLifetime.Transient;
            stopwatch.Start();
            //initialize configuration
            var configuration = new DependousConfiguration();
            configurationBuilder = configurationBuilder ?? new Action<IDependousConfiguration>((cfg) => { });
            configurationBuilder(configuration);
            var assemblyResult = assemblyLocator.Locate(assemblyNameFilter, configuration);
            assemblyResult.Duration = stopwatch.Elapsed;
            var attributeScanning = configuration.GetAttributeScanningConfiguration();
            var scanResult = new DependencyScanResult();

            assemblyResult.DiscoveredAssemblyNames.ToList().ForEach(
            //Parallel.ForEach(assemblyResult.DiscoveredAssemblyNames,
                (assemblyName) =>
                {
                    logger?.DynamicInvoke($"Inspecting Assembly {assemblyName.FullName}");
                    //inspect current assembly and produce a list of types
                    var assemblyTypeResult = this.assemblyTypeService.GetTypes(assemblyName);
                    //add any loaded exceptions
                    scanResult.AddExceptions(assemblyTypeResult.LoaderExceptions);
                    //save the scanned assembly
                    scanResult.AddAssembly(assemblyTypeResult.LoadedAssembly);
                    foreach (var type in assemblyTypeResult.DiscoveredTypes)
                    {
                        var dependencyType = type.GetTypeInfo();
                        var allImplementedInterfaces = dependencyType.ImplementedInterfaces;
                        var interceptAttribute = type.GetCustomAttribute<InterceptAttribute>();
                        var interceptor = dependencyType.GetCustomAttribute<InterceptAttribute>(true);

                        if (attributeScanning.enabled)
                        {
                            var attribute = type.GetCustomAttribute(attributeScanning.attributeType);
                            if (attribute != null)
                            {
                                Delegate d = attributeScanning.mapper as Delegate;
                                var result = d.DynamicInvoke(attribute) as DependencyAttribute;
                                if (result != null)
                                {
                                    scanResult.AddMetadata(new DependencyMetadata(null,
                                                                                  allImplementedInterfaces,
                                                                                  dependencyType,
                                                                                  result.LifeTime,
                                                                                  attribute.GetType().GetTypeInfo(),
                                                                                  interceptor?.Interceptor?.GetTypeInfo()));
                                }
                                continue;
                            }
                        }

                        //grab the interface used to register this dependency.  We'll use it later to determine the lifetime manager
                        var discoveryInterface = allImplementedInterfaces.FirstOrDefault(i => configuration.AdditionalTypes.Any(x => x.InterfaceType?.Equals(i) == true));
                        serviceLifetime = configuration.AdditionalTypes.Where(x => x.InterfaceType?.Equals(discoveryInterface) == true).Select(x => x.ServiceLifetime).FirstOrDefault();

                        //lets see if the user passed any custom discovery interfaces.   Only if they didn't already declare a discovery interface
                        if (discoveryInterface == null)
                        {
                            continue;
                        }
                        //let's exclude the default discovery interfaces.  They aren't useful anymore
                        allImplementedInterfaces = allImplementedInterfaces.Except(DefaultDiscoveryInterfaces.Interfaces);

                        //grab the target attribute if any
                        var targetAttribute = dependencyType.GetCustomAttribute<TargetAttribute>(true);
                        if (targetAttribute != null)
                        {
                            allImplementedInterfaces = allImplementedInterfaces.Intersect(targetAttribute.Interfaces);
                        }

                        //grab the named dependency attribute if any
                        var namedDependency = dependencyType.GetCustomAttribute<NamedDependencyAttribute>(true);
                        //grab the interceptor attribute if it exists

                        var metadata = new DependencyMetadata(namedDependency?.Name,
                                                                                    allImplementedInterfaces,
                                                                                    dependencyType,
                                                                                    serviceLifetime,
                                                                                    discoveryInterface.GetTypeInfo(),
                                                                                    interceptor?.Interceptor?.GetTypeInfo());

                        scanResult.AddMetadata(metadata);
                    }
                }

            );
            stopwatch.Stop();
            scanResult.Duration = stopwatch.Elapsed;
            scanResult.Configuration = configuration;
            return scanResult;
        }

        /// <summary>
        /// Produces a collection of assemblies based on a search filter.
        /// </summary>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public AssemblyLocatorResult DiscoverAssemblies(
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter = null,
            Action<IDependousConfiguration> configurationBuilder = null, Action<object> logger = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //initialize configuration
            var configuration = new DependousConfiguration();
            configurationBuilder = configurationBuilder ?? new Action<IDependousConfiguration>((cfg) => { });
            configurationBuilder(configuration);
            var result = assemblyLocator.Locate(assemblyNameFilter, configuration);
            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;
            return result;
        }
    }
}