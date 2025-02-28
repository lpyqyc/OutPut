using System;

namespace PsMachineWork
{
	/// <summary>工作站上声明参数, 必须在创建时指定这些参数值
	/// 将该特性添加到属性上，表示属性本身是一个点位值
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class StationPointAttribute : Attribute
	{
		/// <summary>点位的Code编码
		/// </summary>
		public string Code { get; set; }

		/// <summary>说明
		/// </summary>
		public string Desc { get; set; }

		/// <summary>点位的读写类型
		/// </summary>
		public emPointReadWriteType ReadWriteType { get; set; }

		/// <summary>自动刷新点位值
		/// </summary>
		public bool IsAutoRefresh { get; set; }

		/// <summary>是否本地内存值
		/// </summary>
		public bool IsLocalDataPoint { get; set; }

		/// <summary>创建的 执行步骤 额外指定的 Type
		/// </summary>
		public Type CreateType { get; set; }

		public StationPointAttribute()
		{
		}

		public StationPointAttribute(string myCode, string myDesc, emPointReadWriteType myReadWriteType, bool myIsAutoRefresh)
		{
			Code = myCode;
			Desc = myDesc;
			ReadWriteType = myReadWriteType;
			IsAutoRefresh = myIsAutoRefresh;
		}
	}
}
