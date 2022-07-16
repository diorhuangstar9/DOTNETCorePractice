// See https://aka.ms/new-console-template for more information

using Grpc.Net.Client;
using GrpcGreeterClient;
using GrpcTestClient;

// using var channel = GrpcChannel.ForAddress("http://localhost:5288");
// var client = new Greeter.GreeterClient(channel);
// var reply = await client.SayHelloAsync(
//     new HelloRequest { Name = "GreeterClientDiorTest1" });

using var channel = GrpcChannel.ForAddress("http://localhost:5040");
var client = new Tester.TesterClient(channel);
var reply = await client.TestReceiveAsync(
    new TestRequest { TestVal = "DiorTest2" });

Console.WriteLine("GRPC Testing: " + reply.TestResponse);
Console.WriteLine("Press any key to exit...");
