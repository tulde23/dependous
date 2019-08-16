using System.Collections.Generic;

namespace Dependous
{
    /// <summary>
    /// Scannable assembly paths for Dependous.
    /// </summary>
    public class AssemblyPaths
    {
        /// <summary>
        /// Gets the paths.
        /// </summary>
        /// <value>The paths.</value>
        public List<string> Paths { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyPaths"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public AssemblyPaths(string path)
        {
            Paths = new List<string>()
            {
                path
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyPaths"/> class.
        /// </summary>
        /// <param name="paths">The paths.</param>
        public AssemblyPaths(string[] paths)
        {
            Paths = new List<string>(paths);
        }

        /// <summary>
        /// Froms the specified paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        public static AssemblyPaths From(params string[] paths)
        {
            return new AssemblyPaths(paths);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="AssemblyPaths"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator AssemblyPaths(string path)
        {
            return new AssemblyPaths(path);
        }

        /// <summary>
        /// Performs an implicit conversion from
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator AssemblyPaths(string[] paths)
        {
            return new AssemblyPaths(paths);
        }
    }
}