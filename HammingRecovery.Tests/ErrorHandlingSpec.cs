using System;

using Force.HammingRecovery;

using NUnit.Framework;

namespace HammingRecovery.Tests
{
	[TestFixture]
	public class ErrorHandlingSpec
	{
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		[TestCase(7)]
		[TestCase(8)]
		public void ValidateRecovery(int size)
		{
			var rp = HammingRecoveryHelper.Create(4);
			var r = new Random();
			var source = new byte[rp.GetDataBlockSize()];
			r.NextBytes(source);
			var res = rp.InsertRecoveryInfo(source, 0, source.Length);
			Assert.That(res.Length, Is.EqualTo(rp.CalculateTotalSize(source.Length)));

			var recovered = rp.ValidateAndRecover(res, 0, res.Length);
			CollectionAssert.AreEqual(source, recovered);

			for (int i = 0; i < res.Length; i++)
			{
				res[i]++;
				recovered = rp.ValidateAndRecover(res, 0, res.Length);
				CollectionAssert.AreEqual(source, recovered, "Error in recovery: " + i);
				res[i]--;
			}
		}
	}
}
