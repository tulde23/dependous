namespace Dependous
{
    /// <summary>
    ///
    /// </summary>
    public class DependencyRegistration
    {
        /// <summary>
        /// Gets or sets the name of the implemented interface.
        /// </summary>
        /// <value>
        /// The name of the implemented interface.
        /// </value>
        public string ImplementedInterfaceName { get; set; }

        /// <summary>
        /// Gets or sets the service lifetime.
        /// </summary>
        /// <value>
        /// The service lifetime.
        /// </value>
        public string ServiceLifetime { get; set; }

        /// <summary>
        /// Gets or sets the name of the dependency.
        /// </summary>
        /// <value>
        /// The name of the dependency.
        /// </value>
        public string DependencyTypeName { get; set; }

        /// <summary>
        /// Gets or sets the key/name used to register this dependency
        /// </summary>
        /// <value>
        /// The named dependency.
        /// </value>
        public object DependencyKey { get; set; }
    }
}