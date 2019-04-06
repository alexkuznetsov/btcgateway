using System;

namespace BTCGatewayAPI.Infrastructure.DB
{
    [Serializable]
    public class ObjectMapperExeption : Exception
    {
        public ObjectMapperExeption(string message) : base(message)
        {
        }
    }
}