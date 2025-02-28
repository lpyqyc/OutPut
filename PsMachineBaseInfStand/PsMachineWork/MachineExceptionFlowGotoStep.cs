using System;

namespace PsMachineWork
{
	/// <summary>工作步骤中，一个步骤发生了异常，用于控制向另一个步骤跳转的异常
	/// </summary>
	public class MachineExceptionFlowGotoStep : Exception
	{
		/// <summary>异常后，跳转后另一个步骤的 InnerCode
		/// </summary>
		public string GotoStepInnerCode { get; set; }

		/// <summary>创建一个新的步骤跳转异常
		/// </summary>
		public MachineExceptionFlowGotoStep()
		{
		}

		/// <summary>创建一个新的步骤跳转异常
		/// </summary>
		/// <param name="myMsg">异常消息内容</param>
		/// <param name="myGotoStepInnerCode">跳转的步骤 InnerCode</param>
		public MachineExceptionFlowGotoStep(string myMsg, string myGotoStepInnerCode)
			: base(myMsg)
		{
			GotoStepInnerCode = myGotoStepInnerCode;
		}

		/// <summary>创建一个新的步骤跳转异常
		/// </summary>
		/// <param name="myMsg">异常消息内容</param>
		/// <param name="myGotoStepInnerCode"></param>
		/// <param name="innerException"></param>
		public MachineExceptionFlowGotoStep(string myMsg, string myGotoStepInnerCode, Exception innerException)
			: base(myMsg, innerException)
		{
			GotoStepInnerCode = myGotoStepInnerCode;
		}
	}
}
