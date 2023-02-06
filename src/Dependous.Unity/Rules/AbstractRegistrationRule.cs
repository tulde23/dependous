using Dependous.Unity.Contracts;
using Dependous.Unity.Models;
using Microsoft.Practices.Unity;

namespace Dependous.Unity.Rules
{
    internal abstract class AbstractRegistrationRule : IRegistrationRule
    {
        public abstract RegistrationResult Register(IRegistrationRuleContext context);

        protected LifetimeManager SetLifetime(ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    return new PerThreadLifetimeManager();

                case ServiceLifetime.Singleton:
                    return new ContainerControlledLifetimeManager();

                case ServiceLifetime.Transient:
                    return new TransientLifetimeManager();

                default:
                    return new TransientLifetimeManager();
            }
        }
    }
}