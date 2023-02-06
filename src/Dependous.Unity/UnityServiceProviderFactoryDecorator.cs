using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity;
using Unity.Microsoft.DependencyInjection;

namespace Dependous.Unity
{
    internal class UnityServiceProviderFactoryDecorator

        : IServiceProviderFactory<IUnityContainer>
    {
        private readonly AssemblyPaths _assemblyPaths;
        private readonly AssemblySearchPatternFactory _assemblySearchPatternFactory;
        private readonly Action<IDependousConfiguration> _configurationBuilder;

        private readonly Action<object> _logger;
        private readonly ServiceProviderFactory _factory;

        internal UnityServiceProviderFactoryDecorator(
            AssemblySearchPatternFactory assemblySearchPatternFactory,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<IUnityContainer> containerBuilder = null)
        {
            _assemblySearchPatternFactory = assemblySearchPatternFactory;
            _configurationBuilder = configurationBuilder;
            _logger = logger;
            _factory = new UnityServiceProvider(new UnityContainer());
        }

        internal UnityServiceProviderFactoryDecorator(
          AssemblyPaths assemblyPaths,
                                                                                   Action<IDependousConfiguration> configurationBuilder = null,
                                                                                   Action<object> logger = null,
                                                                                   Action<IUnityContainer> containerBuilder = null)
        {
            _assemblyPaths = assemblyPaths;
            _configurationBuilder = configurationBuilder;
            _logger = logger;
            _factory = new UnityServiceProvider(new UnityContainer());
        }

        public IUnityContainer CreateBuilder(IServiceCollection services)
        {
            PopulateUnityContainer(services, _configurationBuilder, _logger);
            return _factory.LifetimeScope;
        }

        public IServiceProvider CreateServiceProvider(IUnityContainer containerBuilder)
        {
            ServiceLocator.Configure(() => _factory);
            return _factory;
        }

        private IUnityContainer BuildContainer(

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
                //automatically discovers Unity modules and Sources as well any DLR dynamic types

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

            var containerRegistrationService = new UnityContainerRegistrationService(_factory, scanResults.Configuration);
            if (scanResults.Configuration.PersistScanResults)
            {
                containerRegistrationService.Builder.RegisterInstance(scanResults);
            }
            var allRegistrations = containerRegistrationService.RegisterAll(scanResults.Metadata);
            logger($"Registered {allRegistrations.Count()} dependencies.");
            return containerRegistrationService.Builder;
        }

        private void PopulateUnityContainer(IServiceCollection services,

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

            patternBuilder,
            () =>
            {
                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetService<IDependencyScanner>();
            },
                configurationBuilder,
                logger);

            foreach (var descriptor in services)
            {
                if (descriptor.ImplementationType != null)
                {
                    _factory.LifetimeScope.RegisterType(descriptor.ServiceType, descriptor.ImplementationType, GetManager(descriptor.Lifetime));
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    
                }
                else
                {
                    _factory.LifetimeScope.RegisterInstance(descriptor.ServiceType, descriptor.ImplementationInstance, GetManager(descriptor.Lifetime));
                }
                //if (descriptor.ImplementationType != null)
                //{
                //    // Test if the an open generic type is being registered
                //    var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                //    if (serviceTypeInfo.IsGenericTypeDefinition)
                //    {
                //    //    _factory.LifetimeScope.r
                //    //        .RegisterGeneric(descriptor.ImplementationType)
                //    //        .As(descriptor.ServiceType)
                //    //        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons);
                //    //}
                //    else
                //    {
                //        _factory.LifetimeScope
                //            .RegisterType(descriptor.ImplementationType, descriptor.ServiceType, )
                //            .As(descriptor.ServiceType)
                //            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons);
                //    }
                //}
                //else if (descriptor.ImplementationFactory != null)
                //{
                //    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                //    {
                //        var serviceProvider = context.Resolve<IServiceProvider>();
                //        return descriptor.ImplementationFactory(serviceProvider);
                //    })
                //        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                //        .CreateRegistration();

                //    builder.RegisterComponent(registration);
                //}
                //else
                //{
                //    builder
                //        .RegisterInstance(descriptor.ImplementationInstance)
                //        .As(descriptor.ServiceType)
                //        .ConfigureLifecycle(descriptor.Lifetime, null);
                //}
            }
        }

        private static LifetimeManager GetManager(Microsoft.Extensions.DependencyInjection.ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient: return new TransientLifetimeManager();
                case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton: return new ContainerControlledLifetimeManager();
                case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped: return new PerThreadLifetimeManager();
                default: return new TransientLifetimeManager();
            }
        }
    }
}