using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作步骤的集合
	/// </summary>
	/// <typeparam name="TStep"></typeparam>
	public class FlowStepCollection<TFlow, TStep> : SynCollection<TStep>, iWorkFlowStepCollection<TStep>, IEnumerable, IEnumerable<TStep>, INotifyCollectionChanged, iAddItem where TFlow : iWorkFlow where TStep : class, iWorkFlowStep
	{
		/// <summary>所在工作流
		/// </summary>
		public TFlow WorkFlow { get; set; }

		/// <summary>所在工作流
		/// </summary>
		iWorkFlow iWorkFlowStepCollection<TStep>.WorkFlow => WorkFlow;

		public FlowStepCollection(TFlow myFlow)
		{
			WorkFlow = myFlow;
		}

		/// <summary>向集合中添加一个新成员
		/// </summary>
		/// <param name="item"></param>
		void iAddItem.Add(object item)
		{
			Add((TStep)item);
		}
	}
}
