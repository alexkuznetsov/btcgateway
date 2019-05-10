using BTCGatewayAPI.Models.Requests;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Payments
{
    public interface IPaymentService
    {
        Task SendAsync(SendBtcRequest sendBtcRequest);
    }
}