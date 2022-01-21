using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dependous.Models;
using Dependous.Test;
using Moq;
using Xunit;

namespace Dependous.Tests
{
    public class ScanTests
    {
        private readonly AssemblyName assemblyName = typeof(SingleInterface).GetTypeInfo().Assembly.GetName();
        private readonly Type singleInterface = typeof(SingleInterface);
        private readonly AssemblyLocatorResult locatorResult;

        private readonly Func<AssemblySearchPatternFactory, AssemblySearchPatternFactory> searchFactory = (factory) =>
        {
            return factory.StartsWith("Dependous.Test");
        };

        public ScanTests()
        {
            locatorResult = new AssemblyLocatorResult(new List<AssemblyName>() { assemblyName });
            locatorResult.Duration = TimeSpan.FromMilliseconds(10);
        }

        [Fact(DisplayName = "Unit Test - Scan Produces At Least One Result")]
        public void test_custom_assembly_locator_and_type_service()
        {
            Mock<IAssemblyLocator> mockAssemblyLocator = new Mock<IAssemblyLocator>();
            Mock<IAssemblyTypeService> mockTypeService = new Mock<IAssemblyTypeService>();
            Mock<IDependousConfiguration> mockConfig = new Mock<IDependousConfiguration>();
            mockAssemblyLocator.Setup(x => x.Locate(searchFactory, It.IsAny<IDependousConfiguration>())).Returns(() => locatorResult);
            mockTypeService.Setup(x => x.GetTypes(assemblyName)).Returns(this.GetDefaultTypes());

            var instance = DependencyScannerFactory.Create(() => mockAssemblyLocator.Object, () => mockTypeService.Object);
            var results = instance.Scan(searchFactory);
            Assert.True(results.Metadata.Any());
        }

        [Fact(DisplayName = "Integration Test - Scan Produces At Least One Result")]
        public void TestScanProducesSomeResults()
        {
            var instance = DependencyScannerFactory.Create();
            var scanResult = instance.Scan((f) => f.StartsWith("Dependous.Test.Common"), c=>c.AddProbingPaths(pb=>pb.AddProbingPath("../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.1")));
            Assert.True(scanResult.Metadata.Any());
        }

        private AssemblyTypeResult GetDefaultTypes()
        {
            return new AssemblyTypeResult(new List<Type> { singleInterface }, singleInterface.Assembly);
        }
    }
}