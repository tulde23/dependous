#if NETSTANDARD2_0|| NETSTANDARD2_1

using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Dependous;
using Dependous.Autofac;
using Microsoft.Extensions.Hosting;
using SL = Dependous.ServiceLifetime;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutofacExtensionsNetCore
    {
        public static void InterceptAll<TInterceptor>(this IDependousConfiguration dependousConfiguration)
        {
            dependousConfiguration.AddInterceptionTypes(b => b.RegisterType(typeof(TypeAll), typeof(TInterceptor)));
        }

        /// <summary>
        /// Scans all assemblies for dependencies and registers them with an AutoFac container
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="patternBuilder">The pattern builder.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">A delegate providing manual container registration</param>
        /// <returns>the autofac container</returns>
        public static IContainer BuildAutoFacContainer(this IServiceCollection services,
                                                                                     Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = null,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            return Dependous.Autofac.AutofacContainerFactory.BuildContainer(
                patternBuilder,
                () =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    return serviceProvider.GetService<IDependencyScanner>();
                },
                    configurationBuilder,
                    logger,
                    (cb, r) =>
                    {
                        cb.Populate(services);
                        containerBuilder?.DynamicInvoke(cb);
                    });
        }

        /// <summary>
        /// Builds the automatic fac container.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
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
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> builder = (b) =>
            {
                foreach (var item in assemblyPaths.Paths)
                {
                    b.StartsWith(item);
                }
                return b;
            };
            return BuildAutoFacContainer(services, builder, configurationBuilder, logger, containerBuilder);
        }

        /// <summary>
        /// Registers AutoFac as the container of choice. This is reserved for host building outside
        /// of ASP.NET (Console Apps)
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static IHostBuilder UseAutoFacContainer(this IHostBuilder hostBuilder,
               Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter = null,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            return hostBuilder.UseServiceProviderFactory(new AutoFacServiceProviderFactory(assemblyNameFilter, configurationBuilder, logger, containerBuilder));
        }
    }
}

#endif