using System;

using Force.HammingRecovery.Implementations;

namespace Force.HammingRecovery
{
	public class RecoveryProcessor
	{
		private readonly IRecovery _recovery;

		internal RecoveryProcessor(IRecovery recovery)
		{
			if (recovery == null)
				throw new ArgumentNullException("recovery");
			_recovery = recovery;
		}

		public int GetDataBlockSize()
		{
			return _recovery.GetBlockSize();
		}

		public int GetRecoveryBlockSize()
		{
			return _recovery.GetRecoverySize();
		}
		
		public int CalculateRecoverySize(int dataSize)
		{
			if (dataSize < 0)
				throw new ArgumentOutOfRangeException("dataSize", "Invalid data size");

			if (dataSize == 0) return 0;

			var dataCnt = _recovery.GetBlockSize();
			var recCnt = _recovery.GetRecoverySize();
			var totalCnt = dataCnt + recCnt;

			return (((dataSize - 1) / dataCnt) + 1) * recCnt;
		}

		public int CalculateTotalSize(int dataSize)
		{
			return CalculateRecoverySize(dataSize) + dataSize;
		}

		public byte[] InsertRecoveryInfo(byte[] input, int offset, int length)
		{
			if (offset >= length)
				throw new InvalidOperationException("Invalid offset");
			if (offset < 0)
				throw new InvalidOperationException("Invalid offset");
			if (length < 0)
				throw new InvalidOperationException("Invalid length");
			if (input == null)
				throw new ArgumentNullException("input");
			if (input.Length - offset - length < 0)
				throw new InvalidOperationException("Invalid combination of offset and length");

			var dataCnt = _recovery.GetBlockSize();
			var recCnt = _recovery.GetRecoverySize();
			var totalCnt = dataCnt + recCnt;

			var output = new byte[(((length - 1) / dataCnt) + 1) * totalCnt];

			var oo = 0;
			while (length >= dataCnt)
			{
				Buffer.BlockCopy(input, offset, output, oo, dataCnt);
				_recovery.Insert(input, offset, output, oo + dataCnt);
				length -= dataCnt;
				oo += totalCnt;
				offset += dataCnt;
			}

			return output;
		}

		public byte[] ValidateAndRecover(byte[] input, int offset, int length)
		{
			if (offset >= length)
				throw new InvalidOperationException("Invalid offset");
			if (offset < 0)
				throw new InvalidOperationException("Invalid offset");
			if (length < 0)
				throw new InvalidOperationException("Invalid length");
			if (input == null)
				throw new ArgumentNullException("input");
			if (input.Length - offset - length < 0)
				throw new InvalidOperationException("Invalid combination of offset and length");

			var dataCnt = _recovery.GetBlockSize();
			var recCnt = _recovery.GetRecoverySize();
			var totalCnt = dataCnt + recCnt;

			if (length % totalCnt != 0)
				throw new InvalidOperationException("Invalid recovery data length");
			var output = new byte[length / totalCnt * dataCnt];

			var oo = 0;
			while (length >= totalCnt)
			{
				Buffer.BlockCopy(input, offset, output, oo, dataCnt);
				_recovery.Recover(input, offset + dataCnt, output, oo);
				//_recovery.Recover(output, oo, input, offset + dataCnt);

				oo += dataCnt;
				offset += totalCnt;
				length -= totalCnt;
			}

			return output;
		}
	}
}
