using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作流中引用工作站的集合
	/// 一般来说，工作流中所有使用的触发条件的值，只能来自于这些引用的工作站
	/// 同样，所有动作触发，也只能使用这些引用工作站中的值写入
	/// </summary>
	public class FlowRefStationCollection<TFlow, TRefStation> : SynCollection<TRefStation>, iWorkFlowRefStationCollection<TRefStation>, IEnumerable, IEnumerable<TRefStation>, INotifyCollectionChanged, iAddItem where TFlow : iWorkFlow where TRefStation : class, iWorkFlowRefStation
	{
		/// <summary>所在工作流
		/// </summary>
		public TFlow WorkFlow { get; set; }

		/// <summary>所在工作流
		/// </summary>
		iWorkFlow iWorkFlowRefStationCollection<TRefStation>.WorkFlow => WorkFlow;

		public FlowRefStationCollection()
		{
		}

		public FlowRefStationCollection(TFlow myFlow)
		{
			WorkFlow = myFlow;
		}

		/// <summary>向集合中添加一个新成员
		/// </summary>
		/// <param name="item"></param>
		void iAddItem.Add(object item)
		{
			Add((TRefStation)item);
		}
	}
}
