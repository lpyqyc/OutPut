using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作流的启动条件
	/// </summary>
	public class WorkFlowStarConditionCollection<TFlow, TStarCondition> : SynCollection<TStarCondition>, iWorkFlowStarConditionCollection<TStarCondition>, IEnumerable, IEnumerable<TStarCondition>, INotifyCollectionChanged, iAddItem where TFlow : iWorkFlow where TStarCondition : class, iWorkFlowStarCondition
	{
		private List<TStarCondition> list = new List<TStarCondition>();

		/// <summary>工作流
		/// </summary>
		public TFlow WorkFlow { get; set; }

		/// <summary>工作流
		/// </summary>
		iWorkFlow iWorkFlowStarConditionCollection<TStarCondition>.WorkFlow => WorkFlow;

		public WorkFlowStarConditionCollection(TFlow myFlow)
		{
			WorkFlow = myFlow;
		}

		/// <summary>向集合中添加一个新成员
		/// </summary>
		/// <param name="item"></param>
		void iAddItem.Add(object item)
		{
			Add((TStarCondition)item);
		}
	}
}
