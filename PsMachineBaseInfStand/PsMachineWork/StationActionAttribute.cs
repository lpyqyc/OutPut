using System;

namespace PsMachineWork
{
	/// <summary>工作站上声明的Action按钮, 支持放在工作站、工作流、工作步骤上面声明, 函数本身无返回值
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class StationActionAttribute : Attribute
	{
		public string ID { get; set; }

		/// <summary>
		/// </summary>
		public string Caption { get; set; }

		/// <summary>可执行条件约束属性名称,如果没有定义，则默认没有约束条件
		/// </summary>
		public string CriteriaProperty { get; set; }

		/// <summary>可显示约束条件属性名称
		/// </summary>
		public string VisibleProperty { get; set; }

		public StationActionAttribute()
		{
		}

		public StationActionAttribute(string myCaption)
		{
			Caption = myCaption;
		}
	}
}
