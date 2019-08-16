namespace Dependous
{
    using System.Reflection;

    /// <summary>
    /// Base class implementation providing a default constructor for all implementations.
    /// </summary>
    /// <seealso cref="Dependous.IAssemblySearchPattern" />
    public abstract class BaseSearchPattern : IAssemblySearchPattern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSearchPattern"/> class.
        /// </summary>
        /// <param name="patternToMatch">The pattern to match.</param>
        protected BaseSearchPattern(string patternToMatch)
        {
            this.PatternToMatch = patternToMatch;
        }

        /// <summary>
        /// Gets or sets the pattern to match.
        /// </summary>
        /// <value>
        /// The pattern to match.
        /// </value>
        protected string PatternToMatch { get; set; }

        /// <summary>
        /// Matches the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>bool</returns>
        public abstract bool Match(AssemblyName assemblyName);

        /// <summary>
        /// Matches the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>bool</returns>
        public abstract bool Match(string assemblyName);
    }
}