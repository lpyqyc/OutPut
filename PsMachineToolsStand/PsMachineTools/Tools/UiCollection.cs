using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace PsMachineTools.Tools
{
	/// <summary>用于把多线程的集合，把集合变化，发送到UI主线程中工作
	/// 创建此对象时，会读取当前线程为运行上下文，所以必须在UI线程中创建本对象
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UiCollection : IEnumerable, INotifyCollectionChanged
	{
		private SynchronizationContext _synchronizationContext;

		private IEnumerable list;

		private readonly List<NotifyCollectionChangedEventArgs> _EventList = new List<NotifyCollectionChangedEventArgs>();

		/// <summary>获取此集合创建时指定的上下文执行线程
		/// </summary>
		private SynchronizationContext SynchronizationContext
		{
			get
			{
				if (_synchronizationContext == null)
				{
					_synchronizationContext = SynchronizationContext.Current;
				}
				return _synchronizationContext;
			}
		}

		/// <summary>
		/// </summary>
		public IEnumerable DataSource
		{
			get
			{
				return list;
			}
			set
			{
				if (list != value)
				{
					mdsInitData(value);
				}
			}
		}

		/// <summary>集合成员变更事件
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>初始化一个新的UI展示集合，并读取当前上下文
		/// </summary>
		public UiCollection()
		{
			_synchronizationContext = SynchronizationContext.Current;
		}

		/// <summary>初始化一个新的UI展示集合，并读取当前上下文，以指定对象作为数据源，
		/// 注意，指定的数据源对象必须继承两个接口 IEnumerable / INotifyCollectionChanged
		/// </summary>
		/// <param name="mySource"></param>
		public UiCollection(IEnumerable mySource)
		{
			mdsInitData(mySource);
			_synchronizationContext = SynchronizationContext.Current;
		}

		private void mdsInitData(IEnumerable mySource)
		{
			if (list != null)
			{
				((INotifyCollectionChanged)list).CollectionChanged -= DataSource_CollectionChanged;
			}
			if (mySource != null)
			{
				if (mySource == null)
				{
					throw new Exception("指定数据源不支持 IEnumerable 接口");
				}
				if (!(mySource is INotifyCollectionChanged))
				{
					throw new Exception("指定数据源不支持 INotifyCollectionChanged 接口");
				}
				list = mySource;
				((INotifyCollectionChanged)mySource).CollectionChanged += DataSource_CollectionChanged;
				DataSource_CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null));
			}
		}

		private void DataSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			lock (_EventList)
			{
				_EventList.Add(e);
			}
			SynchronizationContext.Post(delegate
			{
				mdsUiRunEvent();
			}, null);
		}

		/// <summary>在UI线程执行的方法
		/// </summary>
		private void mdsUiRunEvent()
		{
			while (true)
			{
				NotifyCollectionChangedEventArgs myFirstEvent = null;
				lock (_EventList)
				{
					if (_EventList.Count > 0)
					{
						myFirstEvent = _EventList[0];
						_EventList.RemoveAt(0);
					}
				}
				if (myFirstEvent != null)
				{
					try
					{
						this.CollectionChanged?.Invoke(this, myFirstEvent);
					}
					catch
					{
					}
					continue;
				}
				break;
			}
		}

		/// <summary>注意，此集合不是多线程安全的，在Foreach 时会创建一个副本
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (list == null)
			{
				return new List<object>().GetEnumerator();
			}
			return list?.GetEnumerator();
		}
	}
}
