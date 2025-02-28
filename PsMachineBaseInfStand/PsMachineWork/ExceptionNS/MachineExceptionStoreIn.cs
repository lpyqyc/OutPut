using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>设备入料动作发生异常
	/// </summary>
	public class MachineExceptionStoreIn : Exception
	{
		public MachineExceptionStoreIn()
		{
		}

		public MachineExceptionStoreIn(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionStoreIn(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
