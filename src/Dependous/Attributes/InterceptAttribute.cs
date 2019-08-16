using System;

namespace Dependous.Attributes
{
    /// <summary>
    /// Allows a class to be intercepted.  Your methods and properties must be marked virtual for this to work.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class InterceptAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetAttribute"/> class.
        /// </summary>
        /// <param name="interceptors">The interface types.</param>
        /// <exception cref="ArgumentException">
        /// interfaceTypes - You specify at least one implemented interface
        /// or
        /// interfaceTypes - You must supply interface types only.
        /// </exception>
        public InterceptAttribute(Type interceptor)
        {
            if (interceptor == null)
            {
                throw new ArgumentException("You specify  one interceptor");
            }

            Interceptor = interceptor;
        }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        /// <value>
        /// The interfaces.
        /// </value>
        public Type Interceptor { get; }
    }
}