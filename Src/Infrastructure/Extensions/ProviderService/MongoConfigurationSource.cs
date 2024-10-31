using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions.ProviderService
{
    public class MongoConfigurationSource(string connectionString,
                                          string database,
                                          string collectionName,
                                          string configurationName,
                                          bool reloadOnChange) : IConfigurationSource
    {
        private readonly string _database = database;
        private readonly bool _reloadOnChange = reloadOnChange;
        private readonly string _collectionName = collectionName;
        private readonly string _connectionString = connectionString;
        private readonly string _configurationName = configurationName;

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new MongoConfigurationProvider(_connectionString,
                                           _database,
                                           _collectionName,
                                           _configurationName,
                                           _reloadOnChange
            );
    }
}
