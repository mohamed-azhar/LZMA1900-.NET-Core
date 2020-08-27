using Lzma1900DotNetCore.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lzma1900DotNetCore
{
    public class CrcOutStream : Stream
    {
		public CRC CRC = new CRC();
		public void Init() { CRC.Init(); }
		public UInt32 GetDigest() { return CRC.GetDigest(); }

		public override bool CanRead { get { return false; } }
		public override bool CanSeek { get { return false; } }
		public override bool CanWrite { get { return true; } }
		public override Int64 Length { get { return 0; } }
		public override Int64 Position { get { return 0; } set { } }
		public override void Flush() { }
		public override long Seek(long offset, SeekOrigin origin) { return 0; }
		public override void SetLength(long value) { }
		public override int Read(byte[] buffer, int offset, int count) { return 0; }

		public override void WriteByte(byte b)
		{
			CRC.UpdateByte(b);
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			CRC.Update(buffer, (uint)offset, (uint)count);
		}
	}
}
