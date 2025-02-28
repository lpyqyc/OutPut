using System;

namespace PsMachineWork.ExceptionNS
{
	/// <summary>检查到有料时误发的异常(正常应该没料)
	/// </summary>
	public class MachineExceptionHasMaterial : Exception
	{
		public MachineExceptionHasMaterial()
		{
		}

		public MachineExceptionHasMaterial(string myMsg)
			: base(myMsg)
		{
		}

		public MachineExceptionHasMaterial(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
