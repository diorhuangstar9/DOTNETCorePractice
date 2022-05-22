using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBus.Consumer
{
	public class OrderConsumerService
	{
		public OrderConsumerService(ServiceBusProcessor processor)
		{

            //var processor = serviceBusClient.CreateProcessor(
            //    queueName, options);

            // configure the message and error handler to use
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                string body = args.Message.Body.ToString();
                Console.WriteLine($"New order {body}");

                // we can evaluate application logic and use that to determine how to settle the message.
                await args.CompleteMessageAsync(args.Message);
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // the error source tells me at what point in the processing an error occurred
                Console.WriteLine(args.ErrorSource);
                // the fully qualified namespace is available
                Console.WriteLine(args.FullyQualifiedNamespace);
                // as well as the entity path
                Console.WriteLine(args.EntityPath);
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            }

            // start processing
            processor.StartProcessingAsync().GetAwaiter().GetResult();


        }
	}
}

