using System;
using Dependous;
using Dependous.Unity;
using Microsoft.Extensions.Hosting;
using Microsoft.Practices.Unity;
using Unity.Microsoft.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UnityExtensionsNetCore

    {
        /// <summary>
        /// Adds Dependous and Unity as the preferred IOC container.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IHostBuilder UseUnityContainer(this IHostBuilder hostBuilder,
                                                                                     AssemblyPaths assemblyPaths,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<IUnityContainer> containerBuilder = null)
        {
           
        
            return hostBuilder.UseServiceProviderFactory(new UnityServiceProviderFactory(assemblyPaths, configurationBuilder, logger, containerBuilder));
        }

        /// <summary>
        /// Adds Dependous and Unity as the preferred IOC container.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblySearchPatternFactory">The assembly search pattern factory.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IHostBuilder UseUnityContainer(this IHostBuilder hostBuilder,
                                                                                     AssemblySearchPatternFactory assemblySearchPatternFactory,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<IUnityContainer> containerBuilder = null)
        {
            return hostBuilder.UseServiceProviderFactory(new UnityServiceProviderFactory(assemblySearchPatternFactory, configurationBuilder, logger, containerBuilder));
        }

        /// <summary>
        /// Adds Dependous and Unity as the preferred IOC container.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IUnityContainer BuildUnityContainer(this IServiceCollection services,
                                                                                     AssemblyPaths assemblyPaths,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<IUnityContainer> containerBuilder = null)
        {
            var container = new UnityServiceProviderFactory(assemblyPaths, configurationBuilder, logger, containerBuilder).CreateBuilder(services);
            return container;
        }

        public static IUnityContainer BuildUnityContainer(this IServiceCollection services,
                                                                                   AssemblySearchPatternFactory assemblyPaths,
                                                                                   Action<IDependousConfiguration> configurationBuilder = null,
                                                                                   Action<object> logger = null,
                                                                                   Action<IUnityContainer> containerBuilder = null)
        {
            var container = new UnityServiceProviderFactory(assemblyPaths, configurationBuilder, logger, containerBuilder).CreateBuilder(services);
            return container;
        }
    }
}