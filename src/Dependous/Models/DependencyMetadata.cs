namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Models a discovered dependency
    /// </summary>
    public sealed class DependencyMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyMetadata"/> class.
        /// </summary>
        internal DependencyMetadata()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyMetadata" /> class.
        /// </summary>
        /// <param name="dependencyKey">Name/Key of the dependency.</param>
        /// <param name="implementedInterfaces">The implemented interfaces.</param>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <param name="discoveryInterface">The discovery interface.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="decorator">The decorator.</param>
        internal DependencyMetadata(object dependencyKey,
                                                IEnumerable<Type> implementedInterfaces,
                                                TypeInfo dependencyType,
                                                ServiceLifetime serviceLifetime,
                                                TypeInfo discoveryInterface,
                                                TypeInfo interceptor = null,
                                                TypeInfo decorator = null)
        {
            NamedDependency = dependencyKey;
            ImplementedInterfaces = new List<Type>(implementedInterfaces);
            DependencyType = dependencyType;
            ServiceLifetime = serviceLifetime;
            DiscoveryInterface = discoveryInterface;
            Interceptor = interceptor;
            Decorator = decorator;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public object NamedDependency { get; private set; }

        /// <summary>
        /// Gets or sets the service lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public ServiceLifetime ServiceLifetime { get; private set; }

        /// <summary>
        /// Gets the implemented interfaces.
        /// </summary>
        /// <value>
        /// The implemented interfaces.
        /// </value>
        public IEnumerable<Type> ImplementedInterfaces { get; private set; }

        /// <summary>
        /// Gets the type of the dependency.
        /// </summary>
        /// <value>
        /// The type of the dependency.
        /// </value>
        public TypeInfo DependencyType { get; private set; }

        /// <summary>
        /// Gets the discovery interface used to locate this dependency in an assembly
        /// </summary>
        /// <value>
        /// The discovery interface.
        /// </value>
        public TypeInfo DiscoveryInterface { get; private set; }

        /// <summary>
        /// Gets the interceptor.
        /// </summary>
        /// <value>
        /// The interceptor.
        /// </value>
        public TypeInfo Interceptor { get; private set; }

        /// <summary>
        /// Gets the decorator.
        /// </summary>
        /// <value>
        /// The interceptor.
        /// </value>
        public TypeInfo Decorator { get; private set; }

        public Func<IServiceProvider, object> FactoryMethod { get; }
    }
}