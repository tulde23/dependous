using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Rules
{
    internal class AutoFacRegistrationSourceRule : AbstractRegistrationRule
    {
        public AutoFacRegistrationSourceRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            if (dependencyMetadata.ImplementedInterfaces.Contains(typeof(IRegistrationSource)))
            {
                //we are using AutoFac's module registration
                IRegistrationSource module = Activator.CreateInstance(dependencyMetadata.DependencyType.AsType()) as IRegistrationSource;

                if (module != null)
                {
                    Builder.RegisterSource(module);
                    return new RegistrationResult(this, true,Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
                }
            }
            return new RegistrationResult(this, false);
        }
    }
}