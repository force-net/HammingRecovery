namespace Force.HammingRecovery.Implementations
{
	internal interface IRecovery
	{
		int GetBlockSize();

		int GetRecoverySize();

		void Insert(byte[] input, int io, byte[] output, int oo);

		bool Recover(byte[] input, int io, byte[] output, int oo);
	}
}
