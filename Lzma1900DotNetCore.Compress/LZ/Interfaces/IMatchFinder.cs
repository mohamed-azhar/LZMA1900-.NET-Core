using System;

namespace Lzma1900DotNetCore.Compress.LZ.Interfaces
{
    public interface IMatchFinder : IInWindowStream
    {
        void Skip(UInt32 num);
        UInt32 GetMatches(UInt32[] distances);
        void Create(UInt32 historySize, UInt32 keepAddBufferBefore, UInt32 matchMaxLen, UInt32 keepAddBufferAfter);
    }
}
