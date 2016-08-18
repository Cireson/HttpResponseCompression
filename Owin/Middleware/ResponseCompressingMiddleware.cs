using System.IO.Compression;
using System.Threading.Tasks;
using Cireson.HttpResponseCompression.Common;
using Microsoft.Owin;

namespace Cireson.HttpResponseCompression.Owin.Middleware
{
    public class ResponseCompressingMiddleware : OwinMiddleware
    {
        public ResponseCompressingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Response.Body.Length >= 4096)
            {
                var requestCompressionSupport = context.Request.DetectedCompressionSupport();
                switch (requestCompressionSupport)
                {
                    case HttpCompression.GZip:
                        context.Response.Body = new GZipStream(context.Response.Body, CompressionLevel.Fastest);
                        context.Response.Headers.Append(HttpHeaders.ContentEncodingHttpHeaderKey,
                            HttpHeaders.GzipContentEncodingValue);
                        break;
                    case HttpCompression.Deflate:
                        context.Response.Body = new DeflateStream(context.Response.Body, CompressionLevel.Fastest);
                        context.Response.Headers.Append(HttpHeaders.ContentEncodingHttpHeaderKey,
                            HttpHeaders.DeflateContentEncodingValue);
                        break;
                }
            }

            await Next.Invoke(context);
        }
    }
}
