using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Producer
{
    public class ConsistentHashingExchangeProducer
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-hashexchange", "x-consistent-hash");

            var message = "This message is from hash exchange";
            var body = Encoding.UTF8.GetBytes(message);
            var routingKey = "has me !";
            channel.BasicPublish("demo-hashexchange", routingKey, null, body);
            Console.WriteLine($"Send Message : {message}");
        }
    }
}

