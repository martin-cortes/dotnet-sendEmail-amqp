using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependecyInjection
    {
        public static IServiceCollection Application(this IServiceCollection services)
        {
            return services;
        }
    }
}