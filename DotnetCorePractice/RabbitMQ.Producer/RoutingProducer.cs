using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
	public class RoutingProducer
	{
        public static void Publish(IModel channel, string? routingKey = default)
        {
            channel.ExchangeDeclare("demo-routingexchange", ExchangeType.Direct);
            var message = new { Name = "Producer", Message = $"Hello! routing to {routingKey}" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish("demo-routingexchange", "both", null, body);
            Console.WriteLine($"Send Message : {message}");
        }
    }
}

