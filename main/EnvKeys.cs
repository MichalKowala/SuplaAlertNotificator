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
        public static string SNIDbKey => "SNI_DB_KEY";
        public static string SNIDbName => "SNI_DB_NAME";
        public static string SNIDbUrl => "SNI_DB_URL";
        public static string SNI_SendGrid_Mail => "SNI_SendGrid_Mail";
        public static string SNI_SendGrid_ApiKey => "SNI_SendGrid_Api_Key";
    }
}
