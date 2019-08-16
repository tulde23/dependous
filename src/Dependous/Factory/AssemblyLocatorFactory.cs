using Dependous.Services;

namespace Dependous.Factory
{
    internal static class AssemblyLocatorFactory
    {
        public static IAssemblyLocator Resolve()
        {
            return new AssemblyLocatorDecorator();
        }
    }
}