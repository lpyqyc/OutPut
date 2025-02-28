using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>触发器集合
	/// </summary>
	public class PsWorkTiggerCollection<Ttriger> : SynCollection<Ttriger>, iWorkTriggerCollection<Ttriger>, IEnumerable, IEnumerable<Ttriger>, INotifyCollectionChanged, iAddItem where Ttriger : class, iWorkTrigger
	{
		/// <summary>所属的工作站
		/// </summary>
		public iWorkStation ParentStation { get; set; }

		public PsWorkTiggerCollection()
		{
		}

		public PsWorkTiggerCollection(iWorkStation myParentStation)
		{
			ParentStation = myParentStation;
		}

		void iAddItem.Add(object item)
		{
			Add((Ttriger)item);
		}
	}
}
