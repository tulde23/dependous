using System;
using System.Collections.Generic;
using System.Linq;
using Dependous.Models;
using Grace.DependencyInjection;

namespace Dependous.GraceIoc
{
    /// <summary>
    ///
    /// </summary>
    public static class GraceContainerFactory
    {
        /// <summary>
        /// Scans for dependencies and builds the autofac container.
        /// </summary>
        /// <param name="patternBuilder">The pattern builder.</param>
        /// <param name="dependencyScanner">The dependency scanner.  If null, uses the default scanner</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerConfigurator">The container configurator.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">No dependency scanner registered.  Make sure you call AddDependencyScanning before calling this method</exception>
        public static DependencyInjectionContainer BuildContainer(

            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = null,
            Func<IDependencyScanner> dependencyScanner = null,
            Action<IDependousConfiguration> configurationBuilder = null,
            Action<object> logger = null,
            Action<DependencyInjectionContainer, IReadOnlyCollection<DependencyMetadata>> containerConfigurator = null)
        {
            logger = logger ?? ((x) => Console.WriteLine(x));
            dependencyScanner = dependencyScanner ?? (() => DependencyScannerFactory.Create());
            var scanner = dependencyScanner();
            if (scanner == null)
            {
                throw new InvalidOperationException("No dependency scanner registered.  Make sure you call AddDependencyScanning before calling this method");
            }
            Action<IDependousConfiguration> internalBuilder = (config) =>
            {
                //automatically discovers AutoFac modules and Sources as well any DLR dynamic types
                //  config.AddAdditionalDiscoveryTypes(d => d.RegisterType<IModule>(SL.Singleton)
                //                                                                 .RegisterType<IRegistrationSource>(SL.Singleton));
                //  configurationBuilder?.Invoke(config);
            };
            var scanResults = scanner.Scan(patternBuilder, internalBuilder);
            logger($"Assembly Scan Completed In {scanResults.Duration.TotalMilliseconds}ms{Environment.NewLine} {String.Join(Environment.NewLine, scanResults.ScannedAssemblies.Select(x => x.FullName))}");

            logger($"Registering Dependencies {Environment.NewLine} { String.Join(Environment.NewLine, scanResults.Metadata.Select(x => x.DependencyType.FullName))}");
            if (scanResults.ScanExceptions != null && scanResults.ScanExceptions.Any())
            {
                foreach (var error in scanResults.ScanExceptions)
                {
                    logger(error);
                }
            }

            var containerRegistrationService = new GraceContainerRegistrationService(scanResults.Configuration);
            if (scanResults.Configuration.PersistScanResults)
            {
                containerRegistrationService.Builder.Configure(c => c.ExportInstance(scanResults).As<DependencyScanResult>().Lifestyle.SingletonPerRequest());
            }
            containerConfigurator?.Invoke(containerRegistrationService.Builder, scanResults.Metadata);
            var allRegistrations = containerRegistrationService.RegisterAll(scanResults.Metadata);
            logger($"Registered {allRegistrations.Count()} dependencies.");
            return containerRegistrationService.Builder;
        }
    }
}