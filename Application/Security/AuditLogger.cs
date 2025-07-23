using System;
using System.IO;

namespace Terret_Billing.Application.Security
{
    public static class AuditLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", $"audit_{DateTime.Now:yyyyMMdd}.log");
        private static readonly object LockObject = new object();
        public static void Log(string action, string user, string details = null)
        {
            var entry = $"{DateTime.Now:o} | {user} | {action} | {details}";
            lock (LockObject)
            {
                File.AppendAllText(LogFilePath, entry + Environment.NewLine);
            }
        }
    }
}
