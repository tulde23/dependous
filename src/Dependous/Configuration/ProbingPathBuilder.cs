using System;
using System.Collections.Generic;

namespace Dependous.Configuration
{
    public class ProbingPathBuilder
    {
        private IList<ProbingPath> paths = new List<ProbingPath>();

        internal ProbingPathBuilder()
        {
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="probingPath">The probing path.</param>
        /// <param name="func">Allows you to pass a scoped search pattern for the current probing path.</param>
        /// <returns></returns>
        public ProbingPathBuilder AddProbingPath(string probingPath, Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> func = null)
        {
            this.paths.Add(new ProbingPath(probingPath, func));
            return this;
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <returns>an IEnumerable of AdditionalDiscoveryType</returns>
        internal IEnumerable<ProbingPath> GetPaths()
        {
            return this.paths;
        }
    }
}