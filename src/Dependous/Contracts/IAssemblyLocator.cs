namespace Dependous
{
    using System;
    using Dependous.Models;

    /// <summary>
    /// Locates assemblies to be scanned
    /// </summary>
    public interface IAssemblyLocator
    {
        /// <summary>
        /// Locates assemblies using the supplied search filter.
        /// </summary>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <returns></returns>
        AssemblyLocatorResult Locate(Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter, IDependousConfiguration dependousConfiguration);
    }
}