using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Dependous.Autofac.Contracts;
using Dependous.Autofac.Models;
using Dependous.Autofac.Rules;

namespace Dependous
{
    /// <summary>
    /// Provides an AutoFac specific container registration service. Of note is the ability to
    /// register multiple implementations of the same interface distinguished by name.
    /// </summary>
    /// <seealso cref="IContainerRegistrationService"/>
    internal class AutoFacContainerRegistrationService
    {
        private readonly List<Type> ruleTypes = new List<Type>(4)
        {
            typeof(AutoFacModuleRegistrationRule),
            typeof(AutoFacRegistrationSourceRule),
            typeof(AutoFacRegistrationRule),
            typeof(SelfRule)
        };

        /// <summary>

        /// The builder </summary>
        public ContainerBuilder Builder { get; private set; }

        private readonly IDependousConfiguration _dependousConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFacContainerRegistrationService"/> class.
        /// </summary>
        public AutoFacContainerRegistrationService(IDependousConfiguration dependousConfiguration) : this(new ContainerBuilder())
        {
            _dependousConfiguration = dependousConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFacContainerRegistrationService"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public AutoFacContainerRegistrationService(ContainerBuilder builder)
        {
            this.Builder = builder;
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
                var registrationResults = rules.Select(x => x.Register(metadata));
                allResults.AddRange(registrationResults);
            }
            return allResults.SelectMany(x => x.Registrations);
        }

        /// <summary>
        /// Creates the container. Call this method after Register.
        /// </summary>
        /// <returns></returns>
        public IContainer CreateContainer()
        {
            return this.Builder.Build();
        }
    }
}