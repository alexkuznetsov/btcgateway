using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Services.Extensions;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class CheckClientService : BaseService
    {
        private readonly IPasswordHasher hasher;

        public CheckClientService(DBContext dBContext, IPasswordHasher hasher) : base(dBContext)
        {
            this.hasher = hasher;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await DBContext.FindClientByUserNameAsync(username);

            if (user != null)
            {
                return hasher.VerifyHashedPassword(user.Passwhash, password);
            }

            return false;
        }
    }
}
