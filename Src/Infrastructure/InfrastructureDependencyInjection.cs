using Infrastructure.Services.Mongo.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection Infrastructure(this IServiceCollection service)
        {
            return service;
        }

        public static IServiceCollection RegisterMongo(this IServiceCollection services,
                                                       string connectionString,
                                                       string databaseName,
                                                       string collectionName) =>
            services.AddSingleton(cfg => new MongoContext(connectionString, databaseName, collectionName));
    }
}