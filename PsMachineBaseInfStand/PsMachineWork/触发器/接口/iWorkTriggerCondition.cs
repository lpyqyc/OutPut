using System;

namespace PsMachineWork.触发器.接口
{
	/// <summary>触发器的启动条件
	/// </summary>
	public interface iWorkTriggerCondition
	{
		/// <summary>所在的触发器
		/// </summary>
		iWorkTrigger WorkTrigger { get; }

		/// <summary>编码
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
