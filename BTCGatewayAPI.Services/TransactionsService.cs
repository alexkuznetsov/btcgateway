using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class TransactionsService : BaseService
    {
        private readonly Infrastructure.GlobalConf conf;

        public TransactionsService(Infrastructure.DB.DBContext dBContext, Infrastructure.GlobalConf conf) : base(dBContext)
        {
            this.conf = conf;
        }

        public async Task<LastTransactionDTO[]> GetLastTransactions()
        {
            return await DBContext.GetNewIncomeTransactions(conf.MinimalConfirmations).ConfigureAwait(false);
        }
    }
}
