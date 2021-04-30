using System;
using System.Collections.Generic;
using System.Text;

namespace SuplaNotificationIntegration
{
    public static class EnvKeys
    {
        public static string StorageConnString => "SNI_STORAGE_ACC_CONNECTION_STRING";
        public static string SniContainer => "SNI_CONTAINER";
        public static string DevicesFile => "SNI_DEVICES_FILENAME";
        public static string SubscribersFile => "SNI_SUBSCRIBERS_FILENAME";
        public static string AlertLogsFolder => "SNI_ALERT_LOGS_FOLDERNAME";
    }
}
