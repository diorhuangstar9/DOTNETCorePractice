using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class HeaderExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-headerexchange", ExchangeType.Headers);
            channel.QueueDeclare("letterbox");

            var bindingArguments = new Dictionary<string, object>
            {
                {"x-match", "all"},
                {"name", "brian"},
                {"age", "21"}
            };

            channel.QueueBind("letterbox", "demo-headerexchange", "", bindingArguments);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message);
            };

            channel.BasicConsume("letterbox", true, consumer);
        }
    }
}

