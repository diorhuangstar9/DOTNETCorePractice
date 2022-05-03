using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class CompetingConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare("demo-competing-queue",
                durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false); 

            var consumer = new EventingBasicConsumer(channel);
            var random = new Random();
            consumer.Received += (sender, e) =>
            {
                var processTime = random.Next(1, 6);
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received: {message} will take {processTime} to process");

                Task.Delay(TimeSpan.FromSeconds(processTime)).Wait();
                channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume("demo-competing-queue", false, consumer);
        }
    }
}

