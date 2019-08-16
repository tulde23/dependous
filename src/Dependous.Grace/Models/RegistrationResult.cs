using System.Collections.Generic;
using System.Linq;
using Dependous.GraceIoc.Contracts;

namespace Dependous.GraceIoc.Models
{
    internal class RegistrationResult
    {
        public RegistrationResult(IRegistrationRule rule, bool ruleExecuted, IEnumerable<DependencyRegistration> registrations = null)
        {
            Rule = rule;
            RuleExecuted = ruleExecuted;
            Registrations = registrations ?? Enumerable.Empty<DependencyRegistration>();
        }

        public IRegistrationRule Rule { get; }
        public bool RuleExecuted { get; }
        public IEnumerable<DependencyRegistration> Registrations { get; }
    }
}