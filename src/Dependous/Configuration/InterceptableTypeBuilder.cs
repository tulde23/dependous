using System;
using System.Collections.Generic;

namespace Dependous.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public class InterceptableTypeBuilder
    {
        private IList<InterceptableType> types = new List<InterceptableType>();

        internal InterceptableTypeBuilder()
        {
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeToIntercept">The type to intercept.</param>
        /// <returns></returns>
        public InterceptableTypeBuilder RegisterType<T>(Type typeToIntercept)
        {
            this.types.Add(new InterceptableType
            {
                TypeToIntercept = typeToIntercept,
                InterceptionType = typeof(T)
            });
            return this;
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="typeToIntercept">The type to intercept.</param>
        /// <param name="typeOfInterception">The type of interception.</param>
        /// <returns></returns>
        public InterceptableTypeBuilder RegisterType(Type typeToIntercept, Type typeOfInterception)
        {
            this.types.Add(new InterceptableType
            {
                TypeToIntercept = typeToIntercept,
                InterceptionType = typeOfInterception
            });
            return this;
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <returns>an IEnumerable of AdditionalDiscoveryType</returns>
        internal IEnumerable<InterceptableType> GetTypes()
        {
            return this.types;
        }
    }
}