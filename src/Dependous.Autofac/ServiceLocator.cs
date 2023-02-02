using System;
using Autofac.Features.Indexed;
using Microsoft.Extensions.DependencyInjection;

namespace Dependous
{
    /// <summary>
    /// Implements the service locator anit-pattern.  Please use this sparingly.
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// Configurations the specified builder.
        /// </summary>
        /// <param name="resolver">The builder.</param>
        public static void Configure(Func<IServiceProvider> resolver)
        {
            _serviceProvider = resolver();
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Resolves instance by name dependency
        /// </summary>
        /// <param name="dependencyName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(string dependencyName)
        {
            var context = _serviceProvider.GetService<IIndex<string, T>>();
            return context[dependencyName];
        }
    }
}