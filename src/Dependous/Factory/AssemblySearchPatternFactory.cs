namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A Fluent API for building search predicates that will be used to filter the list of assemblies to scan.
    /// </summary>
    public sealed class AssemblySearchPatternFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblySearchPatternFactory"/> class.
        /// </summary>
        internal AssemblySearchPatternFactory()
        {
            this.Patterns = new List<IAssemblySearchPattern>();
        }

        /// <summary>
        /// Gets the list of patterns.
        /// </summary>
        /// <value>
        /// The patterns.
        /// </value>
        public List<IAssemblySearchPattern> Patterns
        {
            get;
            private set;
        }

        /// <summary>
        /// The assembly name contains the pattern
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>SearchPatternFactory</returns>
        public AssemblySearchPatternFactory Contains(string pattern)
        {
            this.Patterns.Add(new Contains(pattern));
            return this;
        }

        /// <summary>
        /// The assembly name ends with the pattern
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>SearchPatternFactory</returns>
        public AssemblySearchPatternFactory EndsWith(string pattern)
        {
            this.Patterns.Add(new EndsWith(pattern));
            return this;
        }

        /// <summary>
        /// The assembly name is the same as the entry assembly
        /// </summary>
        /// <returns>SearchPatternFactory</returns>
        public AssemblySearchPatternFactory Self()
        {
            this.Patterns.Add(new Self());
            return this;
        }

        /// <summary>
        /// The assembly name starts with the pattern
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>SearchPatternFactory</returns>
        public AssemblySearchPatternFactory StartsWith(string pattern)
        {
            this.Patterns.Add(new StartsWith(pattern));
            return this;
        }

        /// <summary>
        /// Add a custom search pattern.
        /// </summary>
        /// <param name="pattern">The pattern delegate.</param>
        /// <returns></returns>
        public AssemblySearchPatternFactory Custom(Func<string, bool> pattern)
        {
            this.Patterns.Add(new Custom(pattern));
            return this;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static AssemblySearchPatternFactory Create()
        {
            return new AssemblySearchPatternFactory();
        }

        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void Merge(AssemblySearchPatternFactory source, AssemblySearchPatternFactory target)
        {
            if (source != null && target != null)
            {
                target.Patterns.AddRange(source.Patterns);
            }
        }

        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void Merge(AssemblyPaths source, AssemblySearchPatternFactory target)
        {
            if (source?.Paths?.Any() == true && target != null)
            {
                foreach (var item in source.Paths)
                {
                    target.StartsWith(item);
                }
            }
        }
    }
}