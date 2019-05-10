using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTCGatewayAPI.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class DefaultApiController : Controller
    {
    }
}
