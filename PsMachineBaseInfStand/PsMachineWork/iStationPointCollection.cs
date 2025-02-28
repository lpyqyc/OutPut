using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>工作站点位的集合接口
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface iStationPointCollection<out T> : IEnumerable<T>, IEnumerable, INotifyCollectionChanged where T : iStationPoint
	{
		/// <summary>所属的工作站
		/// </summary>
		iWorkStation ParentStation { get; }

		/// <summary>索引
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		iStationPoint this[int index] { get; }
	}
}
