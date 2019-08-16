using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Dependous.Autofac.Contracts;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Rules
{
    internal abstract class AbstractRegistrationRule : IRegistrationRule
    {
        protected ContainerBuilder Builder { get; }
        protected IDependousConfiguration Configuration { get; }

        public AbstractRegistrationRule(ContainerBuilder containerBuilder, IDependousConfiguration dependousConfiguration)
        {
            Builder = containerBuilder;
            Configuration = dependousConfiguration;
        }

        public abstract RegistrationResult Register(DependencyMetadata dependencyMetadata);

        protected IEnumerable<TModel> Produce<TModel>(params TModel[] items)
        {
            var list = new List<TModel>(items);
            return list;
        }

        protected void SetLifetime(ServiceLifetime serviceLifetime, IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registrationBuilder)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    registrationBuilder.OwnedByLifetimeScope();
                    break;

                case ServiceLifetime.Singleton:
                    registrationBuilder.SingleInstance();

                    break;

                case ServiceLifetime.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }
        }

        protected void SetLifetime(ServiceLifetime serviceLifetime, IRegistrationBuilder<object, ReflectionActivatorData, SingleRegistrationStyle> registrationBuilder)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    registrationBuilder.OwnedByLifetimeScope();
                    break;

                case ServiceLifetime.Singleton:
                    registrationBuilder.SingleInstance();

                    break;

                case ServiceLifetime.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }
        }
    }
}