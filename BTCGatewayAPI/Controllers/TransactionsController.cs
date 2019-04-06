using BTCGatewayAPI.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    public class TransactionsController : ApiController
    {
        private readonly TransactionsService transactionsService;

        public TransactionsController(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                transactionsService.Dispose();
            }
        }

        [HttpPost]
        [Route("GetLast")]
        public async Task<IHttpActionResult> GetLast()
        {
            var last = await transactionsService.GetLastTransactions();

            return Ok(last);
        }
    }
}
