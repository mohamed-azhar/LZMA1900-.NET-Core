using System;
using System.IO;

namespace Lzma1900DotNetCore.Compress.LZ.Interfaces
{
    public interface IInWindowStream
    {
		void Init();
		void ReleaseStream();
		UInt32 GetNumAvailableBytes();
		byte GetIndexByte(Int32 index);
		void SetStream(Stream inStream);
		UInt32 GetMatchLen(Int32 index, UInt32 distance, UInt32 limit);
	}
}
