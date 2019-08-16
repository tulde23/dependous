using System;
using System.Collections.Generic;
using System.Linq;
using Dependous.GraceIoc.Contracts;
using Dependous.GraceIoc.Models;
using Dependous.GraceIoc.Rules;
using Grace.DependencyInjection;

namespace Dependous
{
    /// <summary>
    /// Provides an AutoFac specific container registration service.  Of note is the ability to register multiple implementations of the same interface
    /// distinguished by name.
    /// </summary>
    /// <seealso cref="IContainerRegistrationService" />
    internal class GraceContainerRegistrationService
    {
        private readonly List<Type> ruleTypes = new List<Type>(4)
        {
          
            typeof(GraceRegistrationRule)
        };

        /// <summary>

        /// The builder
        /// </summary>
        public DependencyInjectionContainer Builder { get; private set; }

        private readonly IDependousConfiguration _dependousConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraceContainerRegistrationService"/> class.
        /// </summary>
        public GraceContainerRegistrationService(IDependousConfiguration dependousConfiguration) : this(new DependencyInjectionContainer())
        {
            _dependousConfiguration = dependousConfiguration;
           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraceContainerRegistrationService" /> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public GraceContainerRegistrationService(DependencyInjectionContainer builder)
        {

            builder = new DependencyInjectionContainer(c =>
            {
                c.AutoRegisterUnknown =true;
                c.Trace = (m) =>
                {
                    Console.WriteLine(m);
                };
                
            });
            Builder = builder;
         
          
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

      
    }
}