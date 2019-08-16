namespace Dependous
{
    /// <summary>
    /// Designates an implementor as a discoverable dependency and asserts it's lifetime management should be scoped.
    /// Scoped lifetime services are shared within a single request (or Service Scope)
    /// </summary>
    public interface IScopedDependency
    {
    }
}