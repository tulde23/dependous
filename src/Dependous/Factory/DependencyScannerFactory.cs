using System;
using Dependous.Factory;

namespace Dependous
{
    /// <summary>
    ///  A factory class for creating instances of IDependencyScanner
    /// </summary>
    public static class DependencyScannerFactory
    {
        /// <summary>
        /// Creates an instance of an IDependencyScanner
        /// </summary>

        /// <param name="assemblyLocator">The assembly locator.  This parameter is optional.  </param>
        /// <param name="assemblyTypeService">The assembly type service.  This parameter is optional</param>
        /// <returns></returns>
        public static IDependencyScanner Create(
                                                                 Func<IAssemblyLocator> assemblyLocator = null,
                                                                 Func<IAssemblyTypeService> assemblyTypeService = null)
        {
            assemblyLocator = assemblyLocator ?? (() => AssemblyLocatorFactory.Resolve());
            assemblyTypeService = assemblyTypeService ?? (() => AssemblyTypeFactory.Resolve());

            var defaultScanner = new DependencyScanner(assemblyLocator(),
                                                                            assemblyTypeService());
            return defaultScanner;
        }
    }
}