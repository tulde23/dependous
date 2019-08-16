using System.Collections.Generic;
using Dependous;

namespace Dependous.Test
{
    [Target(typeof(ITestD), typeof(ITestE))]
    public class MultipleInterfacesWithTargeting : ITestD, ITestE, ITestF, ITransientDependency
    {
        public virtual IEnumerable<ITestA> ListOfA { get; }

        public virtual void DoIt()
        {
            throw new System.NotImplementedException();
        }
    }
}