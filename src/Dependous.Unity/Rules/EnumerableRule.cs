using System;
using System.Collections.Generic;
using System.Linq;
using Dependous.Unity.Contracts;
using Dependous.Unity.Models;
using Microsoft.Practices.Unity;

namespace Dependous.Unity.Rules
{
    internal class EnumerableRule : AbstractRegistrationRule
    {
        public override RegistrationResult Register(IRegistrationRuleContext context)
        {
            // Unity considers a collection of registration as an array only. The general standard for DI is for IEnumerable<> type so to support it,
            // we're adding this registration for any given type
            context.Builder.RegisterType(
                typeof(IEnumerable<>),
                new InjectionFactory((IUnityContainer c, Type type, string name) => c.ResolveAll(type.GetGenericArguments().Single())));

            return new RegistrationResult(this, true);
        }
    }
}