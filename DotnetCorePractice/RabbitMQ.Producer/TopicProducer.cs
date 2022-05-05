using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
	public class TopicProducer
	{
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-topicexchange", ExchangeType.Topic);
            var message1 = "A european user paid for something";
            var body1 = Encoding.UTF8.GetBytes(message1);
            channel.BasicPublish("demo-topicexchange", "user.europe.payments", null, body1);
            Console.WriteLine($"Send Message : {message1}");

            var message2 = "A european business ordered goods";
            var body2 = Encoding.UTF8.GetBytes(message2);
            channel.BasicPublish("demo-topicexchange", "business.europe.order", null, body2);
            Console.WriteLine($"Send Message : {message2}");
        }
    }
}

