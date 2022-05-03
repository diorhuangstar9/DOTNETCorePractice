using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
	public class PubsubConsumer
	{
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-pubsub", ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "demo-pubsub", "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message);
            };

            channel.BasicConsume(queueName, true, consumer);
        }
    }
}

