using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace RabbitMQ.Producer
{
    public class AzureServiceBusProducer
    {
        const string connectionString = "Endpoint=sb://diorservicebus2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YSidQ6pwKjHB8eeBnaUF/MfKgGH/ZdG/uiF+s8gQCIw=";
        const string queueName = "secondqueue";
        //static IQueueClient queueClient;

        public static async void Produce()
        {
            var queueClient = new QueueClient(connectionString, queueName);
            string messageBody = JsonConvert.SerializeObject(new PersonModel
            {
                FirstName = "Dior",
                LastName = "Huang"
            });
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            try
            {
                await queueClient.SendAsync(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            
            Console.WriteLine($"Send message: {messageBody}");
        }

    }
}

