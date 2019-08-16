using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dependous.Models
{
    /// <summary>
    ///  Models the result from locating types in an assembly
    /// </summary>
    public class AssemblyTypeResult
    {
        /// <summary>
        /// Gets the loader exceptions.
        /// </summary>
        /// <value>
        /// The loader exceptions.
        /// </value>
        public IEnumerable<Exception> LoaderExceptions { get; }

        /// <summary>
        /// Gets the discovered types.
        /// </summary>
        /// <value>
        /// The discovered types.
        /// </value>
        public IEnumerable<Type> DiscoveredTypes { get; }

        /// <summary>
        /// Gets the loaded assembly.
        /// </summary>
        /// <value>
        /// The loaded assembly.
        /// </value>
        public Assembly LoadedAssembly { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyTypeResult" /> class.
        /// </summary>
        /// <param name="discoverdTypes">The discoverd types.</param>
        /// <param name="asssembly">The asssembly.</param>
        /// <param name="loaderExceptions">The loader exceptions.</param>
        public AssemblyTypeResult(IEnumerable<Type> discoverdTypes, Assembly asssembly, IEnumerable<Exception> loaderExceptions = null)
        {
            LoaderExceptions = loaderExceptions;
            DiscoveredTypes = discoverdTypes;
            LoadedAssembly = asssembly;
        }
    }
}