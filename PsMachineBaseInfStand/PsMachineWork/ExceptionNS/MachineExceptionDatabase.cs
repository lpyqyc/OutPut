using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>数据库交互异常或者api 交互异常
	/// </summary>
	public class MachineExceptionDatabase : Exception
	{
		public MachineExceptionDatabase()
		{
		}

		public MachineExceptionDatabase(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionDatabase(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
