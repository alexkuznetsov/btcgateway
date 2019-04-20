using BTCGatewayAPI.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    public class TransactionsController : ServicedApiController<TransactionsService>
    {
        public TransactionsController(TransactionsService transactionsService) : base(transactionsService)
        {
        }

        [HttpGet]
        [Route("GetLast")]
        public async Task<IHttpActionResult> GetLast()
        {
            var last = await Service.GetLastTransactionsAsync();

            return Ok(last);
        }
    }
}
