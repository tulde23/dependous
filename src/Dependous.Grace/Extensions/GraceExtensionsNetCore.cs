#if NETSTANDARD2_0|| NETSTANDARD2_1

using System;
using System.Linq;
using Grace;
using Grace.DependencyInjection;
using Grace.DependencyInjection.Extensions;
using Dependous;
using Dependous.GraceIoc;
using SL = Dependous.ServiceLifetime;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GraceExtensionsNetCore
    {
        
        /// <summary>
        /// Scans all assemblies for dependencies and registers them with an AutoFac container
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="patternBuilder">The pattern builder.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerBuilder">A delegate providing manual container registration</param>
        /// <returns>
        /// the autofac container
        /// </returns>
        public static DependencyInjectionContainer BuildGraceContainer(this IServiceCollection services,
                                                                                     Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = null,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<DependencyInjectionContainer> containerBuilder = null)
        {
            return Dependous.GraceIoc.GraceContainerFactory.BuildContainer(
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
    }
}

#endif