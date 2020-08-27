using System;

namespace Lzma1900DotNetCore
{
    public class CBitRandomGenerator
    {
		CRandomGenerator RG = new CRandomGenerator();
		UInt32 Value;
		int NumBits;
		public void Init()
		{
			Value = 0;
			NumBits = 0;
		}
		public UInt32 GetRnd(int numBits)
		{
			UInt32 result;
			if (NumBits > numBits)
			{
				result = Value & (((UInt32)1 << numBits) - 1);
				Value >>= numBits;
				NumBits -= numBits;
				return result;
			}
			numBits -= NumBits;
			result = (Value << numBits);
			Value = RG.GetRnd();
			result |= Value & (((UInt32)1 << numBits) - 1);
			Value >>= numBits;
			NumBits = 32 - numBits;
			return result;
		}

	}
}
