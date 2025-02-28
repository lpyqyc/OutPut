using System;

namespace PsMachineWork
{
	/// <summary>标记一个无参数方法，是一个流程步骤的创建函数
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class FlowStepAttribute : Attribute
	{
		/// <summary>对应执行步骤的Code属性
		/// </summary>
		public string Code { get; set; }

		/// <summary>验证函数名称
		/// </summary>
		public string ValidateCode { get; set; }

		/// <summary>注释说明
		/// </summary>
		public string Desc { get; set; }

		/// <summary>下一个步骤的内码
		/// </summary>
		public string NextStep { get; set; }

		/// <summary>创建的 执行步骤 额外指定的 Type
		/// </summary>
		public Type CreateType { get; set; }

		public FlowStepAttribute()
		{
		}

		public FlowStepAttribute(string myDesc)
		{
			Desc = myDesc;
		}

		public FlowStepAttribute(string myCode, string myDesc)
		{
			Code = myCode;
			Desc = myDesc;
		}
	}
}
