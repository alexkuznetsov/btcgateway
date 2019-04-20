using BTCGatewayAPI.Services;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BTCGatewayAPI
{
    public class WebApiOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly CheckClientService checkClientService;
        private static readonly string InvalidGrant = "invalid_grant";

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

            if (string.IsNullOrEmpty(context.UserName))
            {
                context.SetError(InvalidGrant, Resources.Messages.InvalidGrantUserNameIsEmpty);
                return;
            }

            if (string.IsNullOrEmpty(context.Password))
            {
                context.SetError(InvalidGrant, Resources.Messages.InvalidGrantPasswordIsEmpty);
                return;
            }

            var res = await checkClientService.AuthenticateAsync(context.UserName, context.Password);

            if (!res)
            {
                context.SetError(InvalidGrant, Resources.Messages.InvalidGrantUserNameOrPasswordNotFound);
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            context.Validated(identity);
        }
    }
}