using System;

namespace Dependous.Configuration
{
    /// <summary>
    /// Describes a type that can be intercepted/proxied
    /// </summary>
    public class InterceptableType
    {
        /// <summary>
        /// Gets or sets the type to intercept.
        /// </summary>
        /// <value>
        /// The type to intercept.
        /// </value>
        public Type TypeToIntercept { get; set; }

        /// <summary>
        /// Gets or sets the type of the interception.
        /// </summary>
        /// <value>
        /// The type of the interception.
        /// </value>
        public Type InterceptionType { get; set; }
    }
}