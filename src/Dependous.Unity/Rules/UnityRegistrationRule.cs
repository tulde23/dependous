using System.Collections.Generic;
using System.Text;
using Dependous.Unity.Contracts;
using Dependous.Unity.Models;
using Microsoft.Practices.Unity;

namespace Dependous.Unity.Rules
{
    internal class UnityRegistrationRule : UnityOpenGenericRule
    {
        public override RegistrationResult Register(IRegistrationRuleContext context)
        {
            var result = base.Register(context);
            if (result.RuleExecuted)
            {
                return result;
            }
            var results = new List<DependencyRegistration>();
            var sb = new StringBuilder();
            foreach (var interfaceType in context.Metadata.ImplementedInterfaces)
            {
                var namedDependency = context.Metadata.NamedDependency;

                var lifeTime = this.SetLifetime(context.Metadata.ServiceLifetime);

                if (namedDependency != null)
                {
                    context.Builder.RegisterType(interfaceType, context.Metadata.DependencyType.AsType(), namedDependency.ToString(), lifeTime, null);
                }
                else
                {
                    context.Builder.RegisterType(interfaceType, context.Metadata.DependencyType.AsType(), lifeTime);
                }

                results.Add(new DependencyRegistration
                {
                    DependencyTypeName = context.Metadata.DependencyType.FullName,
                    ImplementedInterfaceName = interfaceType.FullName,
                    DependencyKey = context.Metadata.NamedDependency,
                    ServiceLifetime = context.Metadata.ServiceLifetime.ToString(),
                    Debug = sb.ToString()
                });
            }
            return new RegistrationResult(this, true, results);
        }
    }
}