using System.IO;
using System.IO.Compression;
using Cireson.HttpResponseCompression.Common;

namespace Cireson.HttpResponseCompression.WebApi.Compression
{
    public class DeflateCompressor : Compressor
    {
        public override HttpCompression EncodingType => HttpCompression.Deflate;

        public override Stream CreateCompressionStream(Stream output)
        {
            return new DeflateStream(output, CompressionMode.Compress, true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new DeflateStream(input, CompressionMode.Decompress, true);
        }
    }
}
