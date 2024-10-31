using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Infrastructure.Extensions.ProviderService.Parameters
{
    public class BusinessParameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public DateTime ModificationDate { get; set; }

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public object Id { get; set; }
    }
}