using System.IO;
using System.Threading.Tasks;
using Cireson.HttpResponseCompression.Common;

namespace Cireson.HttpResponseCompression.WebApi.Compression
{
    public abstract class Compressor : ICompressor
    {
        public abstract HttpCompression EncodingType { get; }
        public abstract Stream CreateCompressionStream(Stream output);
        public abstract Stream CreateDecompressionStream(Stream input);

        public virtual Task Compress(Stream source, Stream destination)
        {
            var compressed = CreateCompressionStream(destination);

            return Pump(source, compressed)
                .ContinueWith(task => compressed.Dispose());
        }

        public virtual Task Decompress(Stream source, Stream destination)
        {
            var decompressed = CreateDecompressionStream(source);

            return Pump(decompressed, destination)
                .ContinueWith(task => decompressed.Dispose());
        }

        protected virtual Task Pump(Stream input, Stream output)
        {
            return input.CopyToAsync(output);
        }
    }
}
