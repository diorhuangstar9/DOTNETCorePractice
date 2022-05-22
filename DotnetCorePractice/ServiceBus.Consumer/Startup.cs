using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServiceBus.Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddHostedService<CustomerConsumerService>();
            //services.AddSingleton<ISubscriptionClient>(x =>
            //    new SubscriptionClient(Configuration.GetValue<string>("ServiceBus:ConnectionString"),
            //        Configuration.GetValue<string>("ServiceBus:TopicName"),
            //        Configuration.GetValue<string>("ServiceBus:SubscriptionName")));
            services.AddSingleton(x =>
                {
                    var serviceBusClient = new ServiceBusClient(Configuration.GetValue<string>("ServiceBus:ConnectionString"));
                    return serviceBusClient.CreateProcessor(
                        Configuration.GetValue<string>("ServiceBus:TopicName"),
                        Configuration.GetValue<string>("ServiceBus:SubscriptionName2"),
                        new ServiceBusProcessorOptions { MaxConcurrentCalls = 1, AutoCompleteMessages = false });
                });
            services.AddSingleton(sp =>
                {
                    var processor = sp.GetRequiredService<ServiceBusProcessor>();
                    var orderConsumerService = new OrderConsumerService(processor);
                    return orderConsumerService;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var orderConsumerService = app.ApplicationServices.GetRequiredService<OrderConsumerService>();
        }
    }
}

