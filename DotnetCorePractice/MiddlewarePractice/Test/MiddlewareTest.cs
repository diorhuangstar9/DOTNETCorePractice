using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace MiddlewarePractice.Test
{
    public class MiddlewareTest
    {
        public MiddlewareTest()
        {
        }

        [Fact]
        public async Task MiddlewareTest_ReturnsNotFoundForRequest()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddMyServices();
                        })
                        .Configure(app =>
                        {
                            app.UseMiddleware<MyMiddleware>();
                        });
                })
                .StartAsync();

        }
    }
}
