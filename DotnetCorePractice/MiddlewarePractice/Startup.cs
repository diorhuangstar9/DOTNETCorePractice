using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using MiddlewarePractice.Middleware;

namespace MiddlewarePractice
{
    public class Startup
    {
        private const string enUSCulture = "en-US";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(enUSCulture),
                    new CultureInfo("fr")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
                {
                    // My custom request culture logic
                    var locale = context.Request.Path.Value.Replace("/weatherforecast/", string.Empty);
                    return Task.FromResult(new ProviderCultureResult(locale));
                }));
            });
            services.AddControllers();
        }

        private void HandleLocale(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var locale = context.Request.Path.Value.Replace("weatherforecast", string.Empty);
                context.Items.Add("locale", locale);

                // Do work that doesn't write to the Response.
                await next();
                // Do other work that doesn't write to the Response.
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();
            //app.UseHttpsRedirection();

            //app.UseRequestLocalization();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseWhen(context => context.Request.Path.Value.StartsWith("weatherforecast"),
            //    appBuilder => HandleLocale(appBuilder));
            app.UseRequestCulture();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.Use(async (context, next)=> { await next.Invoke(); });
            //app.Run(context => {  return null; });
        }
    }
}
