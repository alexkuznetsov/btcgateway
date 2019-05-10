using BTCGatewayAPI.Common;
using BTCGatewayAPI.Models.Requests;
using BTCGatewayAPI.Services.Payments;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Controllers
{

    [Route("SendBtc")]
    public class PaymentController : ServicedApiController<IPaymentService>
    {
        private readonly GlobalConf conf;

        public PaymentController(IPaymentService paymentService, GlobalConf conf) : base(paymentService)
        {
            this.conf = conf;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SendBtcRequest sendBtcRequest)
        {
            if (!sendBtcRequest.IsValid(conf.IsTestNet))
            {
                throw new ValidationException("InvalidRecieverAddress");
            }

            await Service.SendAsync(sendBtcRequest);

            return Ok(new { status = true });
        }
    }
}
