using System;
using System.Collections.Generic;
using System.Linq;
using Dependous.Unity;
using Dependous.Unity.Contracts;
using Dependous.Unity.Models;
using Dependous.Unity.Rules;
using Microsoft.Practices.Unity;

namespace Dependous
{
    /// <summary>
    /// Provides an Unity specific container registration service. Of note is the ability to
    /// register multiple implementations of the same interface distinguished by name.
    /// </summary>
    /// <seealso cref="IContainerRegistrationService"/>
    internal class UnityContainerRegistrationService
    {
        private readonly List<Type> ruleTypes = new List<Type>(4)
        {
            typeof(UnityRegistrationRule),
            typeof(SelfRule),
        };

        /// <summary>

        /// The builder </summary>
        public IUnityContainer Builder => serviceProvider.LifetimeScope;

        private readonly UnityServiceProviderDecorator serviceProvider;
        private readonly IDependousConfiguration _dependousConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityContainerRegistrationService"/> class.
        /// </summary>
        public UnityContainerRegistrationService(UnityServiceProviderDecorator serviceProvider, IDependousConfiguration dependousConfiguration)
        {
            this.serviceProvider = serviceProvider;
            _dependousConfiguration = dependousConfiguration;
        }

        /// <summary>
        /// Registers all the specified services.
        /// </summary>
        /// <param name="dependencyMetadata">The dependency metadata.</param>
        /// <returns></returns>
        public IEnumerable<DependencyRegistration> RegisterAll(
            IEnumerable<DependencyMetadata> dependencyMetadata)
        {
            var rules = ruleTypes.Select(x => (IRegistrationRule)Activator.CreateInstance(x, this.Builder, _dependousConfiguration));
            var allResults = new List<RegistrationResult>();
            foreach (var metadata in dependencyMetadata)
            {
                var registrationResults = rules.Select(x => x.Register(new RegistrationRuleContext(serviceProvider, _dependousConfiguration, metadata)));
                allResults.AddRange(registrationResults);
            }
            return allResults.SelectMany(x => x.Registrations);
        }

        /// <summary>
        /// Creates the container. Call this method after Register.
        /// </summary>
        /// <returns></returns>
        public IUnityContainer CreateContainer()
        {
            return this.Builder;
        }
    }
}