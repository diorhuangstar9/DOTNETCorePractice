using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class MQClient
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare("demo-client-requestqueue", exclusive: false);
            var replyQueueName = channel.QueueDeclare("", exclusive: true).QueueName;
            var property = channel.CreateBasicProperties();
            property.CorrelationId = Guid.NewGuid().ToString();
            property.ReplyTo = replyQueueName;
            var message = "Client Request a reply";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "demo-client-requestqueue", property, body);
            Console.WriteLine($"Sending message: {property.CorrelationId}");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Reply - Received message: {message}");
            };
            channel.BasicConsume(replyQueueName, true, consumer);

        }
    }
}

