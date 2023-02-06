using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Autofac.Extras.DynamicProxy;
using Dependous.DefaultContainer.Models;
using Dependous.DefaultContainer.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Dependous.Autofac.Rules
{
    internal class DefaultOpenGenericRule
        : AbstractRegistrationRule
    {
        public DefaultOpenGenericRule(IServiceCollection services, IDependousConfiguration dependousConfiguration) : base(services, dependousConfiguration)
        {
        }

        public override RegistrationResult Register(DependencyMetadata dependencyMetadata)
        {

            //not yet implemented.
            ////first lets see if the type in question is an open generic and if it implements an open generic interface?
            if (dependencyMetadata.DependencyType.IsGenericTypeDefinition)
            {
                var genericInterfaces = dependencyMetadata.ImplementedInterfaces.Where(x => x.IsGenericType);
                var results = new List<DependencyRegistration>(genericInterfaces.Count());
                var sb = new StringBuilder();
                //foreach (var interfaceType in genericInterfaces)
                //{
                //    var rb = Builder.RegisterGeneric(dependencyMetadata.DependencyType);
                //    this.SetLifetime(dependencyMetadata.ServiceLifetime, rb);// this.CreateGenericRegistrationWithLifetime(dependencyMetadata.DependencyType, dependencyMetadata.ServiceLifetime);
                //    var namedDependency = dependencyMetadata.NamedDependency;

                //    if (namedDependency != null)
                //    {
                //        rb.Keyed(namedDependency, interfaceType).As(interfaceType);
                //        if (dependencyMetadata.Decorator != null)
                //        {
                //            //                builder.RegisterDecorator<INationalAffiliationLookupService>(
                //            //    (c, inner) => new NationalAffiliationLookupServiceDecorator(inner, c.Resolve<ICache>()),
                //            //    fromKey: "decoratable"
                //            //).As<IExpirable>().As<INationalAffiliationLookupService>().SingleInstance();
                //        }
                //    }
                //    else
                //    {
                //        rb.As(interfaceType);
                //    }

                //    //user has defined interception
                //    if (dependencyMetadata.Interceptor != null)
                //    {
                //        // rb.InterceptedBy(dependencyMetadata.Interceptor);
                //    }
                //    else if (Configuration.InterceptableTypes != null && Configuration.InterceptableTypes.Any())
                //    {
                //        var classInterception = Configuration.InterceptableTypes.FirstOrDefault(x => x.TypeToIntercept.IsAssignableFrom(dependencyMetadata.DependencyType));
                //        if (classInterception != null)
                //        {
                //            // rb.InterceptedBy(classInterception.InterceptionType);
                //        }
                //    }
                //    results.Add(new DependencyRegistration
                //    {
                //        DependencyTypeName = dependencyMetadata.DependencyType.FullName,
                //        ImplementedInterfaceName = interfaceType.FullName,
                //        DependencyKey = dependencyMetadata.NamedDependency,
                //        ServiceLifetime = dependencyMetadata.ServiceLifetime.ToString()
                //    });
                //}
                return new RegistrationResult(this, true);
            }
            return new RegistrationResult(this, false);
        }
    }
}