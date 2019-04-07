using System;
using System.Text;

namespace BTCGatewayAPI.Infrastructure.Logging.Impl
{
    class TraceLoggingBackend : ILoggingBackend
    {
        private readonly string _sourceMethodName;

        public TraceLoggingBackend(string sourceName, string sourceMethod)
        {
            SourceName = sourceName;
            _sourceMethodName = sourceMethod;
        }

        public string SourceName { get; }

        public void SetParam(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level)
        {
            WriteA(ASource, AMessage, exc, level, null);
        }

        public void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1)
        {
            WriteA(ASource, AMessage, exc, level, new[] { arg1 });
        }

        public void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object arg1, object arg2)
        {
            WriteA(ASource, AMessage, exc, level, new[] { arg1, arg2 });
        }

        public void Write(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, params object[] parameters)
        {
            WriteA(ASource, AMessage, exc, level, parameters);
        }

        private void WriteA(string ASource, string AMessage, Exception exc, LogEntryTypeLevel level, object[] parameters)
        {
            System.Diagnostics.Trace.WriteLine(FormatMessage(ASource, AMessage, exc, parameters), level.ToString());
        }

        private StringBuilder FormatMessage(string aSource, string aMessage, Exception exception, object[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now);
            sb.Append("\t");
            sb.Append(aSource);
            sb.Append("\t");

            if (parameters != null)
            {
                sb.AppendFormat(aMessage, parameters);
            }
            else
            {
                sb.Append(aMessage);
            }

            if (exception != null)
            {
                sb.AppendLine();
                sb.Append("\t");
                sb.Append(exception.ToString());
            }

            return sb;
        }
    }
}
