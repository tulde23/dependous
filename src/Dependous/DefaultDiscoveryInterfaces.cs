using System;
using System.Collections.Generic;

namespace Dependous
{
    internal static class DefaultDiscoveryInterfaces
    {
        private static readonly List<Type> _discoveryInterfaces = new List<Type>()
                {
                  typeof(ISingletonDependency),
                 typeof(ITransientDependency),
                 typeof(IScopedDependency),
                 typeof(ISelfTransient)
                };

        public static IReadOnlyCollection<Type> Interfaces
        {
            get
            {
                return _discoveryInterfaces;
            }
        }
    }
}