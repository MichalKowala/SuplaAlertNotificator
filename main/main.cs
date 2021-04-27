using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SNIClassLibrary;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration
{
    public static class Function
    {
        [FunctionName("Executor")]
        //public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("SNISTORAGEACCOUNT");
            var account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient _client = account.CreateCloudBlobClient();
            CloudBlobContainer _container = _client.GetContainerReference("sni-container");
            var subscribers = await _container.GetBlockBlobReference("subscribers.json").DownloadTextAsync();
            var sensorsText = await _container.GetBlockBlobReference("sensors.json").DownloadTextAsync();
            DevicesFileContent devices = JsonConvert.DeserializeObject<DevicesFileContent>(sensorsText);
            return new OkObjectResult("ok");
        }
    }
}
