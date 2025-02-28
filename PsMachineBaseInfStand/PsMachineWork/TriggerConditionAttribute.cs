using System;

namespace PsMachineWork
{
	/// <summary>标记一个无参数方法，是一个条件创建条件
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class TriggerConditionAttribute : Attribute
	{
		/// <summary>注释说明
		/// </summary>
		public string Desc { get; set; }

		/// <summary>创建的 启动条件 额外指定的 Type
		/// </summary>
		public Type CreateType { get; set; }

		public TriggerConditionAttribute()
		{
		}

		public TriggerConditionAttribute(string myDesc)
		{
			Desc = myDesc;
		}
	}
}
