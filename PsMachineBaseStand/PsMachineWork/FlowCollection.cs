using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作站的点位集合
	/// </summary>
	/// <typeparam name="TFlow"></typeparam>
	public class FlowCollection<TFlow> : SynCollection<TFlow>, iWorkFlowCollection<TFlow>, IEnumerable, IEnumerable<TFlow>, INotifyCollectionChanged, iAddItem where TFlow : class, iWorkFlow
	{
		/// <summary>所属的工作站
		/// </summary>
		public virtual iWorkStation ParentStation { get; set; }

		public FlowCollection()
		{
		}

		public FlowCollection(iWorkStation myParentStation)
		{
			ParentStation = myParentStation;
		}

		/// <summary>向集合中添加一个新成员
		/// </summary>
		/// <param name="item"></param>
		void iAddItem.Add(object item)
		{
			Add((TFlow)item);
		}
	}
}
