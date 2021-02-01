namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Dependous;
    using Dependous.Factory;
    using Dependous.Models;

    /// <summary>
    /// </summary>
    public static class DependencyScanExtensions
    {
        /// <summary>
        /// Enables dependency scanning
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IServiceCollection AddDependencyScanning(this IServiceCollection services,
                                                                                      Func<IServiceCollection, IServiceCollection> builder = null)
        {
            var s = builder?.Invoke(services);
            return RegisterDefaults(s ?? services);
        }

        /// <summary>
        /// Produces a collection of assembly names
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="patternBuilder">The pattern builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// scanner - No dependency scanner registered. Make sure you call AddDependencyScanning
        /// before calling this method
        /// </exception>
        public static AssemblyLocatorResult DiscoverAssemblies(this IServiceCollection services,
                                                                                                        Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = null)
        {
            var serviceProvider = services.BuildServiceProvider();

            var scanner = serviceProvider.GetService<IDependencyScanner>();
            if (scanner == null)
            {
                throw new InvalidOperationException("No dependency scanner registered.  Make sure you call AddDependencyScanning before calling this method");
            }

            return scanner.DiscoverAssemblies(patternBuilder);
        }

        /// <summary>
        /// Discovers the assemblies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns></returns>
        public static AssemblyLocatorResult DiscoverAssemblies(this IServiceCollection services, AssemblyPaths assemblyPath)
        {
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> builder = (b) =>
            {
                foreach (var item in assemblyPath.Paths)
                {
                    b.StartsWith(item);
                }
                return b;
            };
            return DiscoverAssemblies(services, builder);
        }

        /// <summary>
        /// Registers defaults implementations of all required services.
        /// </summary>
        /// <param name="services">The services.</param>
        private static IServiceCollection RegisterDefaults(IServiceCollection services)
        {
            var buildProvider = services.BuildServiceProvider();
            if (buildProvider.GetService<IDependencyScanner>() == null)
            {
                services.AddSingleton((t) => DependencyScannerFactory.Create());
            }
            if (buildProvider.GetService<IAssemblyLocator>() == null)
            {
                services.AddSingleton((t) => AssemblyLocatorFactory.Resolve());
            }
            if (buildProvider.GetService<IAssemblyTypeService>() == null)
            {
                services.AddSingleton((t) => AssemblyTypeFactory.Resolve());
            }

            return services;
        }
    }
}