using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.DynamicProxy;
using Dependous.Attributes;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Dependous.Test.Common.Dependencies
{
   
    [Intercept(typeof(LoggingInterceptor))]
    public  class DoNothingIncerceptableService : IncerceptableService, ITransientDependency
    {
        public virtual  string Invoke()
        {
            return "true method call";
        }
    }

    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.Write("Calling method {0} with parameters {1}... ",
     invocation.Method.Name,
     string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            var test = "before";
            invocation.Proceed();
            var test2 = "after";
            invocation.ReturnValue = $"{test} {invocation.ReturnValue} {test2}";
            Console.WriteLine("Done: result was {0}.", invocation.ReturnValue);
        }
    }
}