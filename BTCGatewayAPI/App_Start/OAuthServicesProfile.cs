using BTCGatewayAPI.Infrastructure.Container;

namespace BTCGatewayAPI
{
    public class OAuthServicesProfile : ContainerProfile
    {
        public OAuthServicesProfile()
        {
            Singleton(typeof(WebApiOAuthAuthorizationServerProvider), (r) =>
            {
                var service = r.GetService<Services.CheckClientService>();
                return new WebApiOAuthAuthorizationServerProvider(service);
            });
        }
    }
}