using BTCGatewayAPI.Infrastructure.DB;

namespace BTCGatewayAPI.Models.Mapping
{
    public sealed class IncomeTransactionIdMappingSpec: DbReaderMappingSpec<IncomeTransactionId>
    {
        protected override void MapProperties()
        {
            Map(x => x.Id, Transform.GetInt);
        }
    }
}