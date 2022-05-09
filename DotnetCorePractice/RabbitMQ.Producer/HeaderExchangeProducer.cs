using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class HeaderExchangeProducer
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-headerexchange", ExchangeType.Headers);

            var message = "This message is from header exchange";
            var body = Encoding.UTF8.GetBytes(message);
            var property = channel.CreateBasicProperties();
            property.Headers = new Dictionary<string, object> {
                { "name", "brian" }
            };
            channel.BasicPublish("demo-headerexchange", "", property, body);
            Console.WriteLine($"Send Message : {message}");

        }
    }
}

