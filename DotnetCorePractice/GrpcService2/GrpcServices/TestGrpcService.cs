using Grpc.Core;
using GrpcTest;

namespace GrpcService2.GrpcServices;

public class TestGrpcService : Tester.TesterBase
{
    public override Task<TestReply> TestReceive(TestRequest request, ServerCallContext context)
    {
        return Task.FromResult(new TestReply { TestResponse = $"Test for grpc response {request.TestVal}" });
    }
}