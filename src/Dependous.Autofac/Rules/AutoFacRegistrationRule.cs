using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core.Resolving.Pipeline;
using Autofac.Extras.DynamicProxy;
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
            var sb = new StringBuilder();

            foreach (var interfaceType in dependencyMetadata.ImplementedInterfaces.Where(x => !x.Equals(typeof(IResolveMiddleware))))
            {
                var namedDependency = dependencyMetadata.NamedDependency;
                
               
                var rb = Builder.RegisterType(dependencyMetadata.DependencyType.AsType());
                this.SetLifetime(dependencyMetadata.ServiceLifetime, rb);

                string lifeTime = "";
                switch (dependencyMetadata.ServiceLifetime)
                {
                    case ServiceLifetime.Scoped:
                        lifeTime = ".OwnedByLifetimeScope()";
                        break;

                    case ServiceLifetime.Singleton:
                        lifeTime = ".SingleInstance()";
                        break;

                    case ServiceLifetime.Transient:
                        lifeTime = ".InstancePerDependency()";
                        break;
                }

                if (namedDependency != null)
                {
                    var text = $"builder.RegisterType<{dependencyMetadata.DependencyType.FullName}>(){lifeTime}.Keyed<{interfaceType.FullName}>({namedDependency}).As<{interfaceType.FullName}>()";

                    rb.Keyed(namedDependency, interfaceType).As(interfaceType);

                    sb.AppendLine(text);
                }
                
                else
                {
                    var text = $"builder.RegisterType<{dependencyMetadata.DependencyType.FullName}>(){lifeTime}.As<{interfaceType.FullName}>()";

                    rb.As(interfaceType);

                    sb.AppendLine(text);
                }

                //user has defined interception
                if (dependencyMetadata.Interceptor != null)
                {
                     rb.EnableClassInterceptors();
                     rb.InterceptedBy(dependencyMetadata.Interceptor);
                    Builder.RegisterType(dependencyMetadata.Interceptor);
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