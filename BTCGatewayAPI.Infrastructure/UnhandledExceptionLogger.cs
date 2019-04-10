using System;
using System.Text;
using System.Web.Http.ExceptionHandling;

namespace BTCGatewayAPI.Infrastructure
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static readonly Lazy<Logging.ILogger> loggerLazy =
            new Lazy<Logging.ILogger>(Logging.LoggerFactory.GetLogger);

        private static Logging.ILogger Logger => loggerLazy.Value;

        public override void Log(ExceptionLoggerContext context)
        {
            var buildr = new StringBuilder();

            FillBuilder(buildr, context.Exception);

            var requestedURi = context.Request.RequestUri.AbsoluteUri;
            var requestMethod = context.Request.Method.ToString();

            Logger.ErrorSource(context.Exception.Source,
                Messages.UnhandledMessageStringTpl,
                new object[] { requestedURi, requestMethod, buildr });
        }

        private void FillBuilder(StringBuilder buildr, Exception ex)
        {
            buildr.AppendLine(Messages.LoggerSourceMsg + ex.Source);
            buildr.AppendLine(Messages.LoggerStackTrcMsg + ex.StackTrace);
            buildr.AppendLine(Messages.LoggerTargetSiteMsg + ex.TargetSite);

            if (ex.HelpLink != null)
            {
                buildr.AppendLine(Messages.LoggerHelpLnkMsg + ex.HelpLink);//error prone
            }

            if (ex.InnerException != null)
            {
                buildr.AppendLine(Messages.LoggerInnerExcMsg);
                FillBuilder(buildr, ex.InnerException);
            }
        }
    }
}
