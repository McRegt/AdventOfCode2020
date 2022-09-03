// © Traxion Development Services

namespace AdventOfCode2020.Days
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Day25
	{
		private readonly long _doorPublicKey = 0;
		private readonly long _cardPublicKey = 0;

		public Day25(string filePath)
		{
			var file = new StreamReader(filePath);
			string result = file.ReadToEnd();

			file.Close();

			List<string> publicKeys = result.Split("\r\n").ToList();

			_doorPublicKey = long.Parse(publicKeys.First());
			_cardPublicKey = long.Parse(publicKeys.Last());
		}

		internal long Part1()
		{
			int doorLoopSize = CalculateLoopSize(_doorPublicKey);
			int carLoopSize = CalculateLoopSize(_cardPublicKey);

			return CalculateEncryptionKey(doorLoopSize, _cardPublicKey);
		}

		private int CalculateLoopSize(long value, long subjectNumber = 7)
		{
			long result = 1;
			var loopSize = 0;

			while (!result.Equals(value))
			{
				result = result * subjectNumber;
				result = result % 20201227;
				loopSize++;
			}

			return loopSize;
		}

		private long CalculateEncryptionKey(long loopSize, long subjectNumber)
		{
			long encryptionKey = 1;
			for (long i = 0; i < loopSize; i++)
			{
				encryptionKey = encryptionKey * subjectNumber;
				encryptionKey = encryptionKey % 20201227;
			}

			return encryptionKey;
		}

		internal string Part2()
		{
			return "Deposit the payment for the room. :D";
		}
	}
}
