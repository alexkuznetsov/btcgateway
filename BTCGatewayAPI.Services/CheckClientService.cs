using BTCGatewayAPI.Services.Extensions;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class CheckClientService : BaseService
    {
        private readonly IPasswordHasher _hasher;

        public CheckClientService(DbConnection dBContext, IPasswordHasher hasher) : base(dBContext)
        {
            _hasher = hasher;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await DbCon.FindClientByUserNameAsync(username);

            if (user != null)
            {
                return _hasher.VerifyHashedPassword(user.Passwhash, password);
            }

            return false;
        }
    }
}
