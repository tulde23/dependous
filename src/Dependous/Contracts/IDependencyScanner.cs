namespace Dependous
{
    using System;
    using Dependous.Models;

    /// <summary>
    /// Interface declaration for a dependency scanner.
    /// </summary>
    public interface IDependencyScanner
    {
        /// <summary>
        /// Iterates over a collection of assemblies looking for dependencies implementing any of the registered AdditionalDiscoveryTypes
        /// </summary>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// a model containing extracted metadata regarding your dependencies.
        /// </returns>
        DependencyScanResult Scan(
             Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter = null,
                          Action<IDependousConfiguration> configurationBuilder = null, Action<object> logger = null);

        /// <summary>
        /// Produces a collection of assemblies based on a search filter.
        /// </summary>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <param name="configurationBuilder">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        AssemblyLocatorResult DiscoverAssemblies(
                        Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter = null,
                        Action<IDependousConfiguration> configurationBuilder = null, Action<object> logger = null);
    }
}