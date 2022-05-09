using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
	public class ConsistentHashingExchangeConsumer
	{
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-hashexchange", "x-consistent-hash");
            channel.QueueDeclare("letterbox1");
            channel.QueueDeclare("letterbox2");
            channel.QueueBind("letterbox1", "demo-hashexchange", "2");
            channel.QueueBind("letterbox2", "demo-hashexchange", "1");
            var consumer1 = new EventingBasicConsumer(channel);
            consumer1.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"consumer1: {message}");
            };
            channel.BasicConsume("letterbox1", true, consumer1);

            var consumer2 = new EventingBasicConsumer(channel);
            consumer2.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"consumer2: {message}");
            };
            channel.BasicConsume("letterbox2", true, consumer2);

        }
    }
}

