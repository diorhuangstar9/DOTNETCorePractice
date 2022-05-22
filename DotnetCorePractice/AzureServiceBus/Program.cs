using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AzureServiceBus
{
    class Program
    {
        const string connectionString = "Endpoint=sb://diorservicebus2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YSidQ6pwKjHB8eeBnaUF/MfKgGH/ZdG/uiF+s8gQCIw=";
        const string queueName = "secondqueue";

        static async Task Main(string[] args)
        {
            

            var queueClient = new QueueClient(connectionString, queueName);
            string messageBody = JsonConvert.SerializeObject(new PersonModel
            {
                FirstName = "Dior3",
                LastName = "Huang"
            });
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            try
            {
                await queueClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            Console.WriteLine($"Send message: {messageBody}");
        }
    }
}
