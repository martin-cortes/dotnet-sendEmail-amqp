namespace Application.Common.Utilities
{
    public class BusinessSettings
    {
        public string LogLevelSink { get; set; }
        public int DocumentExpiration { get; set; }
        public string SinkCollectionName { get; set; }
        public string HealthChecksEndPoint { get; set; }
    }
}