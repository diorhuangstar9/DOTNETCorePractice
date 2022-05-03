using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class PubSubProducer
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-pubsub", ExchangeType.Fanout);
            for (var count = 0; ; count++)
            {
                var message = new { Name = "Producer", Message = $"Hello! Count:{count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("demo-pubsub", "", null, body);
                Thread.Sleep(1000);
            }
        }

    }
}

