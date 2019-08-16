namespace System.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Handy Assembly extension methods
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Gets the loadable types from an assembly.  This includes all protected, private and internal types.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="loaderExceptions">If assembly.GetTypes throws a ReflectionTypeLoadException, this will contain those loader exceptions</param>
        /// <returns>
        /// and IEnumerable of Type
        /// </returns>
        /// <exception cref="ArgumentNullException">thrown if assembly is null</exception>
        internal static IEnumerable<Type> GetLoadableTypes(this Assembly assembly, out Exception[] loaderExceptions)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                loaderExceptions = null;
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                loaderExceptions = e.LoaderExceptions;
                return e.Types.Where(t => t != null);
            }
        }
    }
}