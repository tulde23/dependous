using System;
using Autofac;
using Dependous;
using Dependous.Autofac;
using Microsoft.Extensions.Hosting;
using SportsConnect.Dependous.Autofac;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutofacExtensionsNetCore
    {
        /// <summary>
        /// Adds Dependous and Autofac as the preferred IOC container.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IHostBuilder UseAutoFacContainer(this IHostBuilder hostBuilder,
                                                                                     AssemblyPaths assemblyPaths,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            AutoFacSetup.SetAutoFacCircularDependencyLimit();
            return hostBuilder.UseServiceProviderFactory(new AutoFacServiceProviderFactoryDecorator(assemblyPaths, configurationBuilder, logger, containerBuilder));
        }

        /// <summary>
        /// Adds Dependous and Autofac as the preferred IOC container.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblySearchPatternFactory">The assembly search pattern factory.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IHostBuilder UseAutoFacContainer(this IHostBuilder hostBuilder,
                                                                                     AssemblySearchPatternFactory assemblySearchPatternFactory,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            AutoFacSetup.SetAutoFacCircularDependencyLimit();
            return hostBuilder.UseServiceProviderFactory(new AutoFacServiceProviderFactoryDecorator(assemblySearchPatternFactory, configurationBuilder, logger, containerBuilder));
        }

        /// <summary>
        /// Adds Dependous and Autofac as the preferred IOC container.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IContainer BuildAutoFacContainer(this IServiceCollection services,
                                                                                     AssemblyPaths assemblyPaths,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            var container = new AutoFacServiceProviderFactoryDecorator(assemblyPaths, configurationBuilder, logger, containerBuilder).CreateBuilder(services).Build();
            return container;
        }

        public static IContainer BuildAutoFacContainer(this IServiceCollection services,
                                                                                   AssemblySearchPatternFactory assemblyPaths,
                                                                                   Action<IDependousConfiguration> configurationBuilder = null,
                                                                                   Action<object> logger = null,
                                                                                   Action<ContainerBuilder> containerBuilder = null)
        {
            var container = new AutoFacServiceProviderFactoryDecorator(assemblyPaths, configurationBuilder, logger, containerBuilder).CreateBuilder(services).Build();
            return container;
        }
    }
}