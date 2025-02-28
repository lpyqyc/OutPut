using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作站的点位集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PointCollection<TStation, TPoint> : SynCollection<TPoint>, iStationPointCollection<TPoint>, IEnumerable<TPoint>, IEnumerable, INotifyCollectionChanged where TStation : iWorkStation where TPoint : class, iStationPoint
	{
		/// <summary>所属工作站
		/// </summary>
		public TStation ParentStation { get; set; }

		/// <summary>所属工作站
		/// </summary>
		iWorkStation iStationPointCollection<TPoint>.ParentStation => ParentStation;

		iStationPoint iStationPointCollection<TPoint>.this[int index] => base[index];

		public PointCollection()
		{
		}

		public PointCollection(TStation myParentStation)
		{
			ParentStation = myParentStation;
		}
	}
}
