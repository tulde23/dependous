namespace Dependous
{
    /// <summary>
    /// Designates an implementor as a discoverable dependency and asserts it's lifetime management should be transient.
    /// Transient lifetime services are created each time they are requested. This lifetime works best for lightweight, stateless services.
    /// </summary>
    public interface ITransientDependency
    {
    }
}