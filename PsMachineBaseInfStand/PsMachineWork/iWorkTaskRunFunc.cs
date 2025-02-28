using System;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>线程管理入口函数
	/// </summary>
	public interface iWorkTaskRunFunc : INotifyPropertyChanged
	{
		/// <summary>任务注释
		/// </summary>
		string TaskDesc { get; }

		/// <summary>最后执行时间
		/// </summary>
		DateTime? LastRunTime { get; }
	}
}
