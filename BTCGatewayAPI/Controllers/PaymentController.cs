using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    public class PaymentController : ApiController
    {
        private readonly PaymentService paymentService;

        public PaymentController(PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        [Route("SendBtc")]
        public async Task<IHttpActionResult> SendBtc(SendBtcRequest sendBtcRequest)
        {
            await paymentService.SendAsync(sendBtcRequest);

            return Ok(new { status = true });
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                paymentService.Dispose();
            }
        }
    }
}
