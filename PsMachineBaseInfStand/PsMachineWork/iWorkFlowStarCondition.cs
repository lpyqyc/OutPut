using System;

namespace PsMachineWork
{
	/// <summary>工作流任务的启动条件
	/// 一般来说，用户请求建立的流程会放在缓存池中，等到有空闲时执行
	/// 扫描线程在启动流程前会先检查条件, 比如用户发起A点到B点的运输任务，
	/// 但B点可能没有开机或者不在线，或者B点目前非空闲状态（正在入仓)
	/// </summary>
	public interface iWorkFlowStarCondition
	{
		/// <summary>所在的工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }

		/// <summary>条码编码
		/// </summary>
		string Code { get; }

		/// <summary>注释
		/// </summary>
		string Desc { get; }

		/// <summary>验证通过
		/// </summary>
		bool IsComplate { get; }

		/// <summary>验证异常
		/// </summary>
		bool IsException { get; }

		/// <summary>运行时注释
		/// </summary>
		string RunDesc { get; }

		/// <summary>最后运行时间
		/// </summary>
		DateTime? LastRunTime { get; }

		/// <summary>执行验证,如果验证不通过会抛出异常
		/// 注意这里只是正常的逻辑封装，并不需要包含异常时各种善后处理
		/// </summary>
		void RunValidate();
	}
}
