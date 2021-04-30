using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace SuplaNotificationIntegration
{
    public static class Function
    {

        //public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
        //    [Blob("sni-contaier/sensors.json", System.IO.FileAccess.Read)] Stream myBlob)
        ////public static async Task<IActionResult> Run(
        [FunctionName("Executor")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
              ILogger log)
        {
            ReportManager rM = new ReportManager();
            string message = rM.PrepareReportMessage();
            await rM.MailTheReport(message);
            return new OkObjectResult("ok");
        }
    }
}


