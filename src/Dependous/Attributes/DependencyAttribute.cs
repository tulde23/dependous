using System;

namespace Dependous.Attributes
{
    /// <summary>
    ///     The attribute will indicate services we want to resolve automatically
    /// </summary>
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        ///     Designated constructor
        /// </summary>
        /// <param name="resolveType">Resolve type</param>
        public DependencyAttribute(Type resolveType)
        {
            this.ResolveType = resolveType;
            this.LifeTime = ServiceLifetime.Singleton;
        }

        /// <summary>
        ///     Designated constructor
        /// </summary>
        /// <param name="resolveType">Resolve type</param>
        /// <param name="lifeTime">Service lifetime</param>
        public DependencyAttribute(Type resolveType, ServiceLifetime lifeTime = ServiceLifetime.Singleton, bool enumerationOnly = false)
        {
            this.ResolveType = resolveType;
            this.LifeTime = lifeTime;
            this.EnumerationOnly = enumerationOnly;
        }

        /// <summary>
        ///     Service lifetime
        /// </summary>
        public ServiceLifetime LifeTime { get; }

        /// <summary>
        ///     `true` implies the registration is available via enumeration only
        /// </summary>
        public bool EnumerationOnly { get; }

        /// <summary>
        ///     Type to resolve
        /// </summary>
        public Type ResolveType { get; }
    }
}