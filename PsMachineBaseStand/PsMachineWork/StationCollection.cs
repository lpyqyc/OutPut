using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作站集合
	/// </summary>
	public class StationCollection<T> : SynCollection<T>, iWorkStationCollection<T>, IEnumerable<T>, IEnumerable, INotifyCollectionChanged where T : class, iWorkStation
	{
		public virtual iWorkStation ParentStation { get; set; }

		iWorkStation iWorkStationCollection<T>.this[int index] => base[index];

		public StationCollection()
		{
		}

		public StationCollection(iWorkStation myParentStation)
		{
			ParentStation = myParentStation;
		}
	}
}
