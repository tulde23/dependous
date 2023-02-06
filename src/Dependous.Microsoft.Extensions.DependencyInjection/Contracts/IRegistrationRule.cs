using Dependous.DefaultContainer.Models;

namespace Dependous.DefaultContainer.Contracts
{
    internal interface IRegistrationRule
    {
        RegistrationResult Register(DependencyMetadata dependencyMetadata);
    }
}