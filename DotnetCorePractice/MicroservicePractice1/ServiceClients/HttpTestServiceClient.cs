namespace MicroservicePractice1.ServiceClients;

public class HttpTestServiceClient : ITestServiceClient
{
    private HttpClient _httpClient;   
    
    public HttpTestServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTestServiceItem()
    {
        var response = await _httpClient.GetAsync("WeatherForecast/items");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();
        return string.Empty;
    }
}