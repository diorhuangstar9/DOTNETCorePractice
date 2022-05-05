using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Producer
{
    public class MQServer
    {
        public static void Publish(IModel channel)
        {
            channel.QueueDeclare("demo-client-requestqueue", exclusive: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Request - Received message: {e.BasicProperties.CorrelationId}");

                var replyMessage = $"This is the reply: {e.BasicProperties.CorrelationId}";
                var replyBody = Encoding.UTF8.GetBytes(replyMessage);
                channel.BasicPublish("", e.BasicProperties.ReplyTo, null, replyBody);
            };
            channel.BasicConsume("demo-client-requestqueue", true, consumer);



        }
    }
}

