
namespace mqtt_pump
{
    public class MessageMetadata(string Key, string Value)
    {
        public string Key { get; set; } = Key;
        public string Value { get; set; } = Value;
    }
}
