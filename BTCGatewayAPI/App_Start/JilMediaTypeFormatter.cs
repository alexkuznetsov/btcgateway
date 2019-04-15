using Jil;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BTCGatewayAPI
{
    public class JilMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly MediaTypeHeaderValue _applicationJsonMediaType = new MediaTypeHeaderValue("application/json");
        private static readonly MediaTypeHeaderValue _textJsonMediaType = new MediaTypeHeaderValue("text/json");

        private readonly Options _jilOptions;

        public JilMediaTypeFormatter(Options options)
        {
            _jilOptions = options;

            SupportedMediaTypes.Add(_applicationJsonMediaType);
            SupportedMediaTypes.Add(_textJsonMediaType);

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        public JilMediaTypeFormatter() : this(GetDefaultOptions())
        {
        }

        private static Options GetDefaultOptions()
        {
            return new Options();
        }

        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(DeserializeFromStream(type, readStream));
        }

        private object DeserializeFromStream(Type type, Stream readStream)
        {
            var reader = new StreamReader(readStream);
            var deserialize = TypedDeserializers.GetTyped(type);
            return deserialize(reader, _jilOptions);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value, streamWriter, _jilOptions);
            streamWriter.Flush();
            return Task.FromResult(true);
        }
    }
}