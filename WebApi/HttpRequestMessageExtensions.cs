using System.Linq;
using System.Net.Http;
using Cireson.HttpResponseCompression.Common;

namespace Cireson.HttpResponseCompression.WebApi
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpCompression DetectedCompressionSupport(this HttpRequestMessage request)
        {
            var responseCompressionSupport = HttpCompression.None;
            if (request.Headers.AcceptEncoding.Any(h => h.Value.Contains(HttpHeaders.GzipContentEncodingValue)))
            {
                responseCompressionSupport = HttpCompression.GZip;
            }
            else if (request.Headers.AcceptEncoding.Any(h => h.Value.Contains(HttpHeaders.DeflateContentEncodingValue)))
            {
                responseCompressionSupport = HttpCompression.Deflate;
            }

            return responseCompressionSupport;
        }
    }
}
