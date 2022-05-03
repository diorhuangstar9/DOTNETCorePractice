using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class CompetingProducer
    {
        public static void Publish(IModel channel)
        {
            channel.QueueDeclare("demo-competing-queue",
                durable: true, exclusive: false, autoDelete: false, arguments: null);
            var random = new Random();
            for (var count = 1; ; count++)
            {
                var publishingTime = random.Next(1, 4);
                var message = new { Name = "Producer", Message = $"Hello! Count:{count}" };
                Console.WriteLine($"Sent: {message} ");
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("", "demo-competing-queue", null, body);
                Task.Delay(TimeSpan.FromSeconds(publishingTime)).Wait();
            }
        }
    }
}

