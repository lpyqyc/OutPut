using System;
using PsMachineWork.触发器.接口;

namespace PsMachineWork
{
	/// <summary>触发器
	/// </summary>
	public interface iWorkTrigger
	{
		/// <summary>所属的上一级工作站
		/// </summary>
		iWorkStation ParentStation { get; }

		/// <summary>包含的触发条件
		/// </summary>
		iWorkTriggerConditionCollection<iWorkTriggerCondition> ConditionCollection { get; }

		/// <summary>运行日志
		/// </summary>
		ILogManager LogManager { get; }

		/// <summary>工作步骤的编码
		/// </summary>
		string Code { get; }

		/// <summary>描述
		/// </summary>
		string Desc { get; }

		/// <summary>是否启动触发器，一般情况下，触发器默认都是启动的，
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>最后执行时间
		/// </summary>
		DateTime? LastRunTime { get; }

		/// <summary>执行描述
		/// </summary>
		string RunDesc { get; }

		/// <summary>加载数据结构
		/// 如果加载过程发生异常要直接抛出，应用程序需要终止程序继续执行
		/// </summary>
		void Init_LoadData();

		/// <summary>启动工作对象自检Task
		/// </summary>
		void Init_StarTask();

		/// <summary>启动禁用触发器
		/// </summary>
		/// <param name="myEnabled"></param>
		void Action_Enabled(bool myEnabled);

		/// <summary>执行触发器的命令
		/// </summary>
		void RunTrigger();
	}
}
