using System;

namespace BTCGatewayAPI.Infrastructure.Logging
{
    /// <summary>
    /// Логгер
    /// </summary>
    public sealed class Logger : AbstractLogger
    {
        #region privates

        private readonly ILoggingBackend _backend;

        #endregion

        public Logger(ILoggingBackend backend)
        {
            _backend = backend;
        }

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
            _backend.Write(ASource, AMessage, exc, level);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1)
        {
            _backend.Write(ASource, AMessage, exc, level, arg1);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1, object arg2)
        {
            _backend.Write(ASource, AMessage, exc, level, arg1, arg2);
        }

        public override void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, params object[] parameters)
        {
            _backend.Write(ASource, AMessage, exc, level, parameters);
        }

        #endregion
    }
}