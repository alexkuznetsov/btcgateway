using System;

namespace BTCGatewayAPI.Common.Logging
{
    public delegate ILoggingBackend LoggingBackendBuilder(string sourceName, string sourceMethod);
    /// <summary>
    /// Факбрика получения бэкэнда логирования
    /// </summary>
    public static class LoggerBackendFactory
    {
        public static LoggingBackendBuilder LoggingBackendBuilder { get; set; }

        public static void ConfigureLoggingBackend(LoggingBackendBuilder func) => LoggingBackendBuilder = func;

        /// <summary>
        /// Получить бэкэкнд логирования
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="sourceMethod"></param>
        /// <returns></returns>
        public static ILoggingBackend Create(string sourceName, string sourceMethod)
        {
            return (LoggingBackendBuilder ?? Impl.TraceLoggingBackend.Builder).Invoke(sourceName, sourceMethod);
        }
    }
}
