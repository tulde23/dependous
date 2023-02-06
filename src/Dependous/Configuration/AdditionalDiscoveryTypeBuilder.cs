namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dependous.Attributes;

    /// <summary>
    /// Builds up a collection of additional targets using a fluent syntax.  If you require additional discovery interfaces beyond the default 3
    /// (ITransient, ISingleton, IScoped), use this class to define and register them with Dependous.
    /// This is useful if for example you want to automatically scan for all MVC/WebAPI controllers without the requirement of implementing one of the
    /// default interfaces.  This also provides fine grained control of how your dependencies get scanned.
    /// </summary>
    public class AdditionalDiscoveryTypeBuilder
    {
        /// <summary>
        /// The additional discovery types
        /// </summary>
        private readonly IList<AdditionalDiscoveryType> additionalDiscoveryTypes = new List<AdditionalDiscoveryType>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDiscoveryTypeBuilder"/> class.
        /// </summary>
        internal AdditionalDiscoveryTypeBuilder()
        {
        }

        /// <summary>
        /// Registers the type as a discovery type.  Furthermore, you must indicate what lifetime management policy it will use.
        /// </summary>
        /// <typeparam name="T">A type</typeparam>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <returns>
        /// an instance of AdditionalDiscoveryTypeBuilder
        /// </returns>
        public AdditionalDiscoveryTypeBuilder RegisterType<T>(ServiceLifetime serviceLifetime)
        {
            this.additionalDiscoveryTypes.Add(new AdditionalDiscoveryType
            {
                InterfaceType = typeof(T),
                ServiceLifetime = serviceLifetime
            });
            return this;
        }

        public AdditionalDiscoveryTypeBuilder RegisterAttribute<T>(Func<T, DependencyAttribute> mapper) where T : Attribute
        {
            this.additionalDiscoveryTypes.Add(new AdditionalDiscoveryType<T>(mapper));
            return this;
        }

        /// <summary>
        /// Retrieves a collection of all registered additional discovery types.
        /// </summary>
        /// <returns>an IEnumerable of AdditionalDiscoveryType</returns>
        internal IEnumerable<AdditionalDiscoveryType> GetTypes()
        {
            return this.additionalDiscoveryTypes.Distinct(new AdditionalDiscoveryType());
        }
    }
}