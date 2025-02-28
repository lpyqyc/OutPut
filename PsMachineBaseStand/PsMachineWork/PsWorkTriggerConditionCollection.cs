using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;
using PsMachineWork.触发器.接口;

namespace PsMachineWork
{
	/// <summary>触发器条件集合
	/// </summary>
	/// <typeparam name="Ttriger"></typeparam>
	/// <typeparam name="Tcondition"></typeparam>
	public class PsWorkTriggerConditionCollection<Ttriger, Tcondition> : SynCollection<Tcondition>, iWorkTriggerConditionCollection<Tcondition>, IEnumerable, IEnumerable<Tcondition>, INotifyCollectionChanged, iAddItem where Ttriger : iWorkTrigger where Tcondition : class, iWorkTriggerCondition
	{
		public Ttriger WorkTrigger { get; set; }

		iWorkTrigger iWorkTriggerConditionCollection<Tcondition>.WorkTrigger => WorkTrigger;

		public PsWorkTriggerConditionCollection(Ttriger workTrigger)
		{
			WorkTrigger = workTrigger;
		}

		void iAddItem.Add(object item)
		{
			Add((Tcondition)item);
		}
	}
}
