using Cireson.HttpResponseCompression.Common;
using Microsoft.Owin;

namespace Cireson.HttpResponseCompression.Owin
{
    public static class OwinRequestExtensions
    {
        public static HttpCompression DetectedCompressionSupport(this IOwinRequest request)
        {
            string acceptEncoding = request.Headers[HttpHeaders.AcceptEncodingHttpHeaderKey];
            var requestCompressionSupport = HttpCompression.None;
            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                if (acceptEncoding.Contains(HttpHeaders.GzipContentEncodingValue))
                {
                    requestCompressionSupport = HttpCompression.GZip;
                }
                else if (acceptEncoding.Contains(HttpHeaders.DeflateContentEncodingValue))
                {
                    requestCompressionSupport = HttpCompression.Deflate;
                }
            }
            return requestCompressionSupport;
        }
    }
}
