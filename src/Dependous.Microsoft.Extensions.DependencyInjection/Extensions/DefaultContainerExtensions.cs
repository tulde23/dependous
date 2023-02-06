using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Dependous.DefaultContainer
{
    public static class DefaultContainerExtensions
    {
        /// <summary>
        /// Adds Dependous and Autofac as the preferred IOC container.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static IServiceCollection BuildContainer(this IServiceCollection services,
                                                                                     AssemblyPaths assemblyPaths,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null)
        {
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = (b) =>
            {
                AssemblySearchPatternFactory.Merge(assemblyPaths, b);

                return b;
            };

            return BuildContainer(
            services,
            patternBuilder,
            () =>
            {
                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetService<IDependencyScanner>();
            },
                configurationBuilder,
                logger);
        }

        /// <summary>
        /// Adds the dependencies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assemblySearchPatternFactory">The assembly search pattern factory.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static IServiceCollection BuildContainer(this IServiceCollection services,
                                                                                     AssemblySearchPatternFactory assemblySearchPatternFactory,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null)
        {
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = (b) =>
            {
                AssemblySearchPatternFactory.Merge(assemblySearchPatternFactory, b);

                return b;
            };

            return BuildContainer(
            services,
            patternBuilder,
            () =>
            {
                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetService<IDependencyScanner>();
            },
                configurationBuilder,
                logger);
        }

        private static IServiceCollection BuildContainer(
            IServiceCollection services,
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = null,
            Func<IDependencyScanner> dependencyScanner = null,
            Action<IDependousConfiguration> configurationBuilder = null,
            Action<object> logger = null)
        {
            logger = logger ?? ((x) => { });
            dependencyScanner = dependencyScanner ?? (() => DependencyScannerFactory.Create());
            var scanner = dependencyScanner();
            if (scanner == null)
            {
                throw new InvalidOperationException("No dependency scanner registered.  Make sure you call AddDependencyScanning before calling this method");
            }

            Action<IDependousConfiguration> internalBuilder = (config) =>
            {
                configurationBuilder?.Invoke(config);
            };
            var scanResults = scanner.Scan(patternBuilder, internalBuilder, logger);
            logger($"Assembly Scan Completed In {scanResults.Duration.TotalMilliseconds}ms{Environment.NewLine} {String.Join(Environment.NewLine, scanResults.ScannedAssemblies.Select(x => x.FullName))}");

            logger($"Registering Dependencies {Environment.NewLine} {String.Join(Environment.NewLine, scanResults.Metadata.Select(x => x.DependencyType.FullName))}");
            if (scanResults.ScanExceptions != null && scanResults.ScanExceptions.Any())
            {
                foreach (var error in scanResults.ScanExceptions)
                {
                    logger(error);
                }
            }
            var containerRegistrationSerivce = new DefaultContainerRegistrationService(scanResults, services);
            if (scanResults.Configuration.PersistScanResults)
            {
                services.AddSingleton(scanResults);
            }
            var allRegistrations = containerRegistrationSerivce.RegisterAll();
            logger($"Registered {allRegistrations.Count()} dependencies.");

            return services;
        }
    }
}