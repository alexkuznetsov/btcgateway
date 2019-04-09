using BTCGatewayAPI.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    [Authorize]
    public class TransactionsController : ApiController
    {
        private readonly TransactionsService _transactionsService;

        public TransactionsController(TransactionsService transactionsService)
        {
            this._transactionsService = transactionsService;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _transactionsService.Dispose();
            }
        }

        [HttpGet]
        [Route("GetLast")]
        public async Task<IHttpActionResult> GetLast()
        {
            var last = await _transactionsService.GetLastTransactionsAsync();

            return Ok(last);
        }
    }
}
