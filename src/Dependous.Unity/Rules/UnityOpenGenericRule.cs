using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dependous.Unity.Contracts;

//using Unity.Extras.DynamicProxy;
using Dependous.Unity.Models;

namespace Dependous.Unity.Rules
{
    internal class UnityOpenGenericRule : EnumerableRule
    {
        public override RegistrationResult Register(IRegistrationRuleContext context)
        {
            base.Register(context);
            //first lets see if the type in question is an open generic and if it implements an open generic interface?
            if (context.Metadata.DependencyType.IsGenericTypeDefinition)
            {
                var genericInterfaces = context.Metadata.ImplementedInterfaces.Where(x => x.IsGenericType);
                var results = new List<DependencyRegistration>(genericInterfaces.Count());
                var sb = new StringBuilder();

                return new RegistrationResult(this, true);
            }
            return new RegistrationResult(this, false);
        }
    }
}