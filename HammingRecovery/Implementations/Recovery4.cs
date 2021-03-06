﻿// <autogenerated>by special utility</autogenerated>

namespace Force.HammingRecovery.Implementations
{
internal class Recovery4 : IRecovery
{
public int GetBlockSize() { return 11;}
public int GetRecoverySize() { return 4;}
public void Insert(byte[] input, int io, byte[] output, int oo) { output[oo + 0] = (byte)(input[io + 0] ^ input[io + 1] ^ input[io + 3] ^ input[io + 4] ^ input[io + 6] ^ input[io + 8] ^ input[io + 10] ^  0);
output[oo + 1] = (byte)(input[io + 0] ^ input[io + 2] ^ input[io + 3] ^ input[io + 5] ^ input[io + 6] ^ input[io + 9] ^ input[io + 10] ^  0);
output[oo + 2] = (byte)(input[io + 1] ^ input[io + 2] ^ input[io + 3] ^ input[io + 7] ^ input[io + 8] ^ input[io + 9] ^ input[io + 10] ^  0);
output[oo + 3] = (byte)(input[io + 4] ^ input[io + 5] ^ input[io + 6] ^ input[io + 7] ^ input[io + 8] ^ input[io + 9] ^ input[io + 10] ^  0);
}
public bool Recover(byte[] input, int io, byte[] output, int oo) { var r0 = (byte)(output[oo + 0] ^ output[oo + 1] ^ output[oo + 3] ^ output[oo + 4] ^ output[oo + 6] ^ output[oo + 8] ^ output[oo + 10] ^  0);var f0 = r0 != input[io + 0];
var r1 = (byte)(output[oo + 0] ^ output[oo + 2] ^ output[oo + 3] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 9] ^ output[oo + 10] ^  0);var f1 = r1 != input[io + 1];
var r2 = (byte)(output[oo + 1] ^ output[oo + 2] ^ output[oo + 3] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10] ^  0);var f2 = r2 != input[io + 2];
var r3 = (byte)(output[oo + 4] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10] ^  0);var f3 = r3 != input[io + 3];
if (!f0 && !f1 && !f2 && !f3) { /* no error */ return false; }
else if (f0 && f1 && !f2 && !f3) output[oo + 0] = (byte)(input[io + 1] ^ output[oo + 2] ^ output[oo + 3] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 9] ^ output[oo + 10]);
else if (f0 && !f1 && f2 && !f3) output[oo + 1] = (byte)(input[io + 2] ^ output[oo + 2] ^ output[oo + 3] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (!f0 && f1 && f2 && !f3) output[oo + 2] = (byte)(input[io + 2] ^ output[oo + 1] ^ output[oo + 3] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (f0 && f1 && f2 && !f3) output[oo + 3] = (byte)(input[io + 2] ^ output[oo + 1] ^ output[oo + 2] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (f0 && !f1 && !f2 && f3) output[oo + 4] = (byte)(input[io + 3] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (!f0 && f1 && !f2 && f3) output[oo + 5] = (byte)(input[io + 3] ^ output[oo + 4] ^ output[oo + 6] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (f0 && f1 && !f2 && f3) output[oo + 6] = (byte)(input[io + 3] ^ output[oo + 4] ^ output[oo + 5] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (!f0 && !f1 && f2 && f3) output[oo + 7] = (byte)(input[io + 3] ^ output[oo + 4] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 8] ^ output[oo + 9] ^ output[oo + 10]);
else if (f0 && !f1 && f2 && f3) output[oo + 8] = (byte)(input[io + 3] ^ output[oo + 4] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 7] ^ output[oo + 9] ^ output[oo + 10]);
else if (!f0 && f1 && f2 && f3) output[oo + 9] = (byte)(input[io + 3] ^ output[oo + 4] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 10]);
else if (f0 && f1 && f2 && f3) output[oo + 10] = (byte)(input[io + 3] ^ output[oo + 4] ^ output[oo + 5] ^ output[oo + 6] ^ output[oo + 7] ^ output[oo + 8] ^ output[oo + 9]);
else { /* error in recovery info. assume, that normal data is ok */ }
return true;
}
}
}
