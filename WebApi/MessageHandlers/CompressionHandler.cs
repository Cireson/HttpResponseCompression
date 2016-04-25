using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cireson.HttpResponseCompression.Common;
using Cireson.HttpResponseCompression.WebApi.Compression;

namespace Cireson.HttpResponseCompression.WebApi.MessageHandlers
{
    public class CompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors { get; }

        public CompressionHandler()
        {
            Compressors = new Collection<ICompressor> {new GZipCompressor(), new DeflateCompressor()};

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (request.DetectedCompressionSupport() != HttpCompression.None && response.Content != null)
            {
                // As per RFC2616.14.3:
                // Ignores encodings with quality == 0
                // If multiple content-codings are acceptable, then the acceptable content-coding with the highest non-zero qvalue is preferred.
                var compressor = (from encoding in request.Headers.AcceptEncoding
                    let quality = encoding.Quality ?? 1.0
                    where quality > 0
                    join c in Compressors on encoding.Value.ToLowerInvariant() equals
                        c.EncodingType.ToString().ToLowerInvariant()
                    orderby quality descending
                    select c).FirstOrDefault();

                if (compressor != null)
                {
                    response.Content = new CompressedContent(response.Content, compressor);
                }
            }

            return response;
        }
    }
}
