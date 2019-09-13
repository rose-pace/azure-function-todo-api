using Newtonsoft.Json;

namespace Avantia.Functions
{
    public class ToDoItem : Microsoft.Azure.Documents.Document
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "isComplete")]
        public bool IsComplete { get; set; }
    }
}