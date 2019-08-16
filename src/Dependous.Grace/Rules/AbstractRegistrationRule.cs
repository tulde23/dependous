using System.Collections.Generic;
using Dependous.GraceIoc.Contracts;
using Dependous.GraceIoc.Models;
using Grace.DependencyInjection;

namespace Dependous.GraceIoc.Rules
{
    internal abstract class AbstractRegistrationRule : IRegistrationRule
    {
        protected DependencyInjectionContainer Builder { get; }
        protected IDependousConfiguration Configuration { get; }

        public AbstractRegistrationRule(DependencyInjectionContainer containerBuilder, IDependousConfiguration dependousConfiguration)
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

        protected void SetLifetime(ServiceLifetime serviceLifetime, IFluentExportStrategyConfiguration registrationBuilder)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    registrationBuilder.Lifestyle.SingletonPerScope();
                    break;

                case ServiceLifetime.Singleton:
                    registrationBuilder.Lifestyle.Singleton();

                    break;

                case ServiceLifetime.Transient:

                    break;
            }
        }
    }
}