namespace Dependous
{
    using System.Reflection;

    /// <summary>
    /// Defines a simple abstraction for filtering assembly names during the assembly scan.
    /// </summary>
    public interface IAssemblySearchPattern
    {
        /// <summary>
        /// Matches the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>bool</returns>
        bool Match(AssemblyName assemblyName);

        /// <summary>
        /// Matches the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>bool</returns>
        bool Match(string assemblyName);
    }
}