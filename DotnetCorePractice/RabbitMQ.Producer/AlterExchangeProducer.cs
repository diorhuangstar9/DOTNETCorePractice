using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class AlterExchangeProducer
    {
        public static void Publish(IModel channel)
        {

            channel.ExchangeDeclare("demo-alterexchange", ExchangeType.Fanout);
            channel.ExchangeDeclare("demo-mainexchange", ExchangeType.Direct, arguments: new Dictionary<string, object>
            {
                { "alternate-exchange", "demo-alterexchange"}
            });

            var message = "This message is from demo alter exchange";
            var body = Encoding.UTF8.GetBytes(message);
            var routeKey = "test2";
            channel.BasicPublish("demo-mainexchange", routeKey, null, body);
            Console.WriteLine($"Send Message : {message}");
        }
    }
}

