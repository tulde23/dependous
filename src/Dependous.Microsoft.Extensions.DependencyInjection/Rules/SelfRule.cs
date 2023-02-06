using Dependous.DefaultContainer.Models;
using Dependous.DefaultContainer.Rules;
using sd = Microsoft.Extensions.DependencyInjection.ServiceDescriptor;
using sl = Microsoft.Extensions.DependencyInjection.ServiceLifetime;

namespace Dependous.Autofac.Rules
{
    internal class SelfRule : AbstractRegistrationRule
    {
        public SelfRule(global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, IDependousConfiguration dependousConfiguration) : base(services, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            if (dependencyMetadata.DiscoveryInterface.Equals(typeof(ISelfTransient)))
            {
                //we are using AutoFac's module registration
                Services.Add(new sd(dependencyMetadata.DependencyType, dependencyMetadata.DependencyType, sl.Transient));
                return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
            }
            else if (dependencyMetadata.DiscoveryInterface.Equals(typeof(ISelfSingleton)))
            {
                //we are using AutoFac's module registration
                Services.Add(new sd(dependencyMetadata.DependencyType, dependencyMetadata.DependencyType, sl.Singleton));
                return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
            }
            return new RegistrationResult(this, false);
        }
    }
}