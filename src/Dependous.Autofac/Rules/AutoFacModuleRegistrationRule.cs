using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Rules
{
    internal class AutoFacModuleRegistrationRule : AbstractRegistrationRule
    {
        public AutoFacModuleRegistrationRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            if (dependencyMetadata.ImplementedInterfaces.Contains(typeof(IModule)))
            {
                //we are using AutoFac's module registration
                IModule module = Activator.CreateInstance(dependencyMetadata.DependencyType.AsType()) as IModule;

                if (module != null)
                {
                    Builder.RegisterModule(module);
                    return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
                }
            }
            return new RegistrationResult(this, false);
        }
    }
}