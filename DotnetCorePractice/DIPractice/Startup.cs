using DIPractice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPractice
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
            services.AddScoped<ScopedTest1Service>();
            services.AddScoped<ScopedTest2Service>();
            services.AddScoped<ScopedTest3Service>();
            services.AddScoped<Func<string, IScopedService>>(
                services => type =>
                {
                    switch (type)
                    {
                        case "test1":
                            return services.GetRequiredService<ScopedTest1Service>();
                        case "test2":
                            return services.GetRequiredService<ScopedTest2Service>();
                        default:
                            return services.GetRequiredService<ScopedTest3Service>();
                    }
                    //using (var scope = services.CreateScope())
                    //{
                    //    var scopedServices = scope.ServiceProvider;
                        
                    //}
                        

                });
            services.AddControllers();
            services.AddScoped<TestExecService>();
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
        }
    }
}
