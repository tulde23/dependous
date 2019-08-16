using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dependous.Models;

namespace Dependous.Services
{
    internal class ProbingAssemblyLocator : IAssemblyLocator
    {
        /// <summary>
        /// The extensions
        /// </summary>
        private readonly string[] extensions = new string[2] { ".dll", ".exe" };

        public AssemblyLocatorResult Locate(Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> assemblyNameFilter,
            IDependousConfiguration dependousConfiguration)
        {
            assemblyNameFilter = assemblyNameFilter ?? new Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory>((pf) => pf.Self());
            var defaultFilters = assemblyNameFilter(new AssemblySearchPatternFactory());
            AssemblySearchPatternFactory factory = defaultFilters;
            if (dependousConfiguration.ProbingPaths != null && dependousConfiguration.ProbingPaths.Any())
            {
                List<FileInfo> results = new List<FileInfo>();
                foreach (var path in dependousConfiguration.ProbingPaths)
                {
                    if (path.Pattern != null)
                    {
                        factory = path.Pattern(new AssemblySearchPatternFactory());
                    }
                    else
                    {
                        factory = defaultFilters;
                    }
                    var assemblies = new DirectoryInfo(path.Path)
                                                .EnumerateFiles()
                                                .Where(name => factory.Patterns.Any(x => x.Match(name.Name)) &&
                                                         this.extensions.Contains(name.Extension, StringComparer.OrdinalIgnoreCase));
                    results.AddRange(assemblies);
                }
                return new AssemblyLocatorResult(results.Select(a =>
                {
                    var an = AssemblyName.GetAssemblyName(a.FullName);
                    an.CodeBase = a.FullName;
                    return an;
                }));
            }
            return new AssemblyLocatorResult(Enumerable.Empty<AssemblyName>());
        }
    }
}