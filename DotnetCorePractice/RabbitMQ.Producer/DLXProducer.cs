using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class DLXProducer
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-mainexchange2", ExchangeType.Direct);

            var message = "This message is from demo dlx";
            var body = Encoding.UTF8.GetBytes(message);
            var routeKey = "test";
            channel.BasicPublish("demo-mainexchange2", routeKey, null, body);
            Console.WriteLine($"Send Message : {message}");
        }
    }
}

