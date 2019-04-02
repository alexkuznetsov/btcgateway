using System.ComponentModel.DataAnnotations;

namespace BTCGatewayAPI.Models.Requests
{
    public class SendBtcRequest
    {
        const string BitcoinValidationRegex = "^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$";

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_AccountCanNotBeEmpty), ErrorMessageResourceType = typeof(Messages))]
        [RegularExpression(BitcoinValidationRegex, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_AccountFormatShouldBeValid), ErrorMessageResourceType = typeof(Messages))]
        public string Account { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_AmountCanNotBeEmpty), ErrorMessageResourceType = typeof(Messages))]
        [Range(0, double.MaxValue, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_MoneyFormatShouldBeValid), ErrorMessageResourceType = typeof(Messages))]
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"from account {Account} => amount: {Amount}";
        }
    }
}