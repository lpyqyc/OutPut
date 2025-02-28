using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace PsMachineTools.Tools
{
	/// <summary>支持多线程的异步操作集合
	/// 普通的 List 集合或者其它集合，不支持多个线程条件下的增删操作，可以使用此集合实现
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SynCollection<T> : IEnumerable<T>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged where T : class
	{
		private List<T> list;

		protected ReaderWriterLockSlim m_Lock = new ReaderWriterLockSlim();

		private T _SelectedItem;

		/// <summary>成员总数
		/// </summary>
		public int Count
		{
			get
			{
				try
				{
					m_Lock.EnterReadLock();
					return list.Count;
				}
				finally
				{
					m_Lock.ExitReadLock();
				}
			}
		}

		/// <summary>获取索引值
		/// </summary>
		/// <param name="index">索引序号，从0开始</param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				try
				{
					m_Lock.EnterReadLock();
					return list[index];
				}
				finally
				{
					m_Lock.ExitReadLock();
				}
			}
		}

		/// <summary>选择成员
		/// </summary>
		public T SelectedItem
		{
			get
			{
				return _SelectedItem;
			}
			set
			{
				bool myChanged = false;
				if ((value != null || _SelectedItem != null) && (value == null || _SelectedItem == null || !_SelectedItem.Equals(value)))
				{
					_SelectedItem = value;
					OnPropertyChanged("SelectedItem");
				}
			}
		}

		/// <summary>集合成员变更事件
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>集合属性字段变更事件
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>初始化一个异常集合
		/// </summary>
		public SynCollection()
		{
			list = new List<T>();
		}

		/// <summary>清除集合所有成员
		/// </summary>
		public void Clear()
		{
			try
			{
				m_Lock.EnterWriteLock();
				mdsClear();
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		protected void mdsClear()
		{
			list.Clear();
			SelectedItem = null;
			this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			OnPropertyChanged("Count");
		}

		/// <summary>添加新成员
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			try
			{
				m_Lock.EnterWriteLock();
				mdsAdd(item);
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		/// <summary>添加一个新成员,此函数不支持多线程锁定，所以调用方必须自行确保多线程安全
		/// </summary>
		/// <param name="item"></param>
		protected void mdsAdd(T item)
		{
			list.Add(item);
			int myIndex = list.Count - 1;
			this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, myIndex));
			OnPropertyChanged("Count");
			SelectedItem = item;
		}

		/// <summary>移除旧成员
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Remove(T item)
		{
			try
			{
				m_Lock.EnterWriteLock();
				return mdsRemove(item);
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		/// <summary>移除旧成员
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected int mdsRemove(T item)
		{
			int myIndex = list.IndexOf(item);
			if (myIndex >= 0)
			{
				list.RemoveAt(myIndex);
				this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, myIndex));
				OnPropertyChanged("Count");
				if (SelectedItem == item)
				{
					SelectedItem = null;
				}
				return myIndex;
			}
			return -1;
		}

		/// <summary>从集合中移除指定索引处的成员，索引从0开始
		/// </summary>
		/// <param name="myIndex"></param>
		public void RemoveAt(int myIndex)
		{
			try
			{
				m_Lock.EnterWriteLock();
				mdsRemoveAt(myIndex);
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		protected void mdsRemoveAt(int myIndex)
		{
			T item = list[myIndex];
			list.RemoveAt(myIndex);
			this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, myIndex));
			OnPropertyChanged("Count");
			if (SelectedItem == item)
			{
				SelectedItem = null;
			}
		}

		/// <summary>触发属性变更事件
		/// </summary>
		/// <param name="propertyName"></param>
		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>由于集合ForEach 访问不支持多线程，在访问前应该拷贝一个副本出来
		/// </summary>
		/// <returns></returns>
		public List<T> CloneList()
		{
			try
			{
				m_Lock.EnterReadLock();
				return list.ToList();
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		/// <summary>注意，此集合不是多线程安全的，在Foreach 时会创建一个副本
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			try
			{
				m_Lock.EnterReadLock();
				return list.ToList().GetEnumerator();
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		/// <summary>注意，此集合不是多线程安全的，在Foreach 时会创建一个副本
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			try
			{
				m_Lock.EnterReadLock();
				return list.ToList().GetEnumerator();
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}
	}
}
