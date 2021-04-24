using Microsoft.Azure.WebJobs;
using System;

namespace SuplaNotificationIntegration
{
    public static class Function
    {
        [FunctionName("Executor")]
        public static void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            Console.WriteLine("Test123");
        }
    }
}
