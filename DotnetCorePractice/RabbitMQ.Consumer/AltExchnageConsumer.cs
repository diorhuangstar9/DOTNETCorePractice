using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class AltExchnageConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-alterexchange", ExchangeType.Fanout);
            channel.ExchangeDeclare("demo-mainexchange", ExchangeType.Direct, arguments: new Dictionary<string, object>
            {
                { "alternate-exchange", "demo-alterexchange"}
            });

            channel.QueueDeclare("mainexchangequeue");
            channel.QueueDeclare("alterexchangequeue");
            channel.QueueBind("mainexchangequeue", "demo-mainexchange", "test");
            channel.QueueBind("alterexchangequeue", "demo-alterexchange", "");
            var mainConsumer = new EventingBasicConsumer(channel);
            mainConsumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"main consumer: {message}");
            };
            channel.BasicConsume("mainexchangequeue", true, mainConsumer);

            var alterConsumer = new EventingBasicConsumer(channel);
            alterConsumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"alter consumer: {message}");
            };
            channel.BasicConsume("alterexchangequeue", true, alterConsumer);

        }
    }
}

