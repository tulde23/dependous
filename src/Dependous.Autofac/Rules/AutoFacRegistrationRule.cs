using System.Collections.Generic;
using Autofac;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Rules
{
    internal class AutoFacRegistrationRule : AutoFacOpenGenericRule
    {
        public AutoFacRegistrationRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            var result = base.Register(dependencyMetadata);
            if (result.RuleExecuted)
            {
                return result;
            }
            var results = new List<DependencyRegistration>();

            foreach (var interfaceType in dependencyMetadata.ImplementedInterfaces)
            {
                var namedDependency = dependencyMetadata.NamedDependency;
                var rb = Builder.RegisterType(dependencyMetadata.DependencyType.AsType());
                this.SetLifetime(dependencyMetadata.ServiceLifetime, rb);

                if (namedDependency != null)
                {
                    rb.Keyed(namedDependency, interfaceType).As(interfaceType);
                }
                else
                {
                    rb.As(interfaceType);
                }

                //user has defined interception
                if (dependencyMetadata.Interceptor != null)
                {
                    // rb.EnableClassInterceptors();
                    //  rb.InterceptedBy(dependencyMetadata.Interceptor);
                    Builder.RegisterType(dependencyMetadata.Interceptor);
                }

                results.Add(new DependencyRegistration
                {
                    DependencyTypeName = dependencyMetadata.DependencyType.FullName,
                    ImplementedInterfaceName = interfaceType.FullName,
                    DependencyKey = dependencyMetadata.NamedDependency,
                    ServiceLifetime = dependencyMetadata.ServiceLifetime.ToString()
                });
            }
            return new RegistrationResult(this, true, results);
        }
    }
}