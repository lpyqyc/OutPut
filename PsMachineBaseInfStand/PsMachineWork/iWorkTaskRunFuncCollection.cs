using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>执行函数集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface iWorkTaskRunFuncCollection<out T> : IEnumerable<T>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		int Count { get; }
	}
}
