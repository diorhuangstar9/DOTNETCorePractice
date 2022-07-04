namespace MicroservicePractice1.ServiceClients;

public interface ITestServiceClient
{

    Task<string> GetTestServiceItem();    
}