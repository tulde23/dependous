using System.Collections.Generic;
using Dependous.DefaultContainer.Contracts;
using Dependous.DefaultContainer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dependous.DefaultContainer.Rules
{
    internal abstract class AbstractRegistrationRule : IRegistrationRule
    {
        public IServiceCollection Services { get; }
        protected IDependousConfiguration Configuration { get; }

        public AbstractRegistrationRule(IServiceCollection services, IDependousConfiguration dependousConfiguration)
        {
            Services = services;
            Configuration = dependousConfiguration;
        }

        public abstract RegistrationResult Register(DependencyMetadata dependencyMetadata);

        protected IEnumerable<TModel> Produce<TModel>(params TModel[] items)
        {
            var list = new List<TModel>(items);
            return list;
        }
    }
}