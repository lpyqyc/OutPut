using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>工作流的启动条件的集合
	/// </summary>
	public interface iWorkFlowStarConditionCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkFlowStarCondition
	{
		/// <summary>工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }
	}
}
