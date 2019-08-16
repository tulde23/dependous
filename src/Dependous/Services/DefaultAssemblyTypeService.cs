namespace Dependous
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Dependous.Models;

    /// <summary>
    /// Default implementation of IAssemblyTypeService.
    /// </summary>
    /// <seealso cref="IAssemblyTypeService" />
    internal class DefaultAssemblyTypeService : IAssemblyTypeService
    {
        /// <summary>
        /// Produces a collection of types from an assembly excluding abstract classes and interfaces.
        /// </summary>
        /// <param name="name">The assembly name.</param>
        /// <returns></returns>
        public AssemblyTypeResult GetTypes(AssemblyName name)
        {
            Assembly loadedAssembly = null;
            if (!string.IsNullOrEmpty(name.CodeBase) && File.Exists(name.CodeBase))
            {
                loadedAssembly = Assembly.LoadFile(name.CodeBase);
            }
            else
            {
                loadedAssembly = Assembly.Load(name);
            }

            Exception[] loaderExceptions = null;
            return new AssemblyTypeResult(
                loadedAssembly.GetLoadableTypes(out loaderExceptions).Where(x => !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsInterface),
                loadedAssembly,
                loaderExceptions);
        }

        public AssemblyTypeResult GetTypes(string assemblyName)
        {
            var loadedAssembly = Assembly.LoadFile(assemblyName);
            Exception[] loaderExceptions = null;
            return new AssemblyTypeResult(
                loadedAssembly.GetLoadableTypes(out loaderExceptions).Where(x => !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsInterface),
                loadedAssembly,
                loaderExceptions);
        }
    }
}