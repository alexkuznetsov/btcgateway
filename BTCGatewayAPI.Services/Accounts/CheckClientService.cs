using BTCGatewayAPI.Services.Extensions;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Accounts
{
    public sealed class CheckClientService : BaseService
    {
        private readonly IPasswordHasher _hasher;

        public CheckClientService(DbConnection dBContext, IPasswordHasher hasher) : base(dBContext)
        {
            _hasher = hasher;
        }

        public Task<Models.Client> GetClientByName(string username)
            => DbCon.FindClientByUserNameAsync(username);

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await GetClientByName(username);

            return Authenticate(user, password);
        }

        public bool Authenticate(Models.Client user, string password)
        {
            if (user == null)
            {
                throw new System.ArgumentNullException(nameof(user));
            }

            return _hasher.VerifyHashedPassword(user.Passwhash, password);
        }
    }
}
