namespace mqtt_pump
{
    public class MessageValue(string tag)
    {
        public string Tag { get; set; } = tag;
        public string? TextValue { get; set; }
        public decimal NumericValue { get; set; }
    }
}
