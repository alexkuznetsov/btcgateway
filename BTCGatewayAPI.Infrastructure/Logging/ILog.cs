namespace BTCGatewayAPI.Common.Logging
{
    using System;

    /// <summary>
    /// Журнал
    /// </summary>
    public interface ILog
    {
        void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level);
        void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1);
        void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1, object arg2);
        void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, params object[] parameters);
    }
}
