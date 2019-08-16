#if NETSTANDARD2_0 || NETSTANDARD2_1

using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Dependous.Autofac
{
    internal class AutoFacServiceProviderFactory : IServiceProviderFactory<IContainer>
    {
        private readonly Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> _assemblyNameFilter;
        private readonly Action<IDependousConfiguration> _configurationBuilder;
        private readonly Action<object> _logger;
        private readonly Action<ContainerBuilder> _containerBuilder;

        internal AutoFacServiceProviderFactory(
            Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter = null,
                                                                                     Action<IDependousConfiguration> configurationBuilder = null,
                                                                                     Action<object> logger = null,
                                                                                     Action<ContainerBuilder> containerBuilder = null)
        {
            _assemblyNameFilter = assemblyNameFilter;
            _configurationBuilder = configurationBuilder;
            _logger = logger;
            _containerBuilder = containerBuilder;
        }

        public IContainer CreateBuilder(IServiceCollection services)
        {
            return services.BuildAutoFacContainer(_assemblyNameFilter, _configurationBuilder, _logger, _containerBuilder);
        }

        public IServiceProvider CreateServiceProvider(IContainer containerBuilder)
        {
            return containerBuilder.Resolve<IServiceProvider>();
        }
    }
}

#endif