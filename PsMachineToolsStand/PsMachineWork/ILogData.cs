using System;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>单条的日志消息
	/// </summary>
	public interface ILogData : INotifyPropertyChanged
	{
		/// <summary>创建时间
		/// </summary>
		DateTime CreateTime { get; }

		/// <summary>消息主体
		/// </summary>
		string Message { get; }

		/// <summary>消息附加的详细内容
		/// </summary>
		string Desc { get; }

		/// <summary>是否属于异常消息
		/// </summary>
		bool IsException { get; }

		/// <summary>是否显示详细的消息扩展内容 Desc
		/// </summary>
		bool IsShowDesc { get; set; }

		/// <summary>消息和时间+文本提示
		/// </summary>
		string TimeAndMessage { get; }
	}
}
