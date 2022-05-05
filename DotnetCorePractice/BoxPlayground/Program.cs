// See https://aka.ms/new-console-template for more information
using Box.V2;
using Box.V2.Auth;
using Box.V2.Config;

var config = new BoxConfigBuilder("8pcb1hs3by0ou3ze37o34jbba7k8q9is", "mZx2X1Eyz6PfSu2EKUnaRJyuN1N5kpKt", new Uri("http://localhost")).Build();
var session = new OAuthSession("7A5Nq2PcjYcwCPZl5oww9uGaU2iXBibO", "NOT_NEEDED", 3600, "bearer");
var client = new BoxClient(config, session);

//var user = await client.UsersManager.GetCurrentUserInformationAsync();

// Get root folder with default properties
var items = await client.FoldersManager.GetFolderItemsAsync("0", 500);

foreach (var item in items.Entries)
{
    Console.WriteLine(item);
}