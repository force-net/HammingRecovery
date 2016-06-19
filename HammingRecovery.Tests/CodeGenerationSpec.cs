﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HammingRecovery.Tests
{
	[TestFixture]
	public class CodeGenerationSpec
	{
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		[TestCase(7)]
		[TestCase(8)]
		public void GenerateRelatedCode(int addCnt)
		{
			var validSymbolsCnt = (1 << addCnt) - 1 - addCnt;
			// Assert.That(validSymbolsCnt, Is.EqualTo(4));

			var b1 = new StringBuilder();
			var b2 = new StringBuilder();

			/*for (var i = 0; i < validSymbolsCnt; i++)
			{
				b1.AppendFormat("output[oo + {0}] = input[io + {0}];", i).AppendLine();
				b2.AppendFormat("output[oo + {0}] = input[io + {0}];", i).AppendLine();
			}*/

			var opl = new List<byte[]>();
			for (var i = 0; i < 1 << addCnt; i++)
			{
				var y = i;
				var pos = 0;
				var onePoses = new byte[addCnt];
				while (y > 0)
				{
					if ((y & 1) == 1) onePoses[pos] = 1;
					y /= 2;
					pos++;
				}

				if (onePoses.Sum(x => x) > 1)
				{
					opl.Add(onePoses);
				}
			}

			var rtf = new List<int>[addCnt];
			for (var k = 0; k < rtf.Length; k++) rtf[k] = new List<int>();

			for (var k = 0; k < addCnt; k++)
			{
				b1.AppendFormat("output[oo + {0}] = (byte)(", k);
				b2.AppendFormat("var r{0} = (byte)(",  k);
				for (var l = 0; l < opl.Count; l++)
				{
					if (opl[l][k] == 1)
					{
						b1.AppendFormat("input[io + {0}] ^ ", l);
						b2.AppendFormat("output[oo + {0}] ^ ", l);
						rtf[k].Add(l);
					}
				}

				b1.AppendFormat(" 0);").AppendLine();
				b2.AppendFormat(" 0);var f{0} = r{0} != input[io + {1}];", k, k)
					.AppendLine();
			}

			b2.Append("if (" + string.Join(" && ", Enumerable.Range(0, addCnt).Select(x => "!f" + x)) + ") { /* no error */ return false; }").AppendLine();

			for (var l = 0; l < opl.Count; l++)
			{
				b2.Append("else if (");

				var fll = new byte[opl.Count];
				for (var k = 0; k < fll.Length; k++) fll[k] = 1;

				var rtfl = Enumerable.Range(0, validSymbolsCnt).ToArray();
				var rtfA = new List<int>();
				var kA = -1;
				var firstAppend = string.Empty;
				for (var k = 0; k < addCnt; k++)
				{
					b2.AppendFormat("{2}{1}f{0}", k, opl[l][k] == 1 ? string.Empty : "!", firstAppend);
					firstAppend = " && ";
					if (opl[l][k] == 1)
					{
						// b2.AppendFormat("r{0} + ", k);
						rtfl = rtfl.Intersect(rtf[k]).ToArray();
						rtfA = rtf[k];
						kA = k;
					}
					else
					{
						rtfl = rtfl.Except(rtf[k]).ToArray();
					}
				}

				var failedIdx = rtfl.Single();
				b2.AppendFormat(
					") output[oo + {0}] = (byte)(input[io + {1}] ^ {2});",
					failedIdx,
					kA,
					string.Join(" ^ ", rtfA.Where(x => x != failedIdx).Select(x => "output[oo + " + x + "]")))
					.AppendLine();
			}

			b2.AppendLine("else { /* error in recovery info. assume, that normal data is ok */ }");
			b2.AppendLine("return true;");

			/*for (var k = 0; k < addCnt; k++)
			{
				// b2.Append()
			}*/

			var rb = new StringBuilder();

			rb.Append("// <autogenerated>by special utility</autogenerated>").AppendLine().AppendLine();
			rb.Append("namespace Force.HammingRecovery.Implementations").AppendLine();
			rb.Append("{").AppendLine();
			rb.Append("internal class Recovery" + addCnt + " : IRecovery").AppendLine();
			rb.Append("{").AppendLine();
			rb.Append("public int GetBlockSize() { return " + validSymbolsCnt + ";}").AppendLine();
			rb.Append("public int GetRecoverySize() { return " + addCnt + ";}").AppendLine();
			rb.Append("public void Insert(byte[] input, int io, byte[] output, int oo) { " + b1.ToString()  + "}").AppendLine();
			rb.Append("public bool Recover(byte[] input, int io, byte[] output, int oo) { " + b2.ToString() + "}").AppendLine();
			rb.Append("}").AppendLine();
			rb.Append("}").AppendLine();
			File.WriteAllText(@"..\..\..\HammingRecovery\Implementations\Recovery" + addCnt + ".cs", rb.ToString(), Encoding.UTF8);

			Console.WriteLine(b1.ToString());
			Console.WriteLine(b2.ToString());
		}
	}
}