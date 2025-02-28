using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>执行线程管理
	/// </summary>
	public interface iWorkTaskRun : iWorkTaskRunFuncCollection<iWorkTaskRunFunc>, IEnumerable<iWorkTaskRunFunc>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		/// <summary>任务的KEY
		/// </summary>
		string TaskID { get; }

		/// <summary>任务注释
		/// </summary>
		string TaskDesc { get; }

		/// <summary>执行完一个循环之后,在进行下一个循环之前,停止刷新的时间
		/// 用于防止一些点位或者事务特别多的,高频地循环把CPU吃满
		/// </summary>
		int TimeDelay { get; }
	}
}
