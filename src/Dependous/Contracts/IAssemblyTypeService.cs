namespace Dependous
{
    using System.Reflection;
    using Dependous.Models;

    /// <summary>
    /// Retrieves types from an assembly
    /// </summary>
    public interface IAssemblyTypeService
    {
        /// <summary>
        /// Produces a collection of types from an assembly
        /// </summary>
        /// <param name="name">The assembly name.</param>
        /// <returns></returns>
        AssemblyTypeResult GetTypes(AssemblyName name);
        /// <summary>
        /// Produces a collection of types from an assembly
        /// </summary>
        /// <param name="name">The assembly name.</param>
        /// <returns></returns>
        AssemblyTypeResult GetTypes(string assemblyName);
    }
}