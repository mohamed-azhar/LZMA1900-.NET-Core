using Lzma1900DotNetCore.Compress.Interfaces;
using System;

namespace Lzma1900DotNetCore
{
    public class CProgressInfo : ICodeProgress
	{
		public Int64 ApprovedStart;
		public Int64 InSize;
		public DateTime Time;
		public void Init() { InSize = 0; }
		public void SetProgress(Int64 inSize, Int64 outSize)
		{
			if (inSize >= ApprovedStart && InSize == 0)
			{
				Time = DateTime.UtcNow;
				InSize = inSize;
			}
		}
	}
}
