using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTCGatewayAPI.Services.Extensions;

namespace BTCGatewayAPI.Services
{
    public class HotWalletsService : BaseService
    {
        public HotWalletsService(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<HotWallet>> GetAllWalletsAsync()
        {
            return await DBContext.GetAllHotWallets();
        }
    }
}