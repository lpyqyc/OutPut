using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using PsMachineTools.Tools;
using PsMachineWork;

namespace PsMachineBaseHy
{
	/// <summary>简易的日志输出管理器,集合类型，可以用来输出日志，支持多线程输出日志
	/// 默认最多输出1000条日志，超出部分会自动删除，可以通过 MaxLogCount 属性设置最多日志保存条数
	/// </summary>
	public class LogManager : IEnumerable<ILogData>, IEnumerable, INotifyPropertyChanged, ILogManager, INotifyCollectionChanged
	{
		private string m_LogFileName;

		/// <summary>日志数据
		/// </summary>
		private List<ILogData> m_LogDatas;

		/// <summary>单线程锁定 添加、移除、Foreach 成员时 Lock 此属性
		/// 外部对象如果需要 Foreach 对象，使用此属性时，一定要自行确保及时释放
		/// 一般情况下不推荐外部对象使用此属性锁定对象
		/// </summary>
		public readonly object LockItemsChanged = new object();

		private LogData _SelectedItem;

		private Dictionary<string, clsPeriodLog> m_间隔性输出日志 = new Dictionary<string, clsPeriodLog>();

		/// <summary>最大的日志条数,默认1000条,超过时会移除最早添加的日志
		/// </summary>
		public int MaxLogCount { get; set; } = 1000;


		/// <summary>已经存储的日志条数
		/// </summary>
		public int Count => m_LogDatas.Count;

		/// <summary>当前选中的日志行用于界面中滚动
		/// </summary>
		public LogData SelectedItem
		{
			get
			{
				return _SelectedItem;
			}
			set
			{
				if (_SelectedItem != value)
				{
					_SelectedItem = value;
					OnPropertyChanged("SelectedItem");
				}
			}
		}

		/// <summary>属性变更事件
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>集合成员变更事件
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>初始化新的日志输出集合
		/// </summary>
		/// <param name="myLogFileName">输出的日志文件名称，必须要符合文件名规则，不包含文件夹路径</param>
		public LogManager(string myLogFileName)
		{
			m_LogDatas = new List<ILogData>();
			m_LogFileName = myLogFileName;
			if (string.IsNullOrEmpty(m_LogFileName))
			{
				m_LogFileName = "Log";
			}
		}

		/// <summary>添加新的日志行成员
		/// </summary>
		/// <param name="item"></param>
		public void Add(ILogData item)
		{
			lock (LockItemsChanged)
			{
				int myIndex = 0;
				m_LogDatas.Insert(myIndex, item);
				this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, myIndex));
				if (item != null)
				{
					FileLogWriteHelp.WriteLine(m_LogFileName, item.ToString());
				}
			}
		}

		/// <summary>删除多余的成员
		/// </summary>
		private void mdsRemoveSurplusItem()
		{
			lock (LockItemsChanged)
			{
				while (Count > MaxLogCount)
				{
					m_LogDatas.RemoveAt(MaxLogCount);
				}
			}
		}

		/// <summary>添加新的日志行
		/// </summary>
		/// <param name="myMsg">日志消息</param>
		/// <param name="myIsException">是否异常日志</param>
		public void AddMachineLog(string myMsg, bool myIsException = false)
		{
			LogData myLog = new LogData
			{
				Message = myMsg,
				IsException = myIsException
			};
			mdsRemoveSurplusItem();
			Add(myLog);
			SelectedItem = myLog;
		}

		/// <summary>添加新的日志行
		/// </summary>
		/// <param name="myMsg">日志消息</param>
		/// <param name="myDesc">日志附加详细说明</param>
		/// <param name="myIsException">是否异常日志</param>
		public void AddMachineLog(string myMsg, string myDesc, bool myIsException = false)
		{
			LogData myLog = new LogData
			{
				Message = myMsg,
				Desc = myDesc,
				IsException = myIsException
			};
			mdsRemoveSurplusItem();
			Add(myLog);
			SelectedItem = myLog;
		}

		/// <summary>添加新的日志行,输出异常消息
		/// </summary>
		/// <param name="ex"></param>
		public void AddMachineLog(Exception ex)
		{
			LogData myLog = new LogData
			{
				Message = ex?.Message,
				IsException = true,
				Desc = HyExpFunction.GetErrMsg(ex)
			};
			mdsRemoveSurplusItem();
			Add(myLog);
			SelectedItem = myLog;
		}

		/// <summary>
		/// </summary>
		/// <param name="ex"></param>
		public void AddMachineLogExceptionDesc(Exception ex)
		{
			LogData myLog = new LogData
			{
				Message = ex?.Message,
				IsException = true,
				Desc = ex?.StackTrace
			};
			mdsRemoveSurplusItem();
			Add(myLog);
			SelectedItem = myLog;
		}

		/// <summary>防重复输出消息
		/// </summary>
		/// <param name="myMsg"></param>
		/// <param name="myMsgDesc"></param>
		/// <param name="myIsException"></param>
		/// <param name="myMsgKey">消息的主键</param>
		/// <param name="myTimeOut">限制的时间周期，以毫秒为单位</param>
		public void AddMachineKeyLog(string myMsg, string myMsgDesc, bool myIsException, string myMsgKey, int myTimeOut)
		{
			bool myAllowRun = false;
			lock (m_间隔性输出日志)
			{
				DateTime my时间上限 = DateTime.Now;
				DateTime my时间下限 = my时间上限.AddMilliseconds(-myTimeOut);
				clsPeriodLog myFindItem = null;
				try
				{
					myFindItem = m_间隔性输出日志[myMsgKey];
				}
				catch
				{
				}
				if (myFindItem == null)
				{
					myAllowRun = true;
					myFindItem = new clsPeriodLog
					{
						Message = myMsg,
						LastOutTime = DateTime.Now
					};
					m_间隔性输出日志.Add(myMsgKey, myFindItem);
				}
				else if (myFindItem.LastOutTime <= my时间下限 || myFindItem.LastOutTime > my时间上限)
				{
					myAllowRun = true;
					myFindItem.LastOutTime = DateTime.Now;
				}
			}
			if (myAllowRun)
			{
				AddMachineLog(myMsg, myMsgDesc, myIsException);
			}
		}

		/// <summary>触发属性变更事件
		/// </summary>
		/// <param name="myPropertyName"></param>
		public void OnPropertyChanged(string myPropertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(myPropertyName));
		}

		/// <summary>获取枚举对象，如果在些期间集合有变更，仅维持变更前状态
		/// </summary>
		/// <returns></returns>
		public IEnumerator<ILogData> GetEnumerator()
		{
			lock (LockItemsChanged)
			{
				return m_LogDatas.ToList().GetEnumerator();
			}
		}

		/// <summary>获取枚举对象，如果在些期间集合有变更，仅维持变更前状态
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			lock (LockItemsChanged)
			{
				return m_LogDatas.ToList().GetEnumerator();
			}
		}
	}
}
