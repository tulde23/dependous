using Dependous;
using Dependous.Models;

namespace ExamplesCommon
{
    public interface IMyService : ISingletonDependency
    {
        public Task<string> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class MyServiceImplementation : IMyService
    {
        public async Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(500);
            return DateTime.UtcNow.ToString();
        }
    }

    public class MySingleton : ISelfSingleton
    {
        public MySingleton(DependencyScanResult scanResults)
        {
            ScanResults = scanResults;
        }

        public DependencyScanResult ScanResults { get; }
    }
}