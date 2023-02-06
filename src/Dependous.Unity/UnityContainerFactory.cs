using System;
using System.Collections.Generic;
using System.Linq;
using Dependous.Unity;
using Microsoft.Practices.Unity;

namespace Dependous.Unity
{
    /// <summary>
    ///
    /// </summary>
    internal static class UnityContainerFactory

    {
        /// <summary>
        /// Scans for dependencies and builds the unity container.
        /// </summary>
        /// <param name="patternBuilder">The pattern builder.</param>
        /// <param name="dependencyScanner">The dependency scanner.  If null, uses the default scanner</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="containerConfigurator">The container configurator.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">No dependency scanner registered.  Make sure you call AddDependencyScanning before calling this method</exception>
        public static IUnityContainer BuildContainer(

            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = null,
            Func<IDependencyScanner> dependencyScanner = null,
            Action<IDependousConfiguration> configurationBuilder = null,
            Action<object> logger = null,
            Action<IUnityContainer, IReadOnlyCollection<DependencyMetadata>> containerConfigurator = null)
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

            var containerRegistrationService = new UnityContainerRegistrationService(new UnityServiceProvider(new UnityContainer()), scanResults.Configuration);
            if (scanResults.Configuration.PersistScanResults)
            {
                containerRegistrationService.Builder.RegisterInstance(scanResults);
            }
            containerConfigurator?.Invoke(containerRegistrationService.Builder, scanResults.Metadata);
            var allRegistrations = containerRegistrationService.RegisterAll(scanResults.Metadata);
            logger($"Registered {allRegistrations.Count()} dependencies.");

            var container = containerRegistrationService.Builder;
            return container;
        }
    }
}