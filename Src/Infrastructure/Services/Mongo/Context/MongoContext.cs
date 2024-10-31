using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services.Mongo.Context
{
    public class MongoContext
    {
        private readonly string _collectionName;
        private readonly IMongoDatabase _database;

        public MongoContext(string connectionString,
                            string databaseName,
                            string collectionName)
        {
            MongoClient client = new(connectionString);
            _collectionName = collectionName;
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<BsonDocument> GetCollection() =>
            _database.GetCollection<BsonDocument>(_collectionName);
    }
}