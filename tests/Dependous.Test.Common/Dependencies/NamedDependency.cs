using Dependous;

namespace Dependous.Test
{
    [NamedDependency("UnitTest")]
    public class NamedDependency : ITestE, ITransientDependency
    {
    }
}