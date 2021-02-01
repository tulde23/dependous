using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SL = Dependous.ServiceLifetime;

namespace Dependous.Autofac
{
    internal class AutoFacServiceProviderFactoryDecorator : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly AssemblyPaths _assemblyPaths;
        private readonly AssemblySearchPatternFactory _assemblySearchPatternFactory;
        private readonly Action<IDependousConfiguration> _configurationBuilder;
        private readonly AutofacServiceProviderFactory _decorateer;
        private readonly Action<object> _logger;

        internal AutoFacServiceProviderFactoryDecorator(
            AssemblySearchPatternFactory assemblySearchPatternFactory,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            _assemblySearchPatternFactory = assemblySearchPatternFactory;
            _configurationBuilder = configurationBuilder;
            _logger = logger;
            _decorateer = new AutofacServiceProviderFactory(containerBuilder);
         
        }

        internal AutoFacServiceProviderFactoryDecorator(
          AssemblyPaths assemblyPaths,
                                                                                   Action<IDependousConfiguration> configurationBuilder = null,
                                                                                   Action<object> logger = null,
                                                                                   Action<ContainerBuilder> containerBuilder = null)
        {
            _assemblyPaths = assemblyPaths;
            _configurationBuilder = configurationBuilder;
            _logger = logger ?? ((x) => { });
            _decorateer = new AutofacServiceProviderFactory(containerBuilder);
            
        }

        public static ContainerBuilder BuildContainer(
          ContainerBuilder containerBuilder,
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
                //automatically discovers AutoFac modules and Sources as well any DLR dynamic types
                config.AddAdditionalDiscoveryTypes(d => d.RegisterType<IModule>(SL.Singleton)
                                                                                  .RegisterType<IRegistrationSource>(SL.Singleton));
                configurationBuilder?.Invoke(config);
            };
            var scanResults = scanner.Scan(patternBuilder, internalBuilder, logger);
            logger($"Assembly Scan Completed In {scanResults.Duration.TotalMilliseconds}ms{Environment.NewLine} {String.Join(Environment.NewLine, scanResults.ScannedAssemblies.Select(x => x.FullName))}");

            logger($"Registering Dependencies {Environment.NewLine} { String.Join(Environment.NewLine, scanResults.Metadata.Select(x => x.DependencyType.FullName))}");
            if (scanResults.ScanExceptions != null && scanResults.ScanExceptions.Any())
            {
                foreach (var error in scanResults.ScanExceptions)
                {
                    logger(error);
                }
            }

            var containerRegistrationService = new AutoFacContainerRegistrationService(containerBuilder, scanResults.Configuration);
            if (scanResults.Configuration.PersistScanResults)
            {
                containerRegistrationService.Builder.RegisterInstance(scanResults);
            }
            var allRegistrations = containerRegistrationService.RegisterAll(scanResults.Metadata);
            logger($"Registered {allRegistrations.Count()} dependencies.");
            return containerRegistrationService.Builder;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = _decorateer.CreateBuilder(services);
            PopulateAutoFacContainer(services, builder, _configurationBuilder, _logger);
            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            var sp = _decorateer.CreateServiceProvider(containerBuilder);
            ServiceLocator.Configure(() => sp);
            return sp;
        }

        private void PopulateAutoFacContainer(IServiceCollection services,
                                                                                            ContainerBuilder containerBuilder,
                                                                                            Action<IDependousConfiguration> configurationBuilder = null,
                                                                                            Action<object> logger = null)
        {
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> patternBuilder = (b) =>
            {
                AssemblySearchPatternFactory.Merge(_assemblySearchPatternFactory, b);
                AssemblySearchPatternFactory.Merge(_assemblyPaths, b);
                return b;
            };

            BuildContainer(
            containerBuilder,
            patternBuilder,
            () =>
            {
                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetService<IDependencyScanner>();
            },
                configurationBuilder,
                logger);
        }
    }
}