using System;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>工作流的工作步骤
	/// </summary>
	public interface iWorkFlowStep : INotifyPropertyChanged
	{
		/// <summary>所在的工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }

		/// <summary>前一个工作步骤
		/// </summary>
		iWorkFlowStep FrontStep { get; }

		/// <summary>工作步骤的编码
		/// </summary>
		string Code { get; }

		/// <summary>工作步骤的内部编码
		/// </summary>
		string InnerFuncCode { get; }

		/// <summary>描述
		/// </summary>
		string Desc { get; }

		/// <summary>开始执行
		/// </summary>
		bool IsStart { get; }

		/// <summary>运行有异常
		/// </summary>
		bool IsException { get; }

		/// <summary>执行结束
		/// </summary>
		bool IsComplate { get; }

		/// <summary>运行状态描述，如果有异常在此写入
		/// </summary>
		string RunDesc { get; }

		/// <summary>最后执行时间
		/// </summary>
		DateTime? LastRunTime { get; }

		/// <summary>步骤结束后，是否已经执行下一个步骤的分支
		/// </summary>
		bool IsStarNextStep { get; }

		/// <summary>执行下一个分支的内码
		/// </summary>
		string NextStep { get; }

		/// <summary>验证是否允许运行本步骤,不通过时会抛出异常
		/// </summary>
		void ValidateAllowRunStep();

		/// <summary>执行本步
		/// 注意这里只是正常的逻辑条件封闭，并不需要包含执行本步异常时需要的善后处理
		/// </summary>
		void RunStep();
	}
}
