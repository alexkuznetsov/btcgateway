using System;
using System.Text;
using System.Web.Http.ExceptionHandling;

namespace BTCGatewayAPI.Infrastructure
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static readonly Lazy<Logging.ILogger> loggerLazy = new Lazy<Logging.ILogger>(Logging.LoggerFactory.GetLogger);

        private static Logging.ILogger Logger => loggerLazy.Value;

        public override void Log(ExceptionLoggerContext context)
        {
            var buildr = new StringBuilder();

            FillBuilder(buildr, context.Exception);

            var requestedURi = context.Request.RequestUri.AbsoluteUri;
            var requestMethod = context.Request.Method.ToString();

            Logger.ErrorSource(context.Exception.Source, "Unhandled exception. Uri: {0}, method: {1}. Exception: {2}", new object[] { requestedURi, requestMethod, buildr });
        }

        private void FillBuilder(StringBuilder buildr, Exception ex)
        {
            buildr.AppendLine("Source: " + ex.Source);
            buildr.AppendLine("StackTrace: " + ex.StackTrace);
            buildr.AppendLine("TargetSite: " + ex.TargetSite);

            if (ex.HelpLink != null)
            {
                buildr.AppendLine("HelpLink: " + ex.HelpLink);//error prone
            }

            if (ex.InnerException != null)
            {
                buildr.AppendLine("Inner Exception:");
                FillBuilder(buildr, ex.InnerException);
            }
        }
    }
}
