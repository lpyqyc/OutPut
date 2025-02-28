using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>工作流中工作步骤的集合
	/// </summary>
	public interface iWorkFlowStepCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkFlowStep
	{
		/// <summary>所在工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }
	}
}
