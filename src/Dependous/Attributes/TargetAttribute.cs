namespace Dependous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// By default, the assembly scanning will create an instance of DependencyMetadata for every interface implemented by your dependency.  Often times this behavior is desirable; however, if you need
    /// to target a specific subset of interfaces, use this attribute.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TargetAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAttribute"/> class.
        /// </summary>
        /// <param name="interfaceTypes">The interface types.</param>
        /// <exception cref="ArgumentException">
        /// interfaceTypes - You specify at least one implemented interface
        /// or
        /// interfaceTypes - You must supply interface types only.
        /// </exception>
        public TargetAttribute(params Type[] interfaceTypes)
        {
            if (interfaceTypes == null || !interfaceTypes.Any())
            {
                throw new ArgumentException("You specify at least one implemented interface");
            }

            if (!interfaceTypes.All(x => x.GetTypeInfo().IsInterface))
            {
                throw new ArgumentException("You must supply interface types only.");
            }

            Interfaces = interfaceTypes.ToList();
        }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        /// <value>
        /// The interfaces.
        /// </value>
        public IReadOnlyList<Type> Interfaces { get; private set; }
    }
}