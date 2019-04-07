using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin
{
    [Serializable]
    public class RPCServerException : InvalidOperationException
    {
        public RPCServerException()
        {
        }

        public RPCServerException(string message) : base(message)
        {
        }

        public RPCServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RPCServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
