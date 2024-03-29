﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dependous.Models
{
    /// <summary>
    /// Models the result of a dependency scan
    /// </summary>
    public class DependencyScanResult : Operation
    {
        /// <summary>
        /// The syncroot
        /// </summary>
        private static object syncroot = new object();

        /// <summary>
        /// The assemblies
        /// </summary>
        private readonly List<Assembly> _assemblies;

        /// <summary>
        /// The metadata
        /// </summary>
        private readonly List<DependencyMetadata> _metadata;

        /// <summary>
        /// The exceptions
        /// </summary>
        private readonly List<Exception> _exceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyScanResult"/> class.
        /// </summary>
        public DependencyScanResult()
        {
            _metadata = new List<DependencyMetadata>();
            _exceptions = new List<Exception>();
            _assemblies = new List<Assembly>();
        }

        /// <summary>
        /// Gets the scanned assemblies.
        /// </summary>
        /// <value>
        /// The scanned assemblies.
        /// </value>
        public IReadOnlyCollection<Assembly> ScannedAssemblies
        {
            get
            {
                return _assemblies;
            }
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IDependousConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<DependencyMetadata> Metadata
        {
            get
            {
                return _metadata;
            }
        }

        /// <summary>
        /// If any exceptions occurred durning the scan, they will be reported here.
        /// </summary>
        /// <value>
        /// The type load exceptions.
        /// </value>
        public IReadOnlyCollection<Exception> ScanExceptions
        {
            get
            {
                return _exceptions;
            }
        }

        /// <summary>
        /// Adds the metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public void AddMetadata(DependencyMetadata metadata)
        {
            lock (syncroot)
            {
                _metadata.Add(metadata);
            }
        }

        /// <summary>
        /// Adds the exceptions.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        public void AddExceptions(IEnumerable<Exception> exceptions)
        {
            lock (syncroot)
            {
                if (exceptions != null && exceptions.Any())
                {
                    _exceptions.AddRange(exceptions);
                }
            }
        }

        /// <summary>
        /// Adds the exceptions.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        public void AddAssembly(Assembly assembly)
        {
            lock (syncroot)
            {
                _assemblies.Add(assembly);
            }
        }

        public override string ToString()
        {
            //list all scanned assemblies.

            var sb = new StringBuilder();

            var linePattern = "Dependency: {0,-75}|{1,-17}";
            var dependencies = new StringBuilder();
            dependencies.AppendLine();
            foreach (var metadata in Metadata.OrderBy(x => x.DependencyType.Name))
            {
                var name = metadata.DependencyType.FullName;
                var named = metadata.NamedDependency?.ToString();
                var assembly = metadata.DependencyType.Assembly.GetName().Name;
                if (named != null)
                {
                    dependencies.AppendLine($"Named Dependency: {named}");
                }
                dependencies.AppendLine(string.Format(linePattern, $"{name}", $"Lifetime:{metadata.ServiceLifetime}"));

                dependencies.AppendLine("\tImplements: " + string.Join("|", metadata.ImplementedInterfaces.Select(x => $"{x.FullName}")));
            }
            sb.AppendLine(dependencies.ToString());
            return sb.ToString();
        }
    }
}