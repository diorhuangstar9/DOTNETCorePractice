using GrpcService2.GrpcServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options => 
    options.ListenLocalhost(5040, o => o.Protocols =
    HttpProtocols.Http2));

var app = builder.Build();

app.MapGrpcService<TestGrpcService>();
app.MapGet("/", () => "Hello World!");

app.Run();