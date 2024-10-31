using MongoDB.Driver;
using Serilog;
using Serilog.Events;

namespace SendEmailService.Amqp.Configuration
{
    public static class HostConfiguration
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder,
                                                    string logLevel,
                                                    string databaseName,
                                                    string collectionName,
                                                    string connectionString,
                                                    int documentExpiration) =>
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .WriteTo.Console()
            .WriteTo.MongoDBBson(cfg =>
            {
                cfg.SetExpireTTL(TimeSpan.FromDays(documentExpiration));
                cfg.SetMongoDatabase(MongoSinkConfiguration(connectionString, databaseName));
                cfg.SetCollectionName(collectionName);
            }, Enum.Parse<LogEventLevel>(logLevel))
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", context.HostingEnvironment.ApplicationName)
            .Enrich.FromLogContext();
        });

        #region Private Method

        private static IMongoDatabase MongoSinkConfiguration(string connectionString, string databaseName)
        {
            MongoClient mongoClient = new(connectionString);
            return mongoClient.GetDatabase(databaseName);
        }

        #endregion Private Method
    }
}
