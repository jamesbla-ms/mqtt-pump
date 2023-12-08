using MQTTnet.Client;
using MQTTnet;

namespace mqtt_pump
{
    internal class MessageQueueClient(string TcpUri, int Port)
    {
        private readonly string TcpUri = TcpUri;
        private readonly int Port = Port;

        public async Task Publish(string TopicName, string Message)
        {
            var mqttFactory = new MqttFactory();

            using var mqttClient = mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(TcpUri, Port)
                .Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(TopicName)
                .WithPayload(Message)
                .Build();

            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            await mqttClient.DisconnectAsync();

            Console.WriteLine("MQTT application message is published.");
        }
    }
}
