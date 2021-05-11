using System;

namespace SuplaNotificationIntegration
{
    public static class EnvKeys
    {
        public static string StorageConnString => Environment.GetEnvironmentVariable("SNI_STORAGE_ACC_CONNECTION_STRING");
        public static string SniContainer => Environment.GetEnvironmentVariable("SNI_CONTAINER");
        public static string DevicesFile => Environment.GetEnvironmentVariable("SNI_DEVICES_FILENAME");
        public static string SubscribersFile => Environment.GetEnvironmentVariable("SNI_SUBSCRIBERS_FILENAME");
        public static string AlertLogsFolder => Environment.GetEnvironmentVariable("SNI_ALERT_LOGS_FOLDERNAME");
        public static string SNIDbKey => Environment.GetEnvironmentVariable("SNI_DB_KEY");
        public static string SNIDbName => Environment.GetEnvironmentVariable("SNI_DB_NAME");
        public static string SNIDbUri => Environment.GetEnvironmentVariable("SNI_DB_URI");
        public static string SNISendGridMail => Environment.GetEnvironmentVariable("SNI_SendGrid_Mail");
        public static string SNISendGridApiKey => Environment.GetEnvironmentVariable("SNI_SendGrid_Api_Key");
    }
}
