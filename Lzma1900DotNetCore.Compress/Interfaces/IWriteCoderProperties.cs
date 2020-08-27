using System.IO;

namespace Lzma1900DotNetCore.Compress.Interfaces
{
    public interface IWriteCoderProperties
    {
        void WriteCoderProperties(Stream outStream);
    }
}
