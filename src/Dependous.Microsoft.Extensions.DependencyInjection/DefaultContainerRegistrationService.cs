using System;
using System.Collections.Generic;
using System.Linq;
using Dependous.Autofac.Rules;
using Dependous.DefaultContainer.Contracts;
using Dependous.DefaultContainer.Models;
using Dependous.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dependous.DefaultContainer
{
    internal class DefaultContainerRegistrationService
    {
        private readonly DependencyScanResult dependencyScanResult;
        private readonly IServiceCollection services;

        private readonly List<Type> ruleTypes = new List<Type>(4)
        {
            typeof(DefaultRegistrationRule),
            typeof(SelfRule),
        };

        public DefaultContainerRegistrationService(DependencyScanResult dependencyScanResult, IServiceCollection services)
        {
            this.dependencyScanResult = dependencyScanResult;
            this.services = services;
        }

        public IEnumerable<DependencyRegistration> RegisterAll()
        {
            var rules = ruleTypes.Select(x => (IRegistrationRule)Activator.CreateInstance(x, services, dependencyScanResult.Configuration));
            var allResults = new List<RegistrationResult>();
            foreach (var metadata in this.dependencyScanResult.Metadata)
            {
                var registrationResults = rules.Select(x => x.Register(metadata));
                allResults.AddRange(registrationResults);
            }
            return allResults.SelectMany(x => x.Registrations);
        }
    }
}