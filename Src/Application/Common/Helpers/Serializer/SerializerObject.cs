using System.Text.Json;

namespace Application.Common.Helpers.Serializer
{
    public static class SerializerObject
    {
        public static string ConvertObjectToJsonIndented<TValue>(TValue objectToJson)
        {
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true
            };

            JsonSerializerOptions options = jsonSerializerOptions;

            return JsonSerializer.Serialize(objectToJson, options);
        }
    }
}