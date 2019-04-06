namespace BTCGatewayAPI.Infrastructure.Logging
{
    /// <summary>
    /// Факбрика получения бэкэнда логирования
    /// </summary>
    class LoggerBackendFactory
    {
        /// <summary>
        /// Получить бэкэкнд логирования
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="sourceMethod"></param>
        /// <returns></returns>
        public static ILoggingBackend Create(string sourceName, string sourceMethod)
        {
            return new Impl.TraceLoggingBackend(new LoggerBackendActivationContext
            {
                SourceMethod = sourceMethod,
                SourceName = sourceName
            });
        }
    }
}
