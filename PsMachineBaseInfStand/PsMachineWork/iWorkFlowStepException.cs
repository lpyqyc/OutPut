using System;

namespace PsMachineWork
{
	/// <summary>步骤执行异常处理
	/// 如果没有定义异常类型的处理方式，则默认无反复尝试执行本步，直到OK为止
	/// </summary>
	public interface iWorkFlowStepException
	{
		/// <summary>所属的工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }

		/// <summary>排序号，序号越小的执行时判断的优先级越高
		/// </summary>
		int SeqNO { get; }

		/// <summary>异常的类型,此值不能为null，否则跳过此判定
		/// </summary>
		Type ExceptionType { get; }

		/// <summary>异常时调用的执行步骤内部编码
		/// </summary>
		string StepInnerCode { get; }
	}
}
