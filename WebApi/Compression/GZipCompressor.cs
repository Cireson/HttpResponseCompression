using System.IO;
using System.IO.Compression;
using Cireson.HttpResponseCompression.Common;

namespace Cireson.HttpResponseCompression.WebApi.Compression
{
    public class GZipCompressor : Compressor
    {
        public override HttpCompression EncodingType => HttpCompression.GZip;

        public override Stream CreateCompressionStream(Stream output)
        {
            return new GZipStream(output, CompressionMode.Compress, true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new GZipStream(input, CompressionMode.Decompress, true);
        }
    }
}
