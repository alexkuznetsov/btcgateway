using BTCGatewayAPI.Common.Authentication;
using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services;
using BTCGatewayAPI.Services.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Controllers
{
    /// <summary>
    /// Authentication entry point
    /// </summary>
    [Route("api/[controller]")]
    public class TokenController : ServicedApiController<CheckClientService>
    {
        private readonly IJwtHandler _jwtHandler;

        public TokenController(IJwtHandler jwtHandler, CheckClientService checkClientService) : base(checkClientService)
        {
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]AuthenticationRequest authenticationRequest)
        {
            var user = await Service.GetClientByName(authenticationRequest.Username);
            var isValid = Service.Authenticate(user, authenticationRequest.Password);

            if (isValid)
            {
                var token = _jwtHandler.CreateToken(user.ClientId);
                return Ok(token);
            }

            return BadRequest(new { error = "Password is invalid" });
        }
    }
}
