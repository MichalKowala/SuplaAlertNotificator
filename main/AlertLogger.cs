using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuplaNotificationIntegration
{
    public class AlertLogger
    {
        public void Log(string message)
        {
            string fileName = Environment.GetEnvironmentVariable(EnvKeys.AlertLogsFolder) + "/" + DateTime.Now.ToString("dd.MM.yyyy");
            CloudStorageAccount storage = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable(EnvKeys.StorageConnString));
            CloudBlobClient client = storage.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(Environment.GetEnvironmentVariable(EnvKeys.SniContainer));
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                if (blob.Exists())
                blob.DownloadToStream(stream);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(message);
                    sw.Flush();
                    stream.Position = 0;
                    blob.UploadFromStream(stream);
                }
            }
        }
    }
}
