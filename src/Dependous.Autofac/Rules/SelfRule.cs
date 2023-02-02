using Autofac;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Rules
{
    internal class SelfRule : AbstractRegistrationRule
    {
        public SelfRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            if (dependencyMetadata.DiscoveryInterface.Equals(typeof(ISelfTransient)))
            {
                //we are using AutoFac's module registration
                Builder.RegisterType(dependencyMetadata.DependencyType.AsType()).AsSelf().InstancePerDependency();
                return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
            }
            else if (dependencyMetadata.DiscoveryInterface.Equals(typeof(ISelfSingleton)))
            {
                //we are using AutoFac's module registration
                Builder.RegisterType(dependencyMetadata.DependencyType.AsType()).AsSelf().SingleInstance();
                return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
            }
            return new RegistrationResult(this, false);
        }
    }
}