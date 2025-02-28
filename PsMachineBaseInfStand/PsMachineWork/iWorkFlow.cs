using System;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>一个工作流
	/// </summary>
	public interface iWorkFlow : INotifyPropertyChanged
	{
		/// <summary>所在的工作站
		/// </summary>
		iWorkStation ParentStation { get; }

		/// <summary>工作步骤
		/// </summary>
		iWorkFlowStepCollection<iWorkFlowStep> Steps { get; }

		/// <summary>工作流启动时的限制条件
		/// </summary>
		iWorkFlowStarConditionCollection<iWorkFlowStarCondition> FlowStarConditionList { get; }

		/// <summary>异常处理步骤
		/// </summary>
		iWorkFlowStepExceptionCollection<iWorkFlowStepException> StepExceptionList { get; }

		/// <summary>引用的工作站列表
		/// </summary>
		iWorkFlowRefStationCollection<iWorkFlowRefStation> RefStationList { get; }

		/// <summary>运行日志
		/// </summary>
		ILogManager LogManager { get; }

		/// <summary>编码
		/// </summary>
		string Code { get; }

		/// <summary>流程注释
		/// </summary>
		string Desc { get; }

		/// <summary>流程已经启动
		/// </summary>
		bool IsStart { get; }

		/// <summary>运行时动态说明
		/// </summary>
		string RunDesc { get; }

		/// <summary>流程已经结束或者取消
		/// </summary>
		bool IsStop { get; }

		/// <summary>创建时间
		/// </summary>
		DateTime CreateDate { get; }

		/// <summary>启动时间
		/// </summary>
		DateTime? StarDate { get; }

		/// <summary>是否已经启动事务流程事务
		/// </summary>
		bool IsStarFlowTask { get; }

		/// <summary>选择的步骤
		/// </summary>
		iWorkFlowStep SelectStep { get; set; }

		/// <summary>加载数据结构对象
		/// </summary>
		void Init_LoadData();

		/// <summary>启动工作对象自检Task
		/// </summary>
		void Init_StarTask();

		/// <summary>把流程标记为强制结束，但并不真正结束流程，
		/// 只有当流程线程走到本步时，才会结束流程
		/// </summary>
		void Action_Stop();
	}
}
