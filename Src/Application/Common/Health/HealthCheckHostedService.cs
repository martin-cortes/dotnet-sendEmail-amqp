using Application.Common.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Common.Health
{
    public class HealthCheckHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HealthCheckHostedService> _logger;

        public HealthCheckHostedService(IServiceProvider serviceProvider,
                                        ILogger<HealthCheckHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            HealthCheckService healthCheckService = scope.ServiceProvider
                .GetRequiredService<HealthCheckService>();

            HealthReport report = await healthCheckService
                .CheckHealthAsync(cancellationToken);

            _logger.LogInformation("Health Check Report:");

            _logger.LogInformation("Overall Status: {Status}", report.Status);

            HealthCheckInformation information;

            JsonSerializerOptions options = new() { WriteIndented = true };

            foreach (KeyValuePair<string, HealthReportEntry> entry in report.Entries)
            {
                string description = entry.Value.Status != HealthStatus.Unhealthy ?
                    $"{entry.Key} is avaliable" :
                    $"{entry.Key} not is avaliable";

                information = new()
                {
                    Service = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Description = description,
                };

                _logger.LogInformation("=============== HEALTHCHECK {ServiceStart} ============= \n" +
                                       "{Information} \n" +
                                       "============= END HEALTHCHECK {ServiceEnd} ===========",
                                       entry.Key,
                                       JsonSerializer.Serialize(information, options),
                                       entry.Key);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
