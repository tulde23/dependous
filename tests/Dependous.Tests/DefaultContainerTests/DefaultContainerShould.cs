using System;
using Dependous.AutoFacTests;
using Dependous.DefaultContainer;
using Dependous.Test;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Dependous.Tests.DefaultContainerTests
{
    public class DefaultContainerShould
    {
        private readonly ITestOutputHelper output;
        private readonly IServiceProvider serviceProvider;

        public DefaultContainerShould(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<MyObject>();
            serviceCollection.AddSingleton(new TestObject());
            serviceCollection.AddDependencyScanning(b => b.AddSingleton<MyObject>().AddSingleton(new TestObject()));
            serviceCollection = serviceCollection.BuildContainer(AssemblyPaths.From("Dependous"),
                logger: (item) => { output.WriteLine($"{item}"); Console.WriteLine(item); }, configurationBuilder: (cb) =>
                {
                    cb.PersistScanResults = true;
                    //  cb.InterceptAll<DynamicInterceptor>();
                });
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Item From Default Container")]
        public void TestScanProducesSomeResults()
        {
            var service = serviceProvider.GetServices<ITestF>();
            Assert.NotNull(service);


            var s = serviceProvider.GetService<SingletonInstance>();
            s.Should().NotBeNull();
        }
    }
}