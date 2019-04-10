using System.ComponentModel.DataAnnotations;

namespace BTCGatewayAPI.Models.Requests
{
    public class SendBtcRequest
    {
        const string BitcoinValidationRegex = "^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$";
        const string BitcoinTestNetValidationRegex = "^[a-km-zA-HJ-NP-Z1-9]{25,34}$";

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_AccountCanNotBeEmpty), ErrorMessageResourceType = typeof(Messages))]
        [MinLength(25)]
        [MaxLength(35)]
        public string Account { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_AmountCanNotBeEmpty), ErrorMessageResourceType = typeof(Messages))]
        [Range(0.00001D, double.MaxValue, ErrorMessageResourceName = nameof(Messages.SendBtcRequest_MoneyFormatShouldBeValid), ErrorMessageResourceType = typeof(Messages))]
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{Account} => {Amount}";
        }

        public bool IsValid(bool testNet)
        {
            var pattern = testNet ? BitcoinTestNetValidationRegex : BitcoinValidationRegex;
            return System.Text.RegularExpressions.Regex.IsMatch(Account, pattern);
        }
    }
}