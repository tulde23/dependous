using System.Collections.Generic;
using System.Linq;
using Dependous.GraceIoc.Models;
using Grace.DependencyInjection;

namespace Dependous.GraceIoc.Rules
{
    internal class GraceOpenGenericRule : AbstractRegistrationRule
    {
        public GraceOpenGenericRule(DependencyInjectionContainer containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            //first lets see if the type in question is an open generic and if it implements an open generic interface?
            if (dependencyMetadata.DependencyType.IsGenericTypeDefinition)
            {
                var genericInterfaces = dependencyMetadata.ImplementedInterfaces.Where(x => x.IsGenericType);
                var results = new List<DependencyRegistration>(genericInterfaces.Count());
                foreach (var interfaceType in genericInterfaces)
                {
                    var namedDependency = dependencyMetadata.NamedDependency;
                    Builder.Configure(c =>
                    {
                        if (dependencyMetadata.Decorator != null)
                        {
                            var strategy = c.ExportDecorator(dependencyMetadata.DependencyType).As(interfaceType);
                        }
                        else
                        {
                            var strategy = c.Export(dependencyMetadata.DependencyType).As(interfaceType);
                            SetLifetime(dependencyMetadata.ServiceLifetime, strategy);
                            if (namedDependency != null)
                            {
                                strategy.AsKeyed(interfaceType, namedDependency);
                            }
                        }
                    });

                    results.Add(new DependencyRegistration
                    {
                        DependencyTypeName = dependencyMetadata.DependencyType.FullName,
                        ImplementedInterfaceName = interfaceType.FullName,
                        DependencyKey = dependencyMetadata.NamedDependency,
                        ServiceLifetime = dependencyMetadata.ServiceLifetime.ToString()
                    });
                    return new RegistrationResult(this, true, results);
                }
            }
            return new RegistrationResult(this, false);
        }
    }
}