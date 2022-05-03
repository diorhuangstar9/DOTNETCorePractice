using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public static class MyQueueProducer
    {
        public static void Publish(IModel channel)
        {
            channel.QueueDeclare("demo-queue",
                durable: true, exclusive: false, autoDelete: false, arguments: null);
            for (var count = 0; ; count++)
            {
                var message = new { Name = "Producer", Message = $"Hello! Count:{count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("", "demo-queue", null, body);
                Thread.Sleep(1000);
            }
        }
    }
}

