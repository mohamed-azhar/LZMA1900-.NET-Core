using Lzma1900DotNetCore.Common;
using Lzma1900DotNetCore.Compress.Enums;
using Lzma1900DotNetCore.Compress.LZMA;
using System;
using System.IO;

namespace Lzma1900DotNetCore
{
    internal abstract class LzmaBench
    {
        const UInt32 kAdditionalSize = (6 << 20);
        const UInt32 kCompressedAdditionalSize = (1 << 10);
        const UInt32 kMaxLzmaPropSize = 10;

		const int kSubBits = 8;

		static UInt32 GetLogSize(UInt32 size)
		{
			for (int i = kSubBits; i < 32; i++)
				for (UInt32 j = 0; j < (1 << kSubBits); j++)
					if (size <= (((UInt32)1) << i) + (j << (i - kSubBits)))
						return (UInt32)(i << kSubBits) + j;
			return (32 << kSubBits);
		}

		static UInt64 MyMultDiv64(UInt64 value, UInt64 elapsedTime)
		{
			UInt64 freq = TimeSpan.TicksPerSecond;
			UInt64 elTime = elapsedTime;
			while (freq > 1000000)
			{
				freq >>= 1;
				elTime >>= 1;
			}
			if (elTime == 0)
				elTime = 1;
			return value * freq / elTime;
		}

		static UInt64 GetCompressRating(UInt32 dictionarySize, UInt64 elapsedTime, UInt64 size)
		{
			UInt64 t = GetLogSize(dictionarySize) - (18 << kSubBits);
			UInt64 numCommandsForOne = 1060 + ((t * t * 10) >> (2 * kSubBits));
			UInt64 numCommands = (UInt64)(size) * numCommandsForOne;
			return MyMultDiv64(numCommands, elapsedTime);
		}

		static UInt64 GetDecompressRating(UInt64 elapsedTime, UInt64 outSize, UInt64 inSize)
		{
			UInt64 numCommands = inSize * 220 + outSize * 20;
			return MyMultDiv64(numCommands, elapsedTime);
		}

		static UInt64 GetTotalRating(
			UInt32 dictionarySize,
			UInt64 elapsedTimeEn, UInt64 sizeEn,
			UInt64 elapsedTimeDe,
			UInt64 inSizeDe, UInt64 outSizeDe)
		{
			return (GetCompressRating(dictionarySize, elapsedTimeEn, sizeEn) +
				GetDecompressRating(elapsedTimeDe, inSizeDe, outSizeDe)) / 2;
		}

		static void PrintValue(UInt64 v)
		{
			string s = v.ToString();
			for (int i = 0; i + s.Length < 6; i++)
				Console.Write(" ");
			Console.Write(s);
		}

		static void PrintRating(UInt64 rating)
		{
			PrintValue(rating / 1000000);
			Console.Write(" MIPS");
		}

		static void PrintResults(
			UInt32 dictionarySize,
			UInt64 elapsedTime,
			UInt64 size,
			bool decompressMode, UInt64 secondSize)
		{
			UInt64 speed = MyMultDiv64(size, elapsedTime);
			PrintValue(speed / 1024);
			Console.Write(" KB/s  ");
			UInt64 rating;
			if (decompressMode)
				rating = GetDecompressRating(elapsedTime, size, secondSize);
			else
				rating = GetCompressRating(dictionarySize, elapsedTime, size);
			PrintRating(rating);
		}

		static public int LzmaBenchmark(Int32 numIterations, UInt32 dictionarySize)
		{
			if (numIterations <= 0)
				return 0;
			if (dictionarySize < (1 << 18))
			{
				Console.WriteLine("\nError: dictionary size for benchmark must be >= 19 (512 KB)");
				return 1;
			}
			Console.Write("\n       Compressing                Decompressing\n\n");

			Encoder encoder = new Encoder();
			Decoder decoder = new Decoder();


			CoderPropID[] propIDs =
			{
				CoderPropID.DictionarySize,
			};
			object[] properties =
			{
				(Int32)(dictionarySize),
			};

			UInt32 kBufferSize = dictionarySize + kAdditionalSize;
			UInt32 kCompressedBufferSize = (kBufferSize / 2) + kCompressedAdditionalSize;

			encoder.SetCoderProperties(propIDs, properties);
			System.IO.MemoryStream propStream = new System.IO.MemoryStream();
			encoder.WriteCoderProperties(propStream);
			byte[] propArray = propStream.ToArray();

			CBenchRandomGenerator rg = new CBenchRandomGenerator();

			rg.Set(kBufferSize);
			rg.Generate();
			CRC crc = new CRC();
			crc.Init();
			crc.Update(rg.Buffer, 0, rg.BufferSize);

			CProgressInfo progressInfo = new CProgressInfo();
			progressInfo.ApprovedStart = dictionarySize;

			UInt64 totalBenchSize = 0;
			UInt64 totalEncodeTime = 0;
			UInt64 totalDecodeTime = 0;
			UInt64 totalCompressedSize = 0;

			MemoryStream inStream = new MemoryStream(rg.Buffer, 0, (int)rg.BufferSize);
			MemoryStream compressedStream = new MemoryStream((int)kCompressedBufferSize);
			CrcOutStream crcOutStream = new CrcOutStream();
			for (Int32 i = 0; i < numIterations; i++)
			{
				progressInfo.Init();
				inStream.Seek(0, SeekOrigin.Begin);
				compressedStream.Seek(0, SeekOrigin.Begin);
				encoder.Code(inStream, compressedStream, -1, -1, progressInfo);
				TimeSpan sp2 = DateTime.UtcNow - progressInfo.Time;
				UInt64 encodeTime = (UInt64)sp2.Ticks;

				long compressedSize = compressedStream.Position;
				if (progressInfo.InSize == 0)
					throw (new Exception("Internal ERROR 1282"));

				UInt64 decodeTime = 0;
				for (int j = 0; j < 2; j++)
				{
					compressedStream.Seek(0, SeekOrigin.Begin);
					crcOutStream.Init();

					decoder.SetDecoderProperties(propArray);
					UInt64 outSize = kBufferSize;
					System.DateTime startTime = DateTime.UtcNow;
					decoder.Code(compressedStream, crcOutStream, 0, (Int64)outSize, null);
					TimeSpan sp = (DateTime.UtcNow - startTime);
					decodeTime = (ulong)sp.Ticks;
					if (crcOutStream.GetDigest() != crc.GetDigest())
						throw (new Exception("CRC Error"));
				}
				UInt64 benchSize = kBufferSize - (UInt64)progressInfo.InSize;
				PrintResults(dictionarySize, encodeTime, benchSize, false, 0);
				Console.Write("     ");
				PrintResults(dictionarySize, decodeTime, kBufferSize, true, (ulong)compressedSize);
				Console.WriteLine();

				totalBenchSize += benchSize;
				totalEncodeTime += encodeTime;
				totalDecodeTime += decodeTime;
				totalCompressedSize += (ulong)compressedSize;
			}
			Console.WriteLine("---------------------------------------------------");
			PrintResults(dictionarySize, totalEncodeTime, totalBenchSize, false, 0);
			Console.Write("     ");
			PrintResults(dictionarySize, totalDecodeTime,
					kBufferSize * (UInt64)numIterations, true, totalCompressedSize);
			Console.WriteLine("    Average");
			return 0;
		}

	}
}
