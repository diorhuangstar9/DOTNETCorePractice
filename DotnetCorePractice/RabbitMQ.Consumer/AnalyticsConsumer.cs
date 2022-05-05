using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
	public class AnalyticsConsumer
	{
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-topicexchange", ExchangeType.Topic);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "demo-topicexchange", "*.europe.*");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray(); 
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Analytics - Received message: {message}");
            };

            channel.BasicConsume(queueName, true, consumer);
            Console.WriteLine("Analytics Consuming");
        }

    }
}

