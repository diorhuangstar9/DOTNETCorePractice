using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class DLXConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-dlx", ExchangeType.Fanout);
            channel.ExchangeDeclare("demo-mainexchange2", ExchangeType.Direct);

            channel.QueueDeclare("mainexchange2queue", arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "demo-dlx"},
                { "x-message-ttl", 5000}
            });
            channel.QueueBind("mainexchange2queue", "demo-mainexchange2", "test");
            var mainConsumer = new EventingBasicConsumer(channel);
            mainConsumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"main consumer: {message}");
            };
            channel.BasicConsume("mainexchange2queue", true, mainConsumer);

            channel.QueueDeclare("dlxqueue");
            channel.QueueBind("dlxqueue", "demo-dlx", "");
            var dlxConsumer = new EventingBasicConsumer(channel);
            dlxConsumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"dlx consumer: {message}");
            };
            channel.BasicConsume("dlxqueue", true, dlxConsumer);

        }
    }
}

