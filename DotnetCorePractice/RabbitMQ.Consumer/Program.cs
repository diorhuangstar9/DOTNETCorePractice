// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Consumer;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
//channel.QueueDeclare("demo-queue",
//    durable: true, exclusive: false, autoDelete: false, arguments: null);

//var consumer = new EventingBasicConsumer(channel);
//consumer.Received += (sender, e) =>
//{
//    var body = e.Body.ToArray();
//    var message = Encoding.UTF8.GetString(body);
//    Console.WriteLine(message);
//};

//channel.BasicConsume("demo-queue", true, consumer);
//RabbitMQ.Consumer.MyQueueConsumer.Consume(channel);
//Console.WriteLine("Consuming");

//if ("Payments".Equals(args[0], StringComparison.OrdinalIgnoreCase))
//    PaymentsConsumer.Consume(channel);
//else if ("Analytics".Equals(args[0], StringComparison.OrdinalIgnoreCase))
//    AnalyticsConsumer.Consume(channel);
//else if ("User".Equals(args[0], StringComparison.OrdinalIgnoreCase))
//    UserConsumer.Consume(channel);

MQClient.Consume(channel);
Console.ReadLine();