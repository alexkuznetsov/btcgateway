namespace BTCGatewayAPI.Services.Payments.FundStrategy
{
    public struct FundTransactionStrategyResult
    {
        public FundTransactionStrategyResult(string hex, decimal fee, string txid)
        {
            Hex = hex;
            Fee = fee;
            Txid = txid;
        }

        public string Hex { get; }
        public string Txid { get; }
        public decimal Fee { get; }
    }
}