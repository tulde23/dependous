using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dependous.Models;

namespace Dependous.Services
{
    /// <summary>
    /// Implements a decorator allowing us to chain together multiple IAssemblyLocator implementations
    /// </summary>
    /// <seealso cref="Dependous.IAssemblyLocator" />
    internal class AssemblyLocatorDecorator : IAssemblyLocator
    {
        private readonly List<IAssemblyLocator> _assemblyLocators;

        public AssemblyLocatorDecorator()
        {


             _assemblyLocators = new List<IAssemblyLocator>()
            {
                 new NetFullAssemblyLocator(),
                new ProbingAssemblyLocator()
            };

        }

        public AssemblyLocatorResult Locate(Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter,
            IDependousConfiguration dependousConfiguration)
        {
            var results = _assemblyLocators.Select(x => x.Locate(assemblyNameFilter, dependousConfiguration));
            var alr = new AssemblyLocatorResult(results.SelectMany(x => x.DiscoveredAssemblyNames).Distinct(new AssemblyNameComparer()));
            return alr;
        }

        private class AssemblyNameComparer : IEqualityComparer<AssemblyName>
        {
            public bool Equals(AssemblyName x, AssemblyName y)
            {
                return x.FullName.Equals(y.FullName);
            }

            public int GetHashCode(AssemblyName obj)
            {
                return obj.FullName.GetHashCode();
            }
        }
    }
}