using System.CommandLine;
using System.Text.Json;

using mqtt_pump;

var sourceOption = new Option<string>(name: "--source",
                                            getDefaultValue: () => "source.csv",
                                            description: "The source CSV file");
var tcpServerOption = new Option<string>(name: "--server",
                                            getDefaultValue: () => "localhost",
                                            description: "The MQTT server");
var tcpPortOption = new Option<int>(name: "--port",
                                            getDefaultValue: () => 1883,
                                            description: "The port of the MQTT broker");
var publishTopicOption = new Option<string>(name: "--topic",
                                            description: "The topic to use for publishing");

var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The file to read and display on the console.");

var rootCommand = new RootCommand("Send CSV data to the specified MQTT broker")
{
    sourceOption,
    tcpServerOption,
    tcpPortOption,
    publishTopicOption
};

rootCommand.SetHandler(SendMessages, sourceOption, tcpServerOption, tcpPortOption, publishTopicOption);

return await rootCommand.InvokeAsync(args);

static async Task SendMessages(string source, string tcpServer, int tcpPort, string publishTopic)
{
    var messages = File.ReadAllLines(source)
        .Skip(1)
        .Select(x => x.Split(','))
        .Select(x =>
        {
            var msg = new Message(DateTime.Parse(x[0]));
            var value = new MessageValue(x[1]);
            if (decimal.TryParse(x[2], out var numericValue))
                value.NumericValue = numericValue;
            else
                value.TextValue = x[2];
            msg.Values.Add(value);
            //msg.Metadata.Add("someName", "someValue");
            return msg;
        });

    var MessageClient = new MessageQueueClient(tcpServer, tcpPort);

    foreach (var message in messages)
    {
        var json = JsonSerializer.Serialize(message);
        Console.WriteLine(json);
        await MessageClient.Publish(publishTopic, JsonSerializer.Serialize(json));
    }
}
