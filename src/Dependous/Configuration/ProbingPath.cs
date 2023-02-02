using System;
using System.Collections.Generic;
using System.Text;

namespace Dependous.Configuration
{
    /// <summary>
    /// Defines a probing path (a path used to find external assemblies)
    /// </summary>
    public class ProbingPath
    {
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; }
        public Func<AssemblySearchPatternFactory,AssemblySearchPatternFactory> Pattern { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbingPath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="func">The function.</param>
        internal ProbingPath(string path, Func<AssemblySearchPatternFactory,AssemblySearchPatternFactory> func)
        {
            Path = path;
            Pattern = func;
        }
    }
}
