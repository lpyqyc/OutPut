using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>设备状态发生异常
	/// </summary>
	public class MachineExceptionStatus : Exception
	{
		public MachineExceptionStatus()
		{
		}

		public MachineExceptionStatus(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionStatus(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
