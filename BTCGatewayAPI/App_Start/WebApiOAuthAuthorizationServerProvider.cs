using BTCGatewayAPI.Services;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BTCGatewayAPI
{
    public class WebApiOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly CheckClientService checkClientService;

        public WebApiOAuthAuthorizationServerProvider(CheckClientService checkClientService)
        {
            this.checkClientService = checkClientService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();

            return Task.FromResult(0);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var res = await checkClientService.AuthenticateAsync(context.UserName, context.Password);

            if (!res)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            context.Validated(identity);
        }
    }
}