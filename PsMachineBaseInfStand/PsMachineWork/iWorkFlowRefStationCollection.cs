using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>工作流中引用工作站的集合
	/// </summary>
	public interface iWorkFlowRefStationCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkFlowRefStation
	{
		/// <summary>工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }
	}
}
