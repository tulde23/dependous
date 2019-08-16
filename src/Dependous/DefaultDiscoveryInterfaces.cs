using System;
using System.Collections.Generic;
using Dependous.Contracts;

namespace Dependous
{
    internal static class DefaultDiscoveryInterfaces
    {
        private static readonly List<Type> discoveryInterfaces = new List<Type>()
                {
                  typeof(ISingletonDependency),
                 typeof(ITransientDependency),
                 typeof(IScopedDependency),
                 typeof(ISelf)
                };

        public static IReadOnlyCollection<Type> Interfaces
        {
            get
            {
                return discoveryInterfaces;
            }
        }
    }
}