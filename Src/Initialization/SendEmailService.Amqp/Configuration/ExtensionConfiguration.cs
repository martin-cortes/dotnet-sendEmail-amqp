using Infrastructure.Extensions.ProviderService;

namespace SendEmailService.Amqp.Configuration
{
    public static class ExtensionConfiguration
    {
        public static IConfigurationBuilder AddMongoProvider(this IConfigurationBuilder configurationBuilder,
                                                                  string connectionString,
                                                                  string databaseName,
                                                                  string collectionName,
                                                                  bool reloadOnChange,
                                                                  string configurationName)
        {
            if (string.IsNullOrEmpty(connectionString) ||
                string.IsNullOrEmpty(collectionName) ||
                string.IsNullOrEmpty(databaseName) ||
                string.IsNullOrEmpty(configurationName))
                throw new ArgumentNullException(nameof(connectionString));

            return configurationBuilder.Add(new MongoConfigurationSource(connectionString,
                                                                         databaseName,
                                                                         collectionName,
                                                                         configurationName,
                                                                         reloadOnChange)
            );
        }
    }
}
