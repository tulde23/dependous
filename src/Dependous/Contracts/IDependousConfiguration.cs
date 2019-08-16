/// <summary>
///
/// </summary>
namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using Dependous.Configuration;

    /// <summary>
    ///  Interface declaration for dependous configuration
    /// </summary>
    public interface IDependousConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to persist the dependous scan results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [persist scan results]; otherwise, <c>false</c>.
        /// </value>
        bool PersistScanResults { get; set; }

        /// <summary>
        /// Gets the additional types.
        /// </summary>
        /// <value>
        /// The additional types.
        /// </value>
        IEnumerable<AdditionalDiscoveryType> AdditionalTypes { get; }

        /// <summary>
        /// Gets  the interceptable types.
        /// </summary>
        /// <value>
        /// The interceptable types.
        /// </value>
        IEnumerable<InterceptableType> InterceptableTypes { get; }

        /// <summary>
        /// Gets or sets the probing paths.  These are used to discover assemblies not in bin.  Essentially implements a "plug-in" mechanism.
        /// </summary>
        /// <value>
        /// The probing paths.
        /// </value>
        IEnumerable<ProbingPath> ProbingPaths { get; }

        /// <summary>
        /// Registers additional discovery type interfaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void AddAdditionalDiscoveryTypes(Action<AdditionalDiscoveryTypeBuilder> builder);

        /// <summary>
        /// Registers a type as interceptable.  This allows you to proxy or intercept a class at runtime.  This assumes you are using class interception and all methods and properties are marked virtual
        /// </summary>
        /// <param name="builder">The builder.</param>
        void AddInterceptionTypes(Action<InterceptableTypeBuilder> builder);

        /// <summary>
        /// Configures probing paths
        /// </summary>
        /// <param name="builder">The builder.</param>
        void AddProbingPaths(Action<ProbingPathBuilder> builder);
    }
}