#if NETSTANDARD2_0|| NETSTANDARD2_1

using System;
using System.Linq;
using Dependous.Models;
using Microsoft.Extensions.DependencyModel;

namespace Dependous
{
    /// <summary>
    /// .NET core implementation of IAssemblyLocator.  This implementation uses the dependency context to get a list of all
    /// references assemblies.
    /// </summary>
    /// <seealso cref="IAssemblyLocator" />
    /// <seealso cref="Dependous.IAssemblyLocator" />
    internal class NetCoreAssemblyLocator : IAssemblyLocator
    {
        public AssemblyLocatorResult Locate(Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter, IDependousConfiguration dependousConfiguration)
        {
            assemblyNameFilter = assemblyNameFilter ?? new Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory>((pf) => pf.Self().StartsWith("BSB"));
            var filters = assemblyNameFilter(new AssemblySearchPatternFactory());
            var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(name => filters.Patterns.Any(x => x.Match(name)));
            //
            return new AssemblyLocatorResult(assemblyNames);
        }
    }
}

#endif