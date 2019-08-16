using System.Collections.Generic;
using Dependous.GraceIoc.Models;
using Grace.DependencyInjection;

namespace Dependous.GraceIoc.Rules
{
    internal class GraceRegistrationRule : GraceOpenGenericRule
    {
        public GraceRegistrationRule(DependencyInjectionContainer containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
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
            }
            return new RegistrationResult(this, true, results);
        }
    }
}