using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BTCGatewayAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IncomeTxUpdator updator;

        protected void Application_Start()
        {
            var factory = ObjectRegistryConfig.Configure();
            GlobalConfiguration.Configure((c) =>
            {
                WebApiConfig.Register(c, factory);
            });
            updator = new IncomeTxUpdator(factory);
        }

        protected void Application_End()
        {
            updator.Stop(true);
            updator.Dispose();
            ObjectRegistryConfig.Shutdown();
        }

    }
}
