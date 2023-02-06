using Dependous.Unity.Models;

namespace Dependous.Unity.Contracts
{
    internal interface IRegistrationRule
    {
        RegistrationResult Register(IRegistrationRuleContext context);
    }
}