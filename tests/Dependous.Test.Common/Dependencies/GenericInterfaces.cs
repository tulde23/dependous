using System;

namespace Dependous.Test
{
    public class GenericInterfaces : IGenericA<string>, IProperty, ITransientDependency
    {
        public string Name => throw new System.NotImplementedException();
    }

    public class Generic2<T> : IOpenGeneric<T>, ITransientDependency
    {
        public void Test()
        {
            Console.WriteLine("I passed generic type " + typeof(T).Name);
        }
    }
}