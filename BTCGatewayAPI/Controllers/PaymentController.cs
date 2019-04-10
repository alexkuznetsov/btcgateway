using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    [Authorize]
    public class PaymentController : ApiController
    {
        private readonly PaymentService paymentService;
        private readonly Infrastructure.GlobalConf conf;

        public PaymentController(PaymentService paymentService, Infrastructure.GlobalConf conf)
        {
            this.paymentService = paymentService;
            this.conf = conf;
        }

        [HttpPost]
        [Route("SendBtc")]
        public async Task<IHttpActionResult> SendBtc(SendBtcRequest sendBtcRequest)
        {
            if (!sendBtcRequest.IsValid(conf.IsTestNet))
            {
                throw new ValidationException(Resources.Messages.InvalidRecieverAddress);
            }
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
