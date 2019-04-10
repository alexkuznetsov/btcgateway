using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class ClientDbContextExtensions
    {
        private const string FindClientByNameSQL = "select * from clients where client_id=@clientId";

        public static Task<Client> FindClientByUserNameAsync(this DBContext dBContext, string username)
            => dBContext.FindAsync<Client>(FindClientByNameSQL,
                new KeyValuePair<string, object>("clientId", username));
    }
}