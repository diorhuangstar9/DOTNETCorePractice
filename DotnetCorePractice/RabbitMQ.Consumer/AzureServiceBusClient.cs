using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace RabbitMQ.Consumer
{
    public class AzureServiceBusClient
    {

        const string connectionString = "Endpoint=sb://diorservicebus2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YSidQ6pwKjHB8eeBnaUF/MfKgGH/ZdG/uiF+s8gQCIw=";
        const string queueName = "secondqueue";
        private static IQueueClient queueClient;

        public static async void Consume()
        {
            queueClient = new QueueClient(connectionString, queueName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            Console.ReadLine();
            await queueClient.CloseAsync();
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            var jsonString = Encoding.UTF8.GetString(message.Body);
            var person = JsonConvert.DeserializeObject<PersonModel>(jsonString);
            Console.WriteLine($"Person Received: {person.FirstName} {person.LastName}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message Handler Exception: {arg.Exception}");
            return Task.CompletedTask;
        }
    }

}

