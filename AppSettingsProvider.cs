using System;
namespace VivaAguascalientesAPI
{
    public static class AppSettingsProvider
    {
        public static string DbConnectionString { get; set; }
        public static string CommandTimeoutForDb { get; set; }
        
    }
}
