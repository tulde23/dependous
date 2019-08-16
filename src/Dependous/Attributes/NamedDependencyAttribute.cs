namespace Dependous
{
    using System;

    /// <summary>
    /// Provides the ability to arbitrarily name a dependency.  This is useful if you have more than 1 implementation of an interface and you
    /// need a means of distinguishing between them.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NamedDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedDependencyAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public NamedDependencyAttribute(object name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public object Name { get; set; }
    }
}