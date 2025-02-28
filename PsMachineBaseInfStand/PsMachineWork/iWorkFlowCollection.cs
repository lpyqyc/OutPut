using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>工作流的集合
	/// </summary>
	public interface iWorkFlowCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkFlow
	{
		/// <summary>所属的工作站
		/// </summary>
		iWorkStation ParentStation { get; }
	}
}
