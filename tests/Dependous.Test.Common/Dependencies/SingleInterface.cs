using Dependous;

namespace Dependous.Test
{
    public class SingleInterface : ITestA, ITransientDependency
    {
    }
    public class SingleInterfaceNoDiscovery : INonDiscovery, ITestA
    {

    }


    public class SingletonInstance : ISelfSingleton
    {

    }
}