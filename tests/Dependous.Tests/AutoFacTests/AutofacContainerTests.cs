using System;
using System.Linq;
using Autofac;
using Castle.DynamicProxy;
using Dependous.Models;
using Dependous.Test;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Dependous.AutoFacTests
{
    public interface IMyService
    {
        void MyTest();
    }

    // [Intercept(typeof(DynamicInterceptor))]
    public class MyService : IMyService, ISingletonDependency
    {
        public virtual void MyTest()
        {
            Console.WriteLine("Invoked MyTest");
        }
    }

    /// <summary>
    /// An implementation of AutoFac's type interception. This class intercepts property get/set
    /// methods on all derivations of DynamicDomainEntity. This class is registerd at startup using
    /// the config.AddInterceptionTypes method.
    /// </summary>
    /// <seealso cref="Castle.DynamicProxy.IInterceptor"/>
    public class DynamicInterceptor : IInterceptor
    {
        public DynamicInterceptor(IServiceProvider serviceProvider)
        {
        }

        /// <summary>
        /// A dynamic domain entity is really just a fancy dictionary with some syntactic sugar. This
        /// proxy allows us to intercept the get/set methods on the target instance and interact with
        /// the underlying dictionary, essentially tricking the runtime into thinking declared
        /// properties are dynamic. Furthmore, if you opt not to resolve your instances through an
        /// IOC container, the class will continue to function as a regular ole POCO because the
        /// get/sets will still execute they just won't be placed into the dyanmic backing dictionary
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.GetConcreteMethod();
            if (methodInfo.IsSpecialName)
            {
                //if (invocation.Method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase))
                //{
                //    var entity = invocation.Proxy as MyService;
                //    var name = invocation.Method.Name.Split("_".ToCharArray()).Last();
                //    invocation.ReturnValue = entity?.GetBoundMember(name);
                //}
                //else if (invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
                //{
                //    var entity = invocation.Proxy as MyService;
                //    var name = invocation.Method.Name.Split("_".ToCharArray()).Last();
                //    entity?.SetMemberAfter(name, invocation.Arguments.FirstOrDefault());
                //}
            }
            Console.WriteLine("Before Call");
            invocation.Proceed();
            Console.WriteLine("After Call");
        }
    }

    public class AutofacContainerTests
    {
        private readonly IContainer container;
        private readonly ITestOutputHelper output;

        public AutofacContainerTests(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyScanning();
            container = serviceCollection.BuildAutoFacContainer((f) => f.StartsWith("Dependous"),
                logger: (item) => { output.WriteLine($"{item}"); Console.WriteLine(item); }, configurationBuilder: (cb) =>
                 {
                     cb.PersistScanResults = true;
                     cb.InterceptAll<DynamicInterceptor>();
                     cb.AddProbingPaths(pb => pb.AddProbingPath("../../../../../tests/Dependous.Probing/bin/Debug/netstandard2.0", (p) => p.StartsWith("Dependous.Probing")));
                 });
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Item From Autofac Container")]
        public void TestScanProducesSomeResults()
        {
            var service = container.Resolve<ITestF>();
            Assert.NotNull(service);
            Assert.True(service.ListOfA.Any());
            var s = container.Resolve<IMyService>();
            s.MyTest();
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Item From AutoFac Container With Delegate")]
        public void WithDelegate()
        {
            var service = container.Resolve<Func<ITestF>>();
            Assert.NotNull(service);
            var instnace = service();
            Assert.True(instnace.ListOfA.Any());
            var s = container.Resolve<IMyService>();
            s.MyTest();
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Item From AutoFac Container With Generics")]
        public void WithGeneric()
        {
            var service = container.Resolve<IOpenGeneric<string>>();
            Assert.NotNull(service);
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Item From AutoFac Container When Using Probing")]
        public void WithProbing()
        {
            var service = container.Resolve<IProbeMe>();
            Assert.NotNull(service);
        }

        [Fact(DisplayName = "Integration Test - Can Resolve Scan Results")]
        public void GetScanResults()
        {
            var results = container.Resolve<DependencyScanResult>();
            Assert.NotNull(results);
        }
    }
}