using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace PsMachineTools.Tools
{
	/// <summary>旧的多线程集合,支持多线写入，但不支持集成成员变更属性,不适用于UI集合对象输出
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SynList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		private List<T> m_List;

		protected ReaderWriterLockSlim m_Lock;

		public T this[int index]
		{
			get
			{
				try
				{
					m_Lock.EnterReadLock();
					return m_List[index];
				}
				finally
				{
					m_Lock.ExitReadLock();
				}
			}
			set
			{
				try
				{
					m_Lock.EnterWriteLock();
					m_List[index] = value;
				}
				finally
				{
					m_Lock.ExitWriteLock();
				}
			}
		}

		public int Count
		{
			get
			{
				try
				{
					m_Lock.EnterReadLock();
					return m_List.Count;
				}
				finally
				{
					m_Lock.ExitReadLock();
				}
			}
		}

		public int SynCount => m_List.Count;

		public bool IsReadOnly => false;

		/// <summary>成员列表变更事件
		/// </summary>
		public event EventHandler ListChanged;

		public SynList()
		{
			m_List = new List<T>();
			m_Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		/// <summary>当遍历读取List前加写锁定
		/// 注意读取列表完后一定要调用 ListLockStop 方法
		/// </summary>
		public void ListWriteLockBegin()
		{
			m_Lock.EnterWriteLock();
		}

		/// <summary>当遍历读取List后, 解除写锁定
		/// </summary>
		public void ListWriteLockStop()
		{
			m_Lock.ExitWriteLock();
		}

		public void Add(T item)
		{
			try
			{
				m_Lock.EnterWriteLock();
				m_List.Add(item);
				OnListChanged();
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		public void Clear()
		{
			try
			{
				m_Lock.EnterWriteLock();
				m_List.Clear();
				OnListChanged();
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		public bool Contains(T item)
		{
			try
			{
				m_Lock.EnterReadLock();
				return m_List.Contains(item);
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			try
			{
				m_Lock.EnterReadLock();
				m_List.CopyTo(array, arrayIndex);
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			try
			{
				m_Lock.EnterReadLock();
				return m_List.GetEnumerator();
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		public int IndexOf(T item)
		{
			try
			{
				m_Lock.EnterReadLock();
				return m_List.IndexOf(item);
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		public void Insert(int index, T item)
		{
			try
			{
				m_Lock.EnterWriteLock();
				m_List.Insert(index, item);
				OnListChanged();
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		public bool Remove(T item)
		{
			try
			{
				m_Lock.EnterWriteLock();
				bool result = m_List.Remove(item);
				OnListChanged();
				return result;
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		public void RemoveAt(int index)
		{
			try
			{
				m_Lock.EnterWriteLock();
				m_List.RemoveAt(index);
				OnListChanged();
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			try
			{
				m_Lock.EnterReadLock();
				return m_List.GetEnumerator();
			}
			finally
			{
				m_Lock.ExitReadLock();
			}
		}

		/// <summary>把一个成员移到最前面, 如果成员不在当前集合中，不会处理
		/// </summary>
		/// <param name="item"></param>
		public void MoveItemToFirst(T item)
		{
			try
			{
				m_Lock.EnterWriteLock();
				if (m_List.IndexOf(item) <= 0 || m_List.Count <= 1)
				{
					return;
				}
				m_List.Remove(item);
				m_List.Insert(0, item);
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
			OnListChanged();
		}

		protected void OnListChanged()
		{
			this.ListChanged?.Invoke(this, new EventArgs());
		}
	}
}
