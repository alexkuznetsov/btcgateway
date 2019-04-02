﻿using System.Linq;

namespace BTCGatewayAPI
{
    public class ApiContainerProfile : Infrastructure.Container.ContainerProfile
    {
        public ApiContainerProfile()
        {
            var assembly = GetType().Assembly;
            var apiBaseController = typeof(System.Web.Http.ApiController);
            var types = assembly.GetTypes()
                .Where(x => apiBaseController.IsAssignableFrom(x));

            foreach (var t in types)
            {
                Transient(t);
            }
        }
    }
}