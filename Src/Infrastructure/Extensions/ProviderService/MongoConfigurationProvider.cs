using Infrastructure.Extensions.ProviderService.Parameters;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections;
using System.Text.Json;

namespace Infrastructure.Extensions.ProviderService
{
    public class MongoConfigurationProvider : ConfigurationProvider
    {
        private readonly IMongoCollection<BusinessParameter> _collection;
        private readonly string _configurationName;

        public MongoConfigurationProvider(string connectionString,
                                          string databaseName,
                                          string collectionName,
                                          string configurationName,
                                          bool reloadOnChange)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<BusinessParameter>(collectionName);
            _configurationName = configurationName;
            if (reloadOnChange)
                _ = WatchCollectionStatic();
        }

        public override void Load()
        {
            List<BusinessParameter> configs = _collection.Find(FilterDefinition<BusinessParameter>.Empty).ToList();

            foreach (BusinessParameter config in configs)
            {
                string clave = config.Name;

                object valor = config.Value;

                Type type = valor.GetType();

                if (IsPrimitiveType(type))
                {
                    Set($"{_configurationName}:" + clave, valor.ToString());
                }
                else if (IsEnumerableType(type))
                {
                    ProcessEnumerableValue(clave, valor);
                }
                else
                {
                    string obtenerValor = JsonSerializer.Serialize(valor);

                    Set($"{_configurationName}:" + clave, obtenerValor);
                }
            }
        }

        private async Task WatchCollectionStatic()
        {
            using IChangeStreamCursor<ChangeStreamDocument<BusinessParameter>> cursor = await _collection.WatchAsync();

            await cursor.ForEachAsync((change) =>
            {
                Load();

                OnReload();
            });
        }

        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(decimal) || type == typeof(string);
        }

        private static bool IsEnumerableType(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private void ProcessEnumerableValue(string clave, object valor)
        {
            int num = 0;

            foreach (dynamic array in valor as IEnumerable)
            {
                Type typeArray = array.GetType();

                if (IsPrimitiveType(typeArray))
                {
                    Set($"{_configurationName}:{clave}:{num}", ((object)array).ToString());
                }
                else
                {
                    IDictionary<string, object> dictionary = array;

                    foreach (string key in dictionary.Keys)
                    {
                        Set($"{_configurationName}:{clave}:{num}:{key}", $"{dictionary[key]}");
                    }

                    Set($"{_configurationName}:{clave}:{num}", JsonSerializer.Serialize((object)array));
                }

                num++;
            }
        }
    }
}
