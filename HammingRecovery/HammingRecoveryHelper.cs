using Force.HammingRecovery.Implementations;

namespace Force.HammingRecovery
{
	public static class HammingRecoveryHelper
	{
		public static RecoveryProcessor Create(int recoveryLevel)
		{
			IRecovery _recovery;
			if (recoveryLevel == 3)
				_recovery = new Recovery3();
			else if (recoveryLevel == 4)
				_recovery = new Recovery4();
			else if (recoveryLevel == 5)
				_recovery = new Recovery5();
			else if (recoveryLevel == 6)
				_recovery = new Recovery6();
			else if (recoveryLevel == 7)
				_recovery = new Recovery7();
			else 
				_recovery = new Recovery8();

			return new RecoveryProcessor(_recovery);
		}
	}
}
