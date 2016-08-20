using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Cireson.HttpResponseCompression.Common;
using Microsoft.Owin;

namespace Cireson.HttpResponseCompression.Owin.Middleware
{
    public class ResponseCompressingMiddleware : OwinMiddleware
    {
        private const string Compressed = "Compresssed";

        public ResponseCompressingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var requestCompressionSupport = context.Request.DetectedCompressionSupport();


            if (requestCompressionSupport != HttpCompression.None)
            {
                context.Response.OnSendingHeaders(state =>
                {
                    var response = (IOwinResponse) state;
                    object value;
                    var hasValue = response.Environment.TryGetValue(Compressed, out value);
                    bool compressed = false;
                    if (hasValue)
                    {
                        bool.TryParse(value.ToString(), out compressed);
                    }

                    if (!compressed)
                    {
                        return;
                    }

                    switch (requestCompressionSupport)
                    {
                        case HttpCompression.GZip:
                            response.Headers.Append(HttpHeaders.ContentEncodingHttpHeaderKey,
                                HttpHeaders.GzipContentEncodingValue);
                            break;
                        case HttpCompression.Deflate:
                            response.Headers.Append(HttpHeaders.ContentEncodingHttpHeaderKey,
                                HttpHeaders.DeflateContentEncodingValue);
                            break;
                    }
                },
                    context.Response);

                var originalBody = context.Response.Body;

                using (var nextStream = new MemoryStream())
                {
                    context.Response.Body = nextStream;
                    await Next.Invoke(context);
                    
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    using (var tempStream = new MemoryStream())
                    {
                        if (context.Response.ContentLength > 4096)
                        {
                            Stream compressionStream = new MemoryStream();
                            context.Response.Environment.Add(Compressed, true);
                            switch (requestCompressionSupport)
                            {
                                case HttpCompression.GZip:
                                    compressionStream = new GZipStream(tempStream, CompressionLevel.Fastest, true);
                                    break;
                                case HttpCompression.Deflate:
                                    compressionStream = new DeflateStream(tempStream, CompressionLevel.Fastest, true);
                                    break;
                            }
                            using (compressionStream)
                            {
                                context.Response.Body.CopyTo(compressionStream);
                            }
                        }
                        {
                            context.Response.Body.CopyTo(tempStream);
                        }
                        
                        tempStream.Seek(0, SeekOrigin.Begin);
                        context.Response.ContentLength = tempStream.Length;
                        tempStream.CopyTo(originalBody);
                    }
                }

                context.Response.Body = originalBody;
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}
