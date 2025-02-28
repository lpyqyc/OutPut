using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>触发器的集合
	/// </summary>
	public interface iWorkTriggerCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkTrigger
	{
		/// <summary>所属的工作站
		/// </summary>
		iWorkStation ParentStation { get; }
	}
}
