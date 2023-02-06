using Microsoft.Practices.Unity;

namespace Dependous.Unity.Contracts
{
    public interface IRegistrationRuleContext
    {
        UnityServiceProviderDecorator ServiceProvider { get; }
        IDependousConfiguration DependousConfiguration { get; }

        IUnityContainer Builder { get; }

        DependencyMetadata Metadata { get; }
    }

    public class RegistrationRuleContext : IRegistrationRuleContext
    {
        public RegistrationRuleContext(UnityServiceProviderDecorator serviceProvider, IDependousConfiguration dependousConfiguration, DependencyMetadata dependencyMetadata)
        {
            ServiceProvider = serviceProvider;
            DependousConfiguration = dependousConfiguration;
            Metadata = dependencyMetadata;
        }

        public UnityServiceProviderDecorator ServiceProvider { get; }
        public IDependousConfiguration DependousConfiguration { get; }
        public IUnityContainer Builder => ServiceProvider.LifetimeScope;
        public DependencyMetadata Metadata { get; }
    }
}