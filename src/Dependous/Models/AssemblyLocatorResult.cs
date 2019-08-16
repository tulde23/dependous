using System.Collections.Generic;
using System.Reflection;

namespace Dependous.Models
{
    /// <summary>
    /// Modesl the result from locating assemblies
    /// </summary>
    /// <seealso cref="Dependous.Models.Operation" />
    public class AssemblyLocatorResult : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLocatorResult"/> class.
        /// </summary>
        /// <param name="assemblyNames">The assembly names.</param>
        public AssemblyLocatorResult(IEnumerable<AssemblyName> assemblyNames)
        {
        
            DiscoveredAssemblyNames = assemblyNames;
        }

        /// <summary>
        /// Gets or sets the name of the discovered assembly.
        /// </summary>
        /// <value>
        /// The name of the discovered assembly.
        /// </value>
        public IEnumerable<AssemblyName> DiscoveredAssemblyNames { get; }
    }
}