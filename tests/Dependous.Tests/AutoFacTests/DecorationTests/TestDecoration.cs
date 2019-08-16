using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Dependous.Autofac;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Dependous.Tests.AutoFacTests.DecorationTests
{
    public class TestDecoration
    {
        private readonly IContainer container;
        private readonly ITestOutputHelper output;

        public TestDecoration(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyScanning();
            container = serviceCollection.BuildAutoFacContainer((f) => f.StartsWith("Dependous"),
                logger: (item) => { output.WriteLine($"{item}"); Console.WriteLine(item); });
        }

        [Fact(DisplayName = "Integration Test - Tests Automatic Chained Decoration")]
        public void TestsChainedDecoration()
        {
            var service = container.Resolve<IDecoratableService>();
            Assert.NotNull(service);
            var result = service.Method();
            result.Should().Be("D2.BeforeD1.BeforeHelloD1.AfterD2.After");
        }
    }

    public interface IDecoratableService : ISingletonDependency
    {
        string Method();
    }

    public class TrueService : IDecoratableService
    {
        public string Method()
        {
            return $"Hello";
        }
    }

    public class D1 : IDecoratableService, IDecorator<IDecoratableService>
    {
        public D1(IDecoratableService decoratableService)
        {
            DecoratableService = decoratableService;
        }

        public IDecoratableService DecoratableService { get; }

        public string Method()
        {
            var sb = new StringBuilder();
            sb.Append("D1.Before");
            sb.Append(DecoratableService.Method());
            sb.Append("D1.After");
            return sb.ToString();
        }
    }

    public class D2 : IDecoratableService, IDecorator<IDecoratableService>
    {
        public D2(IDecoratableService decoratableService)
        {
            DecoratableService = decoratableService;
        }

        public IDecoratableService DecoratableService { get; }

        public string Method()
        {
            var sb = new StringBuilder();
            sb.Append("D2.Before");
            sb.Append(DecoratableService.Method());
            sb.Append("D2.After");
            return sb.ToString();
        }
    }
}