using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Dependous.Autofac.Models;
using Dependous.Contracts;

namespace Dependous.Autofac.Rules
{
    internal class SelfRule : AbstractRegistrationRule
    {
        public SelfRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            if (dependencyMetadata.ImplementedInterfaces.Contains(typeof(ISelf)))
            {
                //we are using AutoFac's module registration
                var rb = Builder.RegisterType(dependencyMetadata.DependencyType.AsType()).AsSelf();
                return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
            }
            return new RegistrationResult(this, false);
        }
    }
}