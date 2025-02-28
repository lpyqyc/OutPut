using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>设备复位过程中发生异常
	/// </summary>
	public class MachineExceptionResting : Exception
	{
		public MachineExceptionResting()
		{
		}

		public MachineExceptionResting(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionResting(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
