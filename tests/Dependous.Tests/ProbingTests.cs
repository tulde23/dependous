using System.Linq;
using Xunit;

namespace Dependous.Tests
{
    public class ProbingTests
    {
        [Fact(DisplayName = "Probing Test")]
        public void ProbeIt()
        {
            var instance = DependencyScannerFactory.Create();
            var result = instance.Scan((b) => 
                b.StartsWith("Junk"), 
                    (c) => c.AddProbingPaths(pb => 
                                pb.AddProbingPath("../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.1", (p)=>p.StartsWith("Dependous.Probing"))));
            Assert.True(result.Metadata.Any());
        }
    }
}