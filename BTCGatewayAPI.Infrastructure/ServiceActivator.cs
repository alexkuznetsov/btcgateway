using BTCGatewayAPI.Infrastructure.Container;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace BTCGatewayAPI.Infrastructure
{
    public sealed class ServiceActivator : IHttpControllerActivator
    {
        private readonly HttpConfiguration configuration;
        private readonly ObjectFactory oFactory;

        public ServiceActivator(HttpConfiguration configuration, ObjectFactory oFactory)
        {
            this.configuration = configuration;
            this.oFactory = oFactory;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return oFactory.Create(controllerType) as IHttpController;
        }
    }
}