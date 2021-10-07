using AuthorizePractice.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthorizePractice
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
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    //options.Authority = "https://localhost:5001";
                    //options.Authority = "https://auth-gluu-test.ysd.com";
                    //options.Audience = "fa2ba423-fb86-42f3-91df-54c6b352acb8";
                    //options.Audience = "api2";

                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateAudience = false
                    //};
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TestPolicy", policy =>
                {
                    policy.Requirements.Add(new TestRequirement(2));
                    policy.Requirements.Add(new TestRequirement2(5));

                });
            });
            services.AddSingleton<IAuthorizationHandler, TestHandler>();
            services.AddSingleton<IAuthorizationHandler, TestHandler2>();
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
