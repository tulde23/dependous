using Dependous.Test;

namespace Dependous.Probing
{
    public interface IProbingTest : ISingletonDependency
    {
    }

    internal class ProbingTest : IProbingTest, IProbeMe
    {

    }
}