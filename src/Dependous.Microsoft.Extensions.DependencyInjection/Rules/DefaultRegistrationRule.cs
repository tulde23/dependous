using System.Collections.Generic;
using System.Text;
using Dependous.DefaultContainer.Models;
using sd = Microsoft.Extensions.DependencyInjection.ServiceDescriptor;
using sl = Microsoft.Extensions.DependencyInjection.ServiceLifetime;

namespace Dependous.Autofac.Rules
{
    internal class DefaultRegistrationRule
        : DefaultOpenGenericRule
    {
        public DefaultRegistrationRule(global::Microsoft.Extensions.DependencyInjection.IServiceCollection services, IDependousConfiguration dependousConfiguration) : base(services, dependousConfiguration)
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
            var sb = new StringBuilder();
            foreach (var interfaceType in dependencyMetadata.ImplementedInterfaces)
            {
                var namedDependency = dependencyMetadata.NamedDependency;

                sl lifeTime = sl.Transient;
                switch (dependencyMetadata.ServiceLifetime)
                {
                    case ServiceLifetime.Scoped:
                        lifeTime = sl.Scoped;
                        break;

                    case ServiceLifetime.Singleton:
                        lifeTime = sl.Singleton;
                        break;

                    case ServiceLifetime.Transient:
                        lifeTime = sl.Transient;
                        break;
                }

                Services.Add(new sd(interfaceType, dependencyMetadata.DependencyType, lifeTime));

                if (namedDependency != null)
                {
                    var text = $"builder.RegisterType<{dependencyMetadata.DependencyType.FullName}>(){lifeTime}.Keyed<{interfaceType.FullName}>({namedDependency}).As<{interfaceType.FullName}>()";

                    // rb.Keyed(namedDependency, interfaceType).As(interfaceType);
                    sb.AppendLine(text);
                }
                else
                {
                    var text = $"builder.RegisterType<{dependencyMetadata.DependencyType.FullName}>(){lifeTime}.As<{interfaceType.FullName}>()";

                    //rb.As(interfaceType);
                    sb.AppendLine(text);
                }

                results.Add(new DependencyRegistration
                {
                    DependencyTypeName = dependencyMetadata.DependencyType.FullName,
                    ImplementedInterfaceName = interfaceType.FullName,
                    DependencyKey = dependencyMetadata.NamedDependency,
                    ServiceLifetime = dependencyMetadata.ServiceLifetime.ToString(),
                    Debug = sb.ToString()
                });
            }
            return new RegistrationResult(this, true, results);
        }
    }
}