using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration
{
    public static class Function
    {
        [FunctionName("Executor")]
        public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            string connectionString = Environment.GetEnvironmentVariable("SNISTORAGEACCOUNT");
            var account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient _client = account.CreateCloudBlobClient();
            CloudBlobContainer _container = _client.GetContainerReference("sni-container");
            var subscribers = await _container.GetBlockBlobReference("subscribers.json").DownloadTextAsync();
            var sensors = await _container.GetBlockBlobReference("sensors.json").DownloadTextAsync();
        }
    }
}
