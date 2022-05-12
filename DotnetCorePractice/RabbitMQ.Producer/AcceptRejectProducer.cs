using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    public class AcceptRejectProducer
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-acceptrejectexchange", ExchangeType.Fanout);
            var message = "This message is from demo acceptreject";
            var body = Encoding.UTF8.GetBytes(message);

            while (true)
            {
                channel.BasicPublish("demo-acceptrejectexchange", "", null, body);
                Console.WriteLine($"Send Message : {message}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }
    }
}

