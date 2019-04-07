using System;

namespace BTCGatewayAPI.Infrastructure.Logging
{
    /// <summary>
    /// Логгер
    /// </summary>
    public sealed class Logger : AbstractLogger
    {
        #region privates

        private const string AppSettingLoggingKey = "Logging:Level";

        private readonly ILoggingBackend _backend;
        private readonly LogEntryTypeLevel _logLevel;

        #endregion

        public Logger(ILoggingBackend backend)
        {
            _backend = backend;
            _logLevel = ParseLogLevel();
        }

        /// <summary>
        /// Вообще, это должно быть у backend, но для такой реализации пусть будет тут.
        /// </summary>
        /// <returns></returns>
        private LogEntryTypeLevel ParseLogLevel()
        {
            var temp = System.Configuration.ConfigurationManager.AppSettings[AppSettingLoggingKey] ?? LogEntryTypeLevel.Warn.ToString();

            if (Enum.TryParse<LogEntryTypeLevel>(temp, out var logLevel))
            {
                return logLevel;
            }

            return LogEntryTypeLevel.Warn;
        }

        public override bool CanLog(LogEntryTypeLevel level) => level >= _logLevel;

        #region ILog implementation

        /// <summary>
        /// Имя
        /// </summary>
        public override string SourceName
        {
            get { return _backend.SourceName; }
        }

        /// <summary>
        /// Параметры
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SetParam(string name, string value)
        {
            _backend.SetParam(name, value);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level)
        {
            if (CanLog(level))
                _backend.Write(ASource, AMessage, exc, level);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1)
        {
            if (CanLog(level))
                _backend.Write(ASource, AMessage, exc, level, arg1);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1, object arg2)
        {
            if (CanLog(level))
                _backend.Write(ASource, AMessage, exc, level, arg1, arg2);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, params object[] parameters)
        {
            if (CanLog(level))
                _backend.Write(ASource, AMessage, exc, level, parameters);
        }

        #endregion
    }
}