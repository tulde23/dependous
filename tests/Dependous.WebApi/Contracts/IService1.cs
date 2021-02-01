namespace Dependous.WebApi.Contracts
{
    public interface IService1 : ITransientDependency
    {
    }

    public class Hello
    {
    }

    public interface IOpenGenericService<T> where T : new()
    {
    }

    public class ServiceImplementation1 : IService1
    {
    }

    public class Closed : IOpenGenericService<Hello>, ISingletonDependency
    {
    }
}