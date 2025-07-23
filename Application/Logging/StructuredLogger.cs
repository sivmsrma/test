using System;
using System.IO;
using System.Text.Json;

namespace Terret_Billing.Application.Logging
{
    public static class StructuredLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", $"structured_{DateTime.Now:yyyyMMdd}.log");
        private static readonly object LockObject = new object();

        public static void Log(string level, string message, object data = null)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now,
                Level = level,
                Message = message,
                Data = data
            };
            var json = JsonSerializer.Serialize(logEntry);
            lock (LockObject)
            {
                File.AppendAllText(LogFilePath, json + Environment.NewLine);
            }
        }
    }
}
