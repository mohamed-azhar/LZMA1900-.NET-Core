using System;

namespace Lzma1900DotNetCore
{
    public class CBenchRandomGenerator
    {
		CBitRandomGenerator RG = new CBitRandomGenerator();
		UInt32 Pos;
		UInt32 Rep0;

		public UInt32 BufferSize;
		public Byte[] Buffer = null;

		public CBenchRandomGenerator() { }

		public void Set(UInt32 bufferSize)
		{
			Buffer = new Byte[bufferSize];
			Pos = 0;
			BufferSize = bufferSize;
		}
		UInt32 GetRndBit() { return RG.GetRnd(1); }
		UInt32 GetLogRandBits(int numBits)
		{
			UInt32 len = RG.GetRnd(numBits);
			return RG.GetRnd((int)len);
		}
		UInt32 GetOffset()
		{
			if (GetRndBit() == 0)
				return GetLogRandBits(4);
			return (GetLogRandBits(4) << 10) | RG.GetRnd(10);
		}
		UInt32 GetLen1() { return RG.GetRnd(1 + (int)RG.GetRnd(2)); }
		UInt32 GetLen2() { return RG.GetRnd(2 + (int)RG.GetRnd(2)); }
		public void Generate()
		{
			RG.Init();
			Rep0 = 1;
			while (Pos < BufferSize)
			{
				if (GetRndBit() == 0 || Pos < 1)
					Buffer[Pos++] = (Byte)RG.GetRnd(8);
				else
				{
					UInt32 len;
					if (RG.GetRnd(3) == 0)
						len = 1 + GetLen1();
					else
					{
						do
							Rep0 = GetOffset();
						while (Rep0 >= Pos);
						Rep0++;
						len = 2 + GetLen2();
					}
					for (UInt32 i = 0; i < len && Pos < BufferSize; i++, Pos++)
						Buffer[Pos] = Buffer[Pos - Rep0];
				}
			}
		}
	}
}
