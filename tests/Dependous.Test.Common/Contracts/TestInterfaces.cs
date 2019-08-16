using System.Collections.Generic;

namespace Dependous.Test
{
    public interface ITestA { }

    public interface ITestB { }

    public interface ITestC { }

    public interface ITestD { }

    public interface ITestE { }

    public interface ITestF
    {
        IEnumerable<ITestA> ListOfA { get; }

        void DoIt();
    }

    public interface ITestG
    {
        IEnumerable<IProperty> ListOfProperty { get; }

        void DoIt();
    }

    public interface IProperty
    {
        string Name { get; }
    }

    public interface IDecorateInterface
    {
        void Test();
    }

    public interface IGenericA<out T> { }


    public interface IOpenGeneric<out T>
    {
        void Test();
    }

    public interface INonDiscovery { }

    public interface IProbeMe { }
}