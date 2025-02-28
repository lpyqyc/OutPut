using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>工作站集合接口
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface iWorkStationCollection<out T> : IEnumerable<T>, IEnumerable, INotifyCollectionChanged where T : iWorkStation
	{
		/// <summary>所属的工作站
		/// </summary>
		iWorkStation ParentStation { get; }

		/// <summary>索引属性
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		iWorkStation this[int index] { get; }
	}
}
