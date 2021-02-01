using System.Linq;
using System.Reflection;

namespace SportsConnect.Dependous.Autofac
{
    /// <summary>
    /// Custom autofac setup methods.
    /// </summary>
    public static class AutoFacSetup
    {
        /// <summary>
        /// Sets the automatic fac circular dependency limit.
        /// https://github.com/autofac/Autofac/commit/1bbcc0677d00e2af9328067de8724c5563286e5a
        /// </summary>
        public static void SetAutoFacCircularDependencyLimit()
        {
            try
            {
                //https://github.com/autofac/Autofac/commit/1bbcc0677d00e2af9328067de8724c5563286e5a
                var assembly = Assembly.Load("Autofac");
                var types = assembly.GetTypes();
                var type = types.Single(x => x.Name == "CircularDependencyDetector");
                var field = type?.GetField("MaxResolveDepth", BindingFlags.Static | BindingFlags.NonPublic);
                field?.SetValue(null, int.MaxValue);
            }
            catch
            {
                //swallow it whole.
            }
        }
    }
}