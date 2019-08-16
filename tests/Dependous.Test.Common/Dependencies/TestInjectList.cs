using System;
using System.Collections.Generic;
using Dependous.Test;

namespace Dependous.Test.Dependencies
{
    public class TestInjectList : ITestF, ITransientDependency
    {
        public TestInjectList(IEnumerable<ITestA> tests)
        {
            ListOfA = tests;
        }

        public virtual IEnumerable<ITestA> ListOfA { get; }

        public void DoIt()
        {
            foreach (var i in ListOfA)
            {
                Console.WriteLine(i.GetType().Name);
            }
        }
    }
}