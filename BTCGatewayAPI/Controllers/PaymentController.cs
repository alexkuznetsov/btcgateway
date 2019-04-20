using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    public class PaymentController : ServicedApiController<PaymentService>
    {
        private Infrastructure.GlobalConf Conf { get; }

        public PaymentController(PaymentService paymentService, Infrastructure.GlobalConf conf)
            : base(paymentService)
        {
            Conf = conf;
        }

        [HttpPost]
        [Route("SendBtc")]
        public async Task<IHttpActionResult> SendBtc(SendBtcRequest sendBtcRequest)
        {
            if (!sendBtcRequest.IsValid(Conf.IsTestNet))
            {
                throw new ValidationException(Resources.Messages.InvalidRecieverAddress);
            }
            await Service.SendAsync(sendBtcRequest);

            return Ok(new { status = true });
        }
    }
}
