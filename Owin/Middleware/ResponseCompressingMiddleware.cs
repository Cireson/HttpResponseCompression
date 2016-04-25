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
                default:
                    await Next.Invoke(context);
                    break;
            }
        }
    }
}
