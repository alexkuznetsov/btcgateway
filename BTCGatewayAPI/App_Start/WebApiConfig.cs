﻿using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Infrastructure.Container;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;

namespace BTCGatewayAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, ObjectFactory objectFactory)
        {
            // Web API configuration and services
            config.Filters.Add(new ValidateModelAttribute());

            config.Services.Replace(typeof(IHttpControllerActivator), new Infrastructure.ServiceActivator(config, objectFactory));
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

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
