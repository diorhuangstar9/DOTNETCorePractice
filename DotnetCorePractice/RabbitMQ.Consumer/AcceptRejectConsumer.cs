using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class AcceptRejectConsumer
    {
        public static void Consume(IModel channel)
        {

            channel.ExchangeDeclare("demo-acceptrejectexchange", ExchangeType.Fanout);
            channel.QueueDeclare("acceptrejectexchangequeue");
            channel.QueueBind("acceptrejectexchangequeue", "demo-acceptrejectexchange", "");
            var mainConsumer = new EventingBasicConsumer(channel);
            mainConsumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (e.DeliveryTag % 5 == 0)
                {
                    //channel.BasicAck(e.DeliveryTag, true);
                    channel.BasicNack(e.DeliveryTag, true, false);
                }

                Console.WriteLine($"main consumer: {message}");

            };
            channel.BasicConsume("acceptrejectexchangequeue", false, mainConsumer);


        }
    }
}

