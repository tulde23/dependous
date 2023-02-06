namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using Dependous.Attributes;

    /// <summary>
    /// A class used to define an additional discovery interface to be used by the scanner.
    /// </summary>
    public class AdditionalDiscoveryType : IEqualityComparer<AdditionalDiscoveryType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDiscoveryType"/> class.
        /// </summary>
        internal AdditionalDiscoveryType()
        {
        }

        /// <summary>
        /// Gets or sets the type of the interface.
        /// </summary>
        /// <value>
        /// The type of the interface.
        /// </value>
        public Type InterfaceType { get; set; }

        /// <summary>
        /// Gets or sets the service lifetime.
        /// </summary>
        /// <value>
        /// The service lifetime.
        /// </value>
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        /// Equals the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(AdditionalDiscoveryType x, AdditionalDiscoveryType y)
        {
            return x.InterfaceType.Equals(y.InterfaceType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public int GetHashCode(AdditionalDiscoveryType obj)
        {
            return 1;
        }
    }

    public class AdditionalDiscoveryType<T> : AdditionalDiscoveryType where T : Attribute
    {
        public AdditionalDiscoveryType(Func<T, DependencyAttribute> attributeMapper)
        {
            AttributeMapper = attributeMapper;
        }

        public Func<T, DependencyAttribute> AttributeMapper { get; }
    }
}