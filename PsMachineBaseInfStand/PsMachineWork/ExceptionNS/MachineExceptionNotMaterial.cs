using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>检查不到料件时，触发的无料异常(正常应该感应到料件)
	/// </summary>
	public class MachineExceptionNotMaterial : Exception
	{
		public MachineExceptionNotMaterial()
		{
		}

		public MachineExceptionNotMaterial(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionNotMaterial(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
