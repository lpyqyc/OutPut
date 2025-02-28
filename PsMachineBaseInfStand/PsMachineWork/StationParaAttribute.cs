using System;

namespace PsMachineWork
{
	/// <summary>工作站上声明参数, 必须在创建时指定这些参数值
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class StationParaAttribute : Attribute
	{
		/// <summary>配置参数的声明标题
		/// </summary>
		public string Caption { get; set; }

		/// <summary>不允许空值
		/// </summary>
		public bool IsNotNull { get; set; }

		/// <summary>必须重填的参数
		/// </summary>
		public bool IsRequestIn { get; set; }

		public StationParaAttribute()
		{
		}

		public StationParaAttribute(string myCaption)
		{
			Caption = myCaption;
		}
	}
}
