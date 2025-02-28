using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineWork.触发器.接口;

namespace PsMachineWork
{
	/// <summary>触发器启动条件的集合
	/// </summary>
	public interface iWorkTriggerConditionCollection<out T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged where T : iWorkTriggerCondition
	{
		/// <summary>所在的触发器
		/// </summary>
		iWorkTrigger WorkTrigger { get; }
	}
}
