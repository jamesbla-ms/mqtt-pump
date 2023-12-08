
namespace mqtt_pump
{
    public class Message(DateTime timeStamp)
    {
        public DateTimeOffset Timestamp { get; set; } = timeStamp;
        public List<MessageValue> Values { get; set; } = [];
        public Dictionary<string, string> Metadata { get; set; } = [];
        //public List<KeyValuePair<string, string>> Metadata { get; set; } = [];
    }
}
