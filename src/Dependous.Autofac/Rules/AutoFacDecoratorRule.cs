﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Rules
{
    /// <summary>
    /// Registers dependencies as decorators.
    /// </summary>
    /// <seealso cref="Dependous.Autofac.Rules.AbstractRegistrationRule"/>
    internal class AutoFacDecoratorRule : AbstractRegistrationRule
    {
        public AutoFacDecoratorRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration) : base(containerBuilder, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {
            if (dependencyMetadata.ImplementedInterfaces.Any(x => x.IsGenericType))
            {
                var decoratorType = dependencyMetadata.ImplementedInterfaces.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(typeof(IDecorator<>)));
                if (decoratorType != null)
                {
                    var trueType = decoratorType.GetGenericArguments()[0];
                    Builder.RegisterDecorator(dependencyMetadata.DependencyType, trueType);
                    //we are using AutoFac's module registration
                    // var rb = Builder.RegisterType(dependencyMetadata.DependencyType.AsType()).AsSelf();
                    return new RegistrationResult(this, true, Produce(new DependencyRegistration { DependencyTypeName = dependencyMetadata.DependencyType.Name }));
                }
            }

            return new RegistrationResult(this, false);
        }
    }
}