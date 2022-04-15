using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Threading.Tasks;
using CsvHelper;
using RestSharp;
using System.Collections.Generic;
using System.Threading;

namespace NovaProductUpload
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (!args[0].StartsWith("env="))
            {
                Console.WriteLine("Wrong parameter, need parameter:env={env}");
                return;
            }
            var env = args[0].Replace("env=", string.Empty);
            Console.WriteLine($"env is {env}");
            var needToOnshelf = true;
            var endpointToReplace = env.Equals("PRD", StringComparison.OrdinalIgnoreCase) ?
                "https://vn-api-ApiToReplace.ysd.com" : $"https://vn-api-ApiToReplace-{env.ToLower()}.ysd.com";
            var adminEndpoint = endpointToReplace.Replace("ApiToReplace", "admin");
            var targetFolder = "/Users/diorhuang/diorhuang/dior_work/ProductUpload/VistaNova/UploadTargets";
            var targetUploadName = Path.Combine(targetFolder, $"ProductToUpload-{env}.csv");
            var resultFolder = "/Users/diorhuang/diorhuang/dior_work/ProductUpload/VistaNova/UploadResults";
            var username = "hehuang@cimpress.com.cn";
            var password = "4rfv$RFV3edc#EDC";
            // get token
            var token = await GetUserTokenAsync(adminEndpoint, username, password);
            Console.WriteLine("Upload Product Start");
            var postResults = new List<ProductUploadResult>();
            // loop csv file
            using (var reader = new StreamReader(targetUploadName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var anonymousTypeDefinition = new
                {
                    ItemNumber = string.Empty
                };
                var records = csv.GetRecords(anonymousTypeDefinition).ToList();
                for (var i = 0; i < records.Count; i++)
                {
                    var r = records[i];
                    // compress folder
                    var folderToCompress = Path.Combine(targetFolder, r.ItemNumber);
                    if (Directory.Exists(folderToCompress))
                    {
                        var resultZipPath = Path.Combine(resultFolder,
                            $"{r.ItemNumber}_{DateTime.UtcNow.Ticks}.zip");
                        try
                        {
                            ZipFile.CreateFromDirectory(folderToCompress, resultZipPath);
                            // post product
                            postResults.Add(await PostProductAsync(endpointToReplace.Replace(
                                "ApiToReplace", "product"), token, resultZipPath));
                            File.Delete(resultZipPath);
                            Console.WriteLine($"{i + 1}/{records.Count} Done");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{i + 1}/{records.Count} Error: {ex}");
                        }
                    }
                    else
                        Console.WriteLine($"{i + 1}/{records.Count} No corresponding folder");
                }
            }
            Console.WriteLine("Upload Product End");
            // set current version and isShow
            if (postResults.Count > 0 && needToOnshelf)
            {
                Console.WriteLine("Waiting for Product Sync To Manufacture");
                Thread.Sleep(3000 * postResults.Count);
                Console.WriteLine("Onshelf Product Start");
                for (var i = 0; i < postResults.Count; i++)
                {
                    try
                    {
                        var postResult = postResults[i];
                        var productVersions = await GetProuctVersionsAsync(adminEndpoint, token, postResult.itemNumber);
                        var postedVersion = productVersions.FirstOrDefault(x => x.versionNo == postResult.version);
                        if (postedVersion == null)
                        {
                            Console.WriteLine($"get version error in {postedVersion.itemNumber}");
                            continue;
                        }
                        await SetProuctCurrentVersionAsync(adminEndpoint, token, postedVersion);
                        await OnShelfProuctAsync(adminEndpoint, token, postedVersion.productId);
                        Console.WriteLine($"{i + 1}/{postResults.Count} Done.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{i + 1}/{postResults.Count} Error: {ex}");
                    }
                }
                Console.WriteLine("Onshelf Product End");
            }
        }

        private static async Task SetProuctCurrentVersionAsync(string endpoint, string token,
            ProductVersion productVersion)
        {
            var restClient = new RestClient(endpoint);
            var request = new RestRequest("/api/Product/{productId}/SetVersionCurrent", Method.Put);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddUrlSegment("productId", productVersion.productId);
            request.AddJsonBody(new
            {
                isCurrent = true,
                itemNumber = productVersion.itemNumber,
                versionNo = productVersion.versionNo
            });
            var response = await restClient.ExecuteAsync<NovaResult<bool>>(request);
            if (response.IsSuccessful && response.Data != null &&
                response.Data.success && response.Data.data)
                return;
            throw new Exception($"SetProuctVersionCurrentAsync Error, {response.Content}");
        }

        private static async Task OnShelfProuctAsync(string endpoint, string token, int productId)
        {
            var restClient = new RestClient(endpoint);
            var request = new RestRequest("/api/Product/Product/OnShelfBatch", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(new { productBriefIds = new int[] { productId } });
            var response = await restClient.ExecuteAsync<NovaResult<bool>>(request);
            if (response.IsSuccessful && response.Data != null &&
                response.Data.success && response.Data.data)
                return;
            throw new Exception($"OnShelfProuctAsync Error, {response.Content}");
        }

        private static async Task<IEnumerable<ProductVersion>> GetProuctVersionsAsync(
            string endpoint, string token, string itemNumber)
        {
            var restClient = new RestClient(endpoint);
            var request = new RestRequest("/api/Product/VersionComments");
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddQueryParameter("itemNumber", itemNumber);
            var response = await restClient.ExecuteAsync<
                NovaResult<IEnumerable<ProductVersion>>>(request);
            if (response.IsSuccessful && response.Data != null &&
                response.Data.success && response.Data.data.Count() > 0)
                return response.Data.data;
            throw new Exception($"GetProuctVersionsAsync Error, {response.Content}");
        }

        private static async Task<string> GetUserTokenAsync(string endpoint,
            string username, string password)
        {
            var restClient = new RestClient(endpoint);
            var request = new RestRequest("/api/Admin/Login", Method.Post);
            request.AddJsonBody(new { username, password });
            var response = await restClient.ExecuteAsync<NovaResult<AdminUserLogin>>(request);
            if (response.IsSuccessful && response.Data != null && response.Data.success &&
                !string.IsNullOrWhiteSpace(response.Data.data?.token))
                return response.Data.data.token;
            throw new Exception($"GetUserTokenAsync Error, {response.Content}");
        }

        private static async Task<ProductUploadResult> PostProductAsync(string endpoint,
            string token, string postFilePath)
        {
            var restClient = new RestClient(endpoint);
            var request = new RestRequest("/api/Product", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddFile("file", postFilePath);
            var response = await restClient.ExecuteAsync<NovaResult<ProductUploadResult>>(request);
            if (response.IsSuccessful && response.Data != null &&
                response.Data.success && response.Data.data != null &&
                !string.IsNullOrWhiteSpace(response.Data.data.itemNumber) &&
                response.Data.data.version != 0 &&
                !string.IsNullOrWhiteSpace(response.Data.data.mcpSku))
                return response.Data.data;
            throw new Exception($"PostProductAsync Error, {response.Content}");
        }

    }

    public class NovaResult<T>
    {
        public T data { get; set; }
        public bool success { get; set; }

    }

    public class AdminUserLogin
    {
        public string username { get; set; }
        public string token { get; set; }
    }

    public class ProductUploadResult
    {
        public string itemNumber { get; set; }
        public int version { get; set; }
        public string mcpSku { get; set; }
    }

    public class ProductVersion
    {
        public string itemNumber { get; set; }
        public int versionNo { get; set; }
        public int productId { get; set; }

    }
}
