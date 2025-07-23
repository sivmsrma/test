using System;
using System.IO;

namespace Terret_Billing.Infrastructure.Helpers
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app.log");
        
        static Logger()
        {
            // Ensure log directory exists
            var logDirectory = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }
        
        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }
        
        public static void LogError(string message, Exception ex = null)
        {
            var errorMessage = ex != null ? $"{message}. Exception: {ex.Message}. StackTrace: {ex.StackTrace}" : message;
            Log("ERROR", errorMessage);
        }
        
        public static void LogWarning(string message)
        {
            Log("WARNING", message);
        }
        
        private static void Log(string level, string message)
        {
            try
            {
                var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // Fail silently if logging fails
            }
        }
    }
}
