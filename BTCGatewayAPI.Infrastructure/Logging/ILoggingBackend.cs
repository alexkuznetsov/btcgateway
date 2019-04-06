using System;

namespace BTCGatewayAPI.Infrastructure.Logging
{
    public interface ILoggingBackend : ILog
    {
        string SourceName { get; }
        void SetParam(string name, string value);
    }
}
