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
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(new Action<HttpConfiguration>((c) =>
            {
                var factory = ObjectRegistryConfig.Configure();
                WebApiConfig.Register(c, factory);
            }));
        }

        protected void Application_End()
        {
            ObjectRegistryConfig.Shutdown();
        }

    }
}
