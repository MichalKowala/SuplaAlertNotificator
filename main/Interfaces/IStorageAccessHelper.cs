using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IStorageAccessHelper
    {
        CloudBlockBlob GetSNIContainerBlockBlobReference(string filename);
    }
}
