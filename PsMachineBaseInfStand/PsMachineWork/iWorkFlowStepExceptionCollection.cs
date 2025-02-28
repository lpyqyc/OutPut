using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>步骤执行异常处理集合
	/// </summary>
	public interface iWorkFlowStepExceptionCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkFlowStepException
	{
		/// <summary>所在工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }
	}
}
