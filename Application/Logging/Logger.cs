using System;
using System.IO;

namespace Terret_Billing.Application.Logging
{
    /// <summary>
    /// Static Logger class for application-wide logging
    /// </summary>
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "application.log");
        private static readonly object LockObject = new object();

        static Logger()
        {
            // Ensure log directory exists
            var logDirectory = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public static void LogInfo(string message)
        {
            WriteToLog("INFO", message);
        }

        public static void LogWarning(string message)
        {
            WriteToLog("WARNING", message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            string logMessage = message;
            if (ex != null)
            {
                logMessage += $" | Exception: {ex.Message} | StackTrace: {ex.StackTrace}";
            }
            WriteToLog("ERROR", logMessage);
        }

        private static void WriteToLog(string level, string message)
        {
            try
            {
                lock (LockObject)
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                    
                    // Also write to console for debugging purposes
                    Console.WriteLine(logEntry);
                }
            }
            catch
            {
                // Silently fail if logging fails - we don't want to crash the application due to logging issues
            }
        }

    }
}
