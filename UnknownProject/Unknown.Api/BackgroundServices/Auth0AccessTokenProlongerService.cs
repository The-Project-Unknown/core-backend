namespace Api.Providers;

public class Auth0AccessTokenProlongerService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Auth0AccessTokenProlongerService> _logger;

    public Auth0AccessTokenProlongerService
    (
        IServiceProvider serviceProvider,
        ILogger<Auth0AccessTokenProlongerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        //var autoEvent = new AutoResetEvent(false);
        AskForManagementAccessToken(null);

        var stateTimer = new Timer(AskForManagementAccessToken,null, 10, 1000 * 60 * 60 * 24);
        
        _logger.LogInformation($"nameof(Auth0AccessTokenProlongerService) service is running");
        
        return Task.CompletedTask;
    }

    private async void AskForManagementAccessToken(object? state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<Auth0ManagementProvider>();

            var token = await scopedProcessingService.GetAccessTokenFromAuth0();
            
            var timeToExpiration = token.ValidTo - DateTime.UtcNow;

            _logger.LogInformation($"Token issued in {token.IssuedAt} by {token.Issuer}");
            _logger.LogInformation($"Token will expire in {timeToExpiration}");
        }
    }


    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}