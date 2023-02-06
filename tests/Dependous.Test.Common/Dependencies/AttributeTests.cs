using Dependous.Attributes;

namespace Dependous.Test.Common.Dependencies
{
    public interface IAttributeInterface
    {
    }

    [Dependency(typeof(IAttributeInterface), ServiceLifetime.Singleton, false)]
    public class AttributeImplementation : IAttributeInterface
    {
    }
}