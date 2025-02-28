using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>料件条码识别发生异常
	/// </summary>
	public class MachineExceptionGoodsInfo : Exception
	{
		public MachineExceptionGoodsInfo()
		{
		}

		public MachineExceptionGoodsInfo(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionGoodsInfo(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
