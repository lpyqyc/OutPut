using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>异常处理方式的集合
	/// </summary>
	/// <typeparam name="TFlow"></typeparam>
	/// <typeparam name="TStepEx"></typeparam>
	public class PsWorkFlowStepExceptionCollection<TFlow, TStepEx> : SynCollection<TStepEx>, iWorkFlowStepExceptionCollection<TStepEx>, IEnumerable, IEnumerable<TStepEx>, INotifyCollectionChanged, iAddItem where TFlow : iWorkFlow where TStepEx : class, iWorkFlowStepException
	{
		/// <summary>工作流
		/// </summary>
		public TFlow WorkFlow { get; set; }

		/// <summary>工作流
		/// </summary>
		iWorkFlow iWorkFlowStepExceptionCollection<TStepEx>.WorkFlow => WorkFlow;

		public PsWorkFlowStepExceptionCollection()
		{
		}

		public PsWorkFlowStepExceptionCollection(TFlow myFlow)
		{
			WorkFlow = myFlow;
		}

		/// <summary>向集合中添加一个新成员
		/// </summary>
		/// <param name="item"></param>
		void iAddItem.Add(object item)
		{
			Add((TStepEx)item);
		}
	}
}
