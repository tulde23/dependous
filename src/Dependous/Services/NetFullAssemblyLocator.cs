using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dependous.Models;

namespace Dependous
{
    /// <summary>
    /// .NET Full  implementation of IAssemblyLocator.  This implementation uses the dependency context to get a list of all
    /// references assemblies.
    /// </summary>
    /// <seealso cref="IAssemblyLocator" />
    internal class NetFullAssemblyLocator : IAssemblyLocator
    {
        /// <summary>
        /// The extensions
        /// </summary>
        private readonly string[] extensions = new string[2] { ".dll", ".exe" };

        /// <summary>
        /// Locates assemblies using the supplied search filter.
        /// </summary>
        /// <param name="assemblyNameFilter">The assembly name filter.</param>
        /// <returns></returns>
        public virtual AssemblyLocatorResult Locate(Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter, IDependousConfiguration dependousConfiguration)
        {
            assemblyNameFilter = assemblyNameFilter ?? new Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory>((pf) => pf.Self());
            var filters = assemblyNameFilter(new AssemblySearchPatternFactory());
            var binDir = new DirectoryInfo(this.GetBinPath());
            var files = binDir.EnumerateFiles()
                .Where(name => filters.Patterns.Any(x => x.Match(name.Name)) &&
                                         this.extensions.Contains(name.Extension, StringComparer.OrdinalIgnoreCase));
            //trap any exceptions that may occur.
            var names = new List<AssemblyName>();

            foreach (var file in files)
            {
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(file.FullName);
                    names.Add(assemblyName);
                }
                catch (System.BadImageFormatException)
                {
                }
            }
            return new AssemblyLocatorResult(names);
        }

        /// <summary>
        /// Gets the bin path.
        /// </summary>
        /// <returns></returns>
        protected string GetBinPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
        }
    }
}