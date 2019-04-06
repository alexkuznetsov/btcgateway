using System.Collections.Generic;
using System.Text;

namespace BTCGatewayAPI.Infrastructure.DB
{
    public interface IQueryBuilder
    {
        (StringBuilder, List<KeyValuePair<string, object>>) BuildInsertStatement(string id, object model);
        (StringBuilder, List<KeyValuePair<string, object>>) BuildUpdateStatement(string id, object model);
        (StringBuilder, List<KeyValuePair<string, object>>) BuildDeleteStatement(string id, object model, object value);
    }
}