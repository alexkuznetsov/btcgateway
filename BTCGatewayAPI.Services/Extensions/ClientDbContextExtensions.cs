using BTCGatewayAPI.Models;
using Dapper;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class ClientDbContextExtensions
    {
        private const string FindClientByNameSQL = @"select id Id
  , created_at CreatedAt
  , updated_at UpdatedAt
  , client_id ClientId
  , passwhash Passwhash from clients where client_id=@username";

        public static Task<Client> FindClientByUserNameAsync(this DbConnection dBContext, string username)
            => dBContext.QueryFirstOrDefaultAsync<Client>(FindClientByNameSQL, new { username });
    }
}