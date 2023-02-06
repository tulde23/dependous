using System.Collections.Generic;
using Dependous.Unity.Contracts;
using Dependous.Unity.Models;

namespace Dependous.Unity.Rules
{
    internal class SelfRule : AbstractRegistrationRule
    {
        public override RegistrationResult Register(IRegistrationRuleContext context)
        {
            if (context.Metadata.DiscoveryInterface.Equals(typeof(ISelfTransient)))
            {
                //we are using Unity's module registration
                context.Builder.RegisterType(context.Metadata.DependencyType.AsType(), context.Metadata.DependencyType.AsType(), null, base.SetLifetime(ServiceLifetime.Transient), null);
                return new RegistrationResult(this, true, new List<DependencyRegistration> { new DependencyRegistration { DependencyTypeName = context.Metadata.DependencyType.Name } });
            }
            else if (context.Metadata.DiscoveryInterface.Equals(typeof(ISelfSingleton)))
            {
                //we are using Unity's module registration
                context.Builder.RegisterType(context.Metadata.DependencyType.AsType(), context.Metadata.DependencyType.AsType(), null, base.SetLifetime(ServiceLifetime.Singleton), null);
                return new RegistrationResult(this, true, new List<DependencyRegistration> { new DependencyRegistration { DependencyTypeName = context.Metadata.DependencyType.Name } });
            }
            return new RegistrationResult(this, false);
        }
    }
}