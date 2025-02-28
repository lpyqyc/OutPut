using System;

namespace PsMachineWork
{
	/// <summary>验证方法中，检查到值条件不符合抛出来的异常
	/// </summary>
	public class ActionValidateException : Exception
	{
		/// <summary>初始化一个验证异常
		/// </summary>
		public ActionValidateException()
		{
		}

		/// <summary>初始化一个验证异常
		/// </summary>
		public ActionValidateException(string myMsg)
			: base(myMsg)
		{
		}

		/// <summary>初始化一个验证异常
		/// </summary>
		public ActionValidateException(string myMsg, Exception innerException)
			: base(myMsg, innerException)
		{
		}
	}
}
