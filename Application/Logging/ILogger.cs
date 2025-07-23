using System;

namespace Terret_Billing.Application.Logging
{
    public interface ILogger
    {
        void LogError(string message, Exception ex);
        void LogInfo(string message);
    }
}
