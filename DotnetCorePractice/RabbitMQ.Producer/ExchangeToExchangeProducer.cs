using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
	public class ExchangeToExchangeProducer
	{
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-firstexchange", ExchangeType.Direct);
            channel.ExchangeDeclare("demo-secondexchange", ExchangeType.Fanout);
            channel.ExchangeBind("demo-secondexchange", "demo-firstexchange", "");

            var message1 = "message to exchange";
            var body1 = Encoding.UTF8.GetBytes(message1);
            channel.BasicPublish("demo-firstexchange", "", null, body1);
            Console.WriteLine($"Send Message : {message1}");

        }
    }
}

