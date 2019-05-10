using BTCGatewayAPI.Models.Requests;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public interface IPaymentService
    {
        Task SendAsync(SendBtcRequest sendBtcRequest);
    }
}