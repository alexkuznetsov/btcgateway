using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Infrastructure.Container;
using BTCGatewayAPI.Infrastructure.DB;
using System.Configuration;
using System.Data.Common;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace BTCGatewayAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, ObjectFactory objectFactory)
        {
            // Web API configuration and services
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new GlobalExceptionFilterAttribute());

            config.Services.Replace(typeof(IHttpControllerActivator), new Infrastructure.ServiceActivator(config, objectFactory));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


    }
}
