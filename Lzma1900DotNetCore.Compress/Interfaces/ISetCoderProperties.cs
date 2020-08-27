using Lzma1900DotNetCore.Compress.Enums;

namespace Lzma1900DotNetCore.Compress.Interfaces
{
    public interface ISetCoderProperties
    {
        void SetCoderProperties(CoderPropID[] propIDs, object[] properties);
    }
}
