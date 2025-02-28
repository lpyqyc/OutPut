using System;

namespace PsMachineWork
{
	/// <summary>工作站上声明的公共调用函数, 支持放在工作站、工作流、工作步骤上面声明
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class StationFuncAttribute : Attribute
	{
		/// <summary>工作站声明函数的标题
		/// </summary>
		public string Caption { get; set; }

		public StationFuncAttribute()
		{
		}

		public StationFuncAttribute(string myCaption)
		{
			Caption = myCaption;
		}
	}
}
