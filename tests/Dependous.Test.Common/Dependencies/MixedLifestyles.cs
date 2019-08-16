using System;

namespace Dependous.Test.Dependencies
{
    public class SingletonProperty : IProperty, ISingletonDependency
    {
        public SingletonProperty()
        {
            this.Name = Guid.NewGuid().ToString();
        }

        public string Name { get; }
    }

    public class TransientProperty : IProperty, ITransientDependency
    {
        public TransientProperty()
        {
            this.Name = Guid.NewGuid().ToString();
        }

        public string Name { get; }
    }
}