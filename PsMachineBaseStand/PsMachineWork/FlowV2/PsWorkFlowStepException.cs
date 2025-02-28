using System;

namespace PsMachineWork.FlowV2
{
	/// <summary>步骤执行异常处理
	/// 如果没有定义异常类型的处理方式，则默认无反复尝试执行本步，直到OK为止
	/// </summary>
	public class PsWorkFlowStepException : iWorkFlowStepException
	{
		/// <summary>所属的工作流
		/// </summary>
		public iWorkFlow WorkFlow { get; set; }

		/// <summary>排序号，序号越小的执行时判断的优先级越高
		/// </summary>
		public int SeqNO { get; set; }

		/// <summary>异常的类型,此值不能为null，否则跳过此判定
		/// 此类型值必须是派生于 Exception 的 Type
		/// </summary>
		public Type ExceptionType { get; set; }

		/// <summary>异常时调用的执行步骤内部编码
		/// </summary>
		public string StepInnerCode { get; set; }

		public PsWorkFlowStepException(iWorkFlow workFlow)
		{
			WorkFlow = workFlow;
			((iAddItem)workFlow.StepExceptionList).Add(this);
		}

		public PsWorkFlowStepException(iWorkFlow workFlow, int seqNO, Type exceptionType, string stepInnerCode)
		{
			WorkFlow = workFlow;
			SeqNO = seqNO;
			ExceptionType = exceptionType;
			StepInnerCode = stepInnerCode;
			((iAddItem)workFlow.StepExceptionList).Add(this);
		}
	}
}
