using System;

namespace BTCGatewayAPI.Infrastructure.Logging
{
    /// <summary>
    /// Абстрактное тело логгера
    /// </summary>
    public abstract class AbstractLogger : ILogger
    {
        public abstract string SourceName { get; }

        public abstract bool CanLog(LogEntryTypeLevel level);

        public abstract void SetParam(string name, string value);

        public abstract void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level);
        public abstract void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1);
        public abstract void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1, object arg2);
        public abstract void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, params object[] parameters);

        #region Debug

        public void Debug(string message) => Write(SourceName, message, null, LogEntryTypeLevel.Debug);

        public void Debug(string message, object arg1) => Write(SourceName, message, null, LogEntryTypeLevel.Debug, arg1);

        public void Debug(string message, object arg1, object arg2) => Write(SourceName, message, null, LogEntryTypeLevel.Debug, arg1, arg2);

        public void Debug(string message, params object[] arguments) => Write(SourceName, message, null, LogEntryTypeLevel.Debug, arguments);

        public void DebugSource(string sourceName, string message) => Write(sourceName, message, null, LogEntryTypeLevel.Debug);

        public void DebugSource(string sourceName, string message, object arg1) => Write(sourceName, message, null, LogEntryTypeLevel.Debug, arg1);

        public void DebugSource(string sourceName, string message, object arg1, object arg2) => Write(sourceName, message, null, LogEntryTypeLevel.Debug, arg1, arg2);

        public void DebugSource(string sourceName, string message, params object[] arguments) => Write(sourceName, message, null, LogEntryTypeLevel.Debug, arguments);

        #endregion

        #region Trace

        public void Trace(string message)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Trace);
        }

        public void Trace(string message, object arg1)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Trace, arg1);
        }

        public void Trace(string message, object arg1, object arg2)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Trace, arg1, arg2);
        }

        public void Trace(string message, params object[] arguments)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Trace, arguments);
        }

        public void TraceSource(string sourceName, string message)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Trace);
        }

        public void TraceSource(string sourceName, string message, object arg1)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Trace, arg1);
        }

        public void TraceSource(string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Trace, arg1, arg2);
        }

        public void TraceSource(string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Trace, arguments);
        }

        #endregion

        #region Info

        public void Info(string message)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Info);
        }

        public void Info(string message, object arg1)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Info, arg1);
        }

        public void Info(string message, object arg1, object arg2)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Info, arg1, arg2);
        }

        public void Info(string message, params object[] arguments)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Info, arguments);
        }

        public void InfoSource(string sourceName, string message)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Info);
        }

        public void InfoSource(string sourceName, string message, object arg1)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Info, arg1);
        }

        public void InfoSource(string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Info, arg1, arg2);
        }

        public void InfoSource(string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Info, arguments);
        }

        #endregion

        #region Warn

        public void Warn(string message)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Warn);
        }

        public void Warn(string message, object arg1)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Warn, arg1);
        }

        public void Warn(string message, object arg1, object arg2)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Warn, arg1, arg2);
        }

        public void Warn(string message, params object[] arguments)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Warn, arguments);
        }

        public void WarnSource(string sourceName, string message)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Warn);
        }

        public void WarnSource(string sourceName, string message, object arg1)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Warn, arg1);
        }

        public void WarnSource(string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Warn, arg1, arg2);
        }

        public void WarnSource(string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Warn, arguments);
        }

        #endregion

        #region Error

        public void Error(string message)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Error);
        }

        public void Error(string message, object arg1)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Error, arg1);
        }

        public void Error(string message, object arg1, object arg2)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Error, arg1, arg2);
        }

        public void Error(string message, params object[] arguments)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Error, arguments);
        }

        public void ErrorSource(string sourceName, string message)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Error);
        }

        public void ErrorSource(string sourceName, string message, object arg1)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Error, arg1);
        }

        public void ErrorSource(string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Error, arg1, arg2);
        }

        public void ErrorSource(string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Error, arguments);
        }

        #region with exception

        public void Error(Exception exception)
        {
            Write(SourceName, null, exception, LogEntryTypeLevel.Error);
        }

        public void Error(Exception exception, string message)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Error);
        }

        public void Error(Exception exception, string message, object arg1)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Error, arg1);
        }

        public void Error(Exception exception, string message, object arg1, object arg2)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Error, arg1, arg2);
        }

        public void Error(Exception exception, string message, params object[] arguments)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Error, arguments);
        }

        public void ErrorSource(Exception exception, string sourceName, string message)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Error);
        }

        public void ErrorSource(Exception exception, string sourceName, string message, object arg1)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Error, arg1);
        }

        public void ErrorSource(Exception exception, string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Error, arg1, arg2);
        }

        public void ErrorSource(Exception exception, string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Error, arguments);
        }

        #endregion

        #endregion

        #region Fatal

        public void Fatal(string message)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Fatal);
        }

        public void Fatal(string message, object arg1)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Fatal, arg1);
        }

        public void Fatal(string message, object arg1, object arg2)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Fatal, arg1, arg2);
        }

        public void Fatal(string message, params object[] arguments)
        {
            Write(SourceName, message, null, LogEntryTypeLevel.Fatal, arguments);
        }

        public void FatalSource(string sourceName, string message)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Fatal);
        }

        public void FatalSource(string sourceName, string message, object arg1)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Fatal, arg1);
        }

        public void FatalSource(string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Fatal, arg1, arg2);
        }

        public void FatalSource(string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, null, LogEntryTypeLevel.Fatal, arguments);
        }

        #region with exception

        public void Fatal(Exception exception)
        {
            Write(SourceName, null, exception, LogEntryTypeLevel.Fatal);
        }

        public void Fatal(Exception exception, string message)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Fatal);
        }

        public void Fatal(Exception exception, string message, object arg1)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Fatal, arg1);
        }

        public void Fatal(Exception exception, string message, object arg1, object arg2)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Fatal, arg1, arg2);
        }

        public void Fatal(Exception exception, string message, params object[] arguments)
        {
            Write(SourceName, message, exception, LogEntryTypeLevel.Fatal, arguments);
        }

        public void FatalSource(Exception exception, string sourceName, string message)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Fatal);
        }

        public void FatalSource(Exception exception, string sourceName, string message, object arg1)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Fatal, arg1);
        }

        public void FatalSource(Exception exception, string sourceName, string message, object arg1, object arg2)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Fatal, arg1, arg2);
        }

        public void FatalSource(Exception exception, string sourceName, string message, params object[] arguments)
        {
            Write(sourceName, message, exception, LogEntryTypeLevel.Fatal, arguments);
        }

        #endregion

        #endregion
    }
}
