namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using Dependous.Configuration;

    /// <summary>
    /// Provides configuration options for Dependous
    /// </summary>
    public class DependousConfiguration : IDependousConfiguration
    {
        private readonly AdditionalDiscoveryTypeBuilder typeBuilder = new AdditionalDiscoveryTypeBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="IDependousConfiguration"/> class. This will
        /// register the default discovery type interfaces
        /// </summary>
        public DependousConfiguration()
        {
            this.AdditionalTypes = new List<AdditionalDiscoveryType>();

            this.AddAdditionalDiscoveryTypes(b =>
                b.RegisterType<ISingletonDependency>(ServiceLifetime.Singleton)
                  .RegisterType<ITransientDependency>(ServiceLifetime.Transient)
                  .RegisterType<ISelfTransient>(ServiceLifetime.Transient)
                  .RegisterType<ISelfSingleton>(ServiceLifetime.Singleton)
                  .RegisterType<IScopedDependency>(ServiceLifetime.Scoped));
        }

        /// <summary>
        /// Gets or sets the additional types.
        /// </summary>
        /// <value>The additional types.</value>
        public IEnumerable<AdditionalDiscoveryType> AdditionalTypes { get; private set; }

        /// <summary>
        /// Gets the interceptable types.
        /// </summary>
        /// <value>The interceptable types.</value>
        public IEnumerable<InterceptableType> InterceptableTypes { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to persist the dependous scan results.
        /// </summary>
        /// <value><c>true</c> if [persist scan results]; otherwise, <c>false</c>.</value>
        public bool PersistScanResults { get; set; }

        /// <summary>
        /// If true, attempts to the registration code.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [generate registration code]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateRegistrationCode { get; set; }

        /// <summary>
        /// Gets or sets the probing paths. These are used to discover assemblies not in bin.
        /// Essentially implements a "plug-in" mechanism.
        /// </summary>
        /// <value>The probing paths.</value>
        public IEnumerable<ProbingPath> ProbingPaths { get; private set; }

        /// <summary>
        /// Builds additional discovery types.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void AddAdditionalDiscoveryTypes(Action<AdditionalDiscoveryTypeBuilder> builder)
        {
            builder(this.typeBuilder);
            this.AdditionalTypes = this.typeBuilder.GetTypes();
        }

        /// <summary>
        /// Registers a type as interceptable
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void AddInterceptionTypes(Action<InterceptableTypeBuilder> builder)
        {
            var b = new InterceptableTypeBuilder();
            builder(b);
            this.InterceptableTypes = b.GetTypes();
        }

        /// <summary>
        /// Configures probing paths
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void AddProbingPaths(Action<ProbingPathBuilder> builder)
        {
            var b = new ProbingPathBuilder();
            builder(b);
            this.ProbingPaths = b.GetPaths();
        }
    }
}