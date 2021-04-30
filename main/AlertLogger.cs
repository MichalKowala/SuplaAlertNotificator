using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;

namespace SuplaNotificationIntegration
{
    public class AlertLogger
    {
        public void Log(string message)
        {
            string fileName = Environment.GetEnvironmentVariable(EnvKeys.AlertLogsFolder) + "/" + DateTime.Now.ToString("dd.MM.yyyy");
            StorageAccessHelper storage = new StorageAccessHelper();
            CloudBlockBlob blob = storage.GetBlockBlobReference(fileName);
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
