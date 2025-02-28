using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>运行线程管理集合
	/// </summary>
	public interface iWorkTaskRunCollection<out T> : IEnumerable<T>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged where T : iWorkTaskRun
	{
		int Count { get; }
	}
}
