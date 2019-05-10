using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BTCGatewayAPI.Common.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static string IdValue(this ClaimsPrincipal principal)
        {
            return principal.ClaimValue(ClaimTypes.NameIdentifier);
        }

        public static string ClaimValue(this ClaimsPrincipal principal, string claimType)
        {
            return principal.StringClaimValue(claimType);
        }

        internal static string StringClaimValue(this ClaimsPrincipal principal, string claimType)
        {
            var claim = principal.FindFirst(claimType);
            if (claim != null)
            {
                return claim.Value;
            }

            return null;
        }
    }
}
