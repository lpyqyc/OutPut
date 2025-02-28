using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>设备出料动作发生异常
	/// </summary>
	public class MachineExceptionStoreOut : Exception
	{
		public MachineExceptionStoreOut()
		{
		}

		public MachineExceptionStoreOut(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionStoreOut(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
