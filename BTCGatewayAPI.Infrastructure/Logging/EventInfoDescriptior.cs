using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BTCGatewayAPI.Infrastructure.Logging
{
    /// <summary>
    /// Класс информации по событию, которое логируется.
    /// </summary>
    public class EventInfoDescriptior
    {
        IDictionary Context { get; set; }

        public Exception Exception { get; set; }

        public IFormatProvider FormatProvider { get; set; }

        public string FormattedMessage { get; set; }

        public bool HasStackTrace { get; set; }

        public LogEntryTypeLevel Level { get; set; }

        public string LoggerName { get; set; }

        public string Message { get; set; }

        public IEnumerable<object> Parameters { get; private set; }

        public IDictionary<object, object> Properties { get; private set; }

        public int SequenceID { get; set; }

        public StackTrace StackTrace { get; set; }

        public DateTime TimeStamp { get; set; }

        public StackFrame UserStackFrame { get; set; }

        public int UserStackFrameNumber { get; set; }

        /// <summary>
        /// Модуль
        /// </summary>
        public string ModuleType
        {
            get
            {
                if (Properties != null && Properties.ContainsKey("sourceName"))
                {
                    return Properties["sourceName"].ToString();
                }

                return null;

            }
        }

        /// <summary>
        /// Метод модуля
        /// </summary>
        public string ModuleMethod
        {
            get
            {
                if (Properties != null && Properties.ContainsKey("sourceMethod"))
                {
                    return Properties["sourceMethod"].ToString();
                }

                return null;
            }
        }

        public EventInfoDescriptior(IDictionary<object, object> props, object[] parameters)
        {
            Properties = props;
            Parameters = parameters;
        }
    }
}
