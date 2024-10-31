using Application;
using Application.Common.Health;
using Infrastructure;

namespace SendEmailService.Amqp.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection RegisterHealthCheck(this IServiceCollection services) =>
            services.AddHostedService<HealthCheckHostedService>();

        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            services
                .Application()
                .Infrastructure();

            return services;
        }
    }
}