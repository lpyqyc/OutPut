using System;

namespace PsMachineWork
{
	/// <summary>设备正在等待动作到位中
	/// </summary>
	public class MachineWaitException : Exception
	{
		public MachineWaitException()
		{
		}

		public MachineWaitException(string myMsg)
			: base(myMsg)
		{
		}

		public MachineWaitException(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
