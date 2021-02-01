namespace Dependous.Autofac
{
    /// <summary>
    /// Marks an implementation as a decorator of another type.
    /// </summary>
    public interface IDecorator
    {
    }

    /// <summary>
    /// Marks an implementation as a decorator of another type.
    /// </summary>
    /// <example>
    /// public interface IService{}
    /// public class MyService : IService, ITransientDependency{
    /// }
    /// public class MyCacheService :  IDecorator[IService], ITransientDependency
    /// </example>
    /// <typeparam name="TDecoratee">The type of the decoratee.</typeparam>
    public interface IDecorator<TDecoratee> : IDecorator
    {
    }
}