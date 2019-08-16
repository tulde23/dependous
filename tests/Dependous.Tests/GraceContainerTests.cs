using System;
using System.Linq;
using Dependous.Models;
using Dependous.Test;
using Grace.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Dependous.Tests
{
    public class GraceContainerTests
    {
        private readonly DependencyInjectionContainer container;
        private readonly ITestOutputHelper output;

        public GraceContainerTests(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyScanning();
            container = serviceCollection.BuildGraceContainer((f) => f.StartsWith("Dependous"),
                logger: (item) => { output.WriteLine($"{item}"); Console.WriteLine(item); }, configurationBuilder: (cb) =>
                {
                    cb.PersistScanResults = true;
                });
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Item From Grace Container")]
        public void TestScanProducesSomeResults()
        {
            var service = container.Locate<ITestF>();
            Assert.NotNull(service);
            Assert.True(service.ListOfA.Any());
            var s = container.Locate<IMyService>();
            s.MyTest();
        }
        [Fact(DisplayName = "Integration Test - Can Resolve Item From Grace Container With Delegate")]
        public void WithDelegate()
        {
            var service = container.Locate<Func<ITestF>>();
            Assert.NotNull(service);
            var instnace = service();
            Assert.True(instnace.ListOfA.Any());
            var s = container.Locate<IMyService>();
            s.MyTest();
        }
        [Fact(DisplayName = "Integration Test - Can Resolve Item From Grace Container With Generics")]
        public void WithGeneric()
        {
            var service = container.Locate<IOpenGeneric<string>>();
            Assert.NotNull(service);

        }
        [Fact(DisplayName = "Integration Test - Can Resolve Scan Results From Grace")]
        public void GetScanResults()
        {
            var results = container.Locate<DependencyScanResult>();
            Assert.NotNull(results);
        }
    }
}