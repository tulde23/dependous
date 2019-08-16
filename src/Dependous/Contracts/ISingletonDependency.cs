namespace Dependous
{
    /// <summary>
    /// Designates an implementor as a discoverable dependency and asserts it's lifetime management should be singleton.
    /// Singleton lifetime services are created the first time they are requested (or when ConfigureServices is run if you specify an instance there) and then every subsequent request will use the same instance.
    /// </summary>
    public interface ISingletonDependency
    {
    }
}