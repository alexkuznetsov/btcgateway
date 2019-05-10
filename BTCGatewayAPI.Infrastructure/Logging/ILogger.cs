using System;

namespace BTCGatewayAPI.Common.Logging
{
    /// <summary>
    /// Интерфейс логгирования с разделением по типу события
    /// </summary>
    public interface ILogger : ILog
    {
        bool CanLog(LogEntryTypeLevel level);

        #region Info

        void Info(string message);
        void Info(string message, object arg1);
        void Info(string message, object arg1, object arg2);
        void Info(string message, params object[] arguments);
        void InfoSource(string sourceName, string message);
        void InfoSource(string sourceName, string message, object arg1);
        void InfoSource(string sourceName, string message, object arg1, object arg2);
        void InfoSource(string sourceName, string message, params object[] arguments);

        #endregion

        #region Warn

        void Warn(string message);
        void Warn(string message, object arg1);
        void Warn(string message, object arg1, object arg2);
        void Warn(string message, params object[] arguments);
        void WarnSource(string sourceName, string message);
        void WarnSource(string sourceName, string message, object arg1);
        void WarnSource(string sourceName, string message, object arg1, object arg2);
        void WarnSource(string sourceName, string message, params object[] arguments);

        #endregion

        #region Error

        void Error(string message);
        void Error(string message, object arg1);
        void Error(string message, object arg1, object arg2);
        void Error(string message, params object[] arguments);
        void ErrorSource(string sourceName, string message);
        void ErrorSource(string sourceName, string message, object arg1);
        void ErrorSource(string sourceName, string message, object arg1, object arg2);
        void ErrorSource(string sourceName, string message, params object[] arguments);

        void Error(Exception exception);
        void Error(Exception exception, string message);
        void Error(Exception exception, string message, object arg1);
        void Error(Exception exception, string message, object arg1, object arg2);
        void Error(Exception exception, string message, params object[] arguments);
        void ErrorSource(Exception exception, string sourceName, string message);
        void ErrorSource(Exception exception, string sourceName, string message, object arg1);
        void ErrorSource(Exception exception, string sourceName, string message, object arg1, object arg2);
        void ErrorSource(Exception exception, string sourceName, string message, params object[] arguments);

        #endregion

        #region Fatal

        void Fatal(string message);
        void Fatal(string message, object arg1);
        void Fatal(string message, object arg1, object arg2);
        void Fatal(string message, params object[] arguments);
        void FatalSource(string sourceName, string message);
        void FatalSource(string sourceName, string message, object arg1);
        void FatalSource(string sourceName, string message, object arg1, object arg2);
        void FatalSource(string sourceName, string message, params object[] arguments);

        void Fatal(Exception exception);
        void Fatal(Exception exception, string message);
        void Fatal(Exception exception, string message, object arg1);
        void Fatal(Exception exception, string message, object arg1, object arg2);
        void Fatal(Exception exception, string message, params object[] arguments);
        void FatalSource(Exception exception, string sourceName, string message);
        void FatalSource(Exception exception, string sourceName, string message, object arg1);
        void FatalSource(Exception exception, string sourceName, string message, object arg1, object arg2);
        void FatalSource(Exception exception, string sourceName, string message, params object[] arguments);

        #endregion

        #region Debug

        void Debug(string message);
        void Debug(string message, object arg1);
        void Debug(string message, object arg1, object arg2);
        void Debug(string message, params object[] arguments);
        void DebugSource(string sourceName, string message);
        void DebugSource(string sourceName, string message, object arg1);
        void DebugSource(string sourceName, string message, object arg1, object arg2);
        void DebugSource(string sourceName, string message, params object[] arguments);

        #endregion

        #region Trace

        void Trace(string message);
        void Trace(string message, object arg1);
        void Trace(string message, object arg1, object arg2);
        void Trace(string message, params object[] arguments);
        void TraceSource(string sourceName, string message);
        void TraceSource(string sourceName, string message, object arg1);
        void TraceSource(string sourceName, string message, object arg1, object arg2);
        void TraceSource(string sourceName, string message, params object[] arguments);

        #endregion
    }
}
