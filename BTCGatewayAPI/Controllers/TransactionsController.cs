using BTCGatewayAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Controllers
{
    [Route("GetLast")]
    public class TransactionsController : ServicedApiController<TransactionsService>
    {
        public TransactionsController(TransactionsService transactionsService) :
            base(transactionsService)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var last = await Service.GetLastTransactionsAsync();

            return Ok(last);
        }
    }
}
