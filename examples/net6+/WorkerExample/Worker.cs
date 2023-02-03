using ExamplesCommon;

namespace WorkerExample;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMyService myService;
    private readonly MySingleton mySingleton;

    public Worker(ILogger<Worker> logger, IMyService myService, MySingleton mySingleton)
    {
        _logger = logger;
        this.myService = myService;
        this.mySingleton = mySingleton;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await this.myService.ExecuteAsync(stoppingToken);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _logger.LogInformation($"MyService {message}");
            _logger.LogInformation($"Scan Results: {mySingleton.ScanResults.ToString()}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
