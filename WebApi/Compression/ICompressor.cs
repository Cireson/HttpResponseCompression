using System.IO;
using System.Threading.Tasks;
using Cireson.HttpResponseCompression.Common;

namespace Cireson.HttpResponseCompression.WebApi.Compression
{
    public interface ICompressor
    {
        HttpCompression EncodingType { get; }
        Task Compress(Stream source, Stream destination);
        Task Decompress(Stream source, Stream destination);
    }
}
