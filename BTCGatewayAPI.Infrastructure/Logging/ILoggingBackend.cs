using System;

namespace BTCGatewayAPI.Common.Logging
{
    public interface ILoggingBackend : ILog
    {
        string SourceName { get; }
        void SetParam(string name, string value);

        LogEntryTypeLevel LogLevel { get; }
    }
}
