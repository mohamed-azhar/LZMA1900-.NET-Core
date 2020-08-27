using System;

namespace Lzma1900DotNetCore
{
    public class CRandomGenerator
    {
		UInt32 A1;
		UInt32 A2;
		public CRandomGenerator() { Init(); }
		public void Init() { A1 = 362436069; A2 = 521288629; }
		public UInt32 GetRnd()
		{
			return
				((A1 = 36969 * (A1 & 0xffff) + (A1 >> 16)) << 16) ^
				((A2 = 18000 * (A2 & 0xffff) + (A2 >> 16)));
		}
	}
}
