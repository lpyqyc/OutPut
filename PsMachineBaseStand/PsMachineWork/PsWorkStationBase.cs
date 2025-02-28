using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PsMachineBaseHy;

namespace PsMachineWork
{
	/// <summary>工作站基础类, 常规的工作站可以从这里派生
	/// </summary>
	/// <typeparam name="TStation"></typeparam>
	/// <typeparam name="TPoint"></typeparam>
	public abstract class PsWorkStationBase<TStation, TPoint> : iWorkStation, INotifyPropertyChanged where TStation : PsWorkStationBase<TStation, TPoint> where TPoint : PsWorkStationPointBase<TStation, TPoint>
	{
		private string _Para_TaskID;

		private int _Para_RefreshTimeDelay = 1000;

		private int _Para_FlowTimeDelay = 50;

		private string _Code;

		private string _Desc;

		private bool _IsConnection;

		private bool _IsException;

		private bool _IsAutoConnection;

		private bool _IsClose;

		private bool m_IsFirstConnection;

		/// <summary>读写锁，读写点位时，部分工作站只能单线程读写
		/// </summary>
		protected object m_Lock_ReadWritePoint = new object();

		private bool _IsLog_WritePoint = true;

		private bool _IsLog_ReadPoint;

		/// <summary>刷新的线程ID
		/// </summary>
		public string Para_TaskID
		{
			get
			{
				return _Para_TaskID;
			}
			set
			{
				if (_Para_TaskID != value)
				{
					_Para_TaskID = value;
					OnPropertyChanged("Para_TaskID");
				}
			}
		}

		/// <summary>自动更新停顿时间, 毫秒为单位, 默认为 1000
		/// </summary>
		public int Para_RefreshTimeDelay
		{
			get
			{
				return _Para_RefreshTimeDelay;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (_Para_RefreshTimeDelay != value)
				{
					_Para_RefreshTimeDelay = value;
					OnPropertyChanged("Para_RefreshTimeDelay");
				}
			}
		}

		/// <summary>工作流的处理时间延时
		/// </summary>
		public int Para_FlowTimeDelay
		{
			get
			{
				return _Para_FlowTimeDelay;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (_Para_FlowTimeDelay != value)
				{
					_Para_FlowTimeDelay = value;
					OnPropertyChanged("Para_FlowTimeDelay");
				}
			}
		}

		/// <summary>所属的上一级工作站
		/// </summary>
		public iWorkStation ParentStation { get; set; }

		/// <summary>包含的子工作站,默认为空
		/// </summary>
		public StationCollection<iWorkStation> ChildsStaion { get; }

		/// <summary>包含的子工作站,默认为空
		/// </summary>
		iWorkStationCollection<iWorkStation> iWorkStation.ChildsStaion => ChildsStaion;

		/// <summary>包含的直属点位
		/// </summary>
		public PointCollection<TStation, TPoint> ChildsPoint { get; }

		/// <summary>包含的直属点位
		/// </summary>
		iStationPointCollection<iStationPoint> iWorkStation.ChildsPoint => ChildsPoint;

		/// <summary>包含的触发器集合
		/// </summary>
		public iWorkTriggerCollection<iWorkTrigger> ChildsTrigger { get; }

		/// <summary>包含的触发器集合
		/// </summary>
		iWorkTriggerCollection<iWorkTrigger> iWorkStation.ChildsTrigger => ChildsTrigger;

		/// <summary>包含的执行工作流
		/// </summary>
		public FlowCollection<iWorkFlow> ChildsWorkFlow { get; }

		/// <summary>包含的执行工作流
		/// </summary>
		iWorkFlowCollection<iWorkFlow> iWorkStation.ChildsWorkFlow => ChildsWorkFlow;

		/// <summary>运行日志
		/// </summary>
		public LogManager LogManager { get; }

		/// <summary>运行日志
		/// </summary>
		ILogManager iWorkStation.LogManager => LogManager;

		/// <summary>工作站本地编码
		/// </summary>
		public string Code
		{
			get
			{
				return _Code;
			}
			protected set
			{
				if (_Code != value)
				{
					_Code = value;
					OnPropertyChanged("Code");
				}
			}
		}

		/// <summary>注释说明
		/// </summary>
		public string Desc
		{
			get
			{
				return _Desc;
			}
			set
			{
				if (_Desc != value)
				{
					_Desc = value;
					OnPropertyChanged("Desc");
				}
			}
		}

		/// <summary>是否连接
		/// </summary>
		public bool IsConnection
		{
			get
			{
				return _IsConnection;
			}
			set
			{
				if (_IsConnection != value)
				{
					_IsConnection = value;
					OnPropertyChanged("IsConnection");
				}
			}
		}

		/// <summary>连接异常
		/// </summary>
		public bool IsException
		{
			get
			{
				return _IsException;
			}
			set
			{
				if (_IsException != value)
				{
					_IsException = value;
					OnPropertyChanged("IsException");
				}
			}
		}

		/// <summary>是否支持断线后自动重新连接
		/// </summary>
		public bool IsAutoConnection
		{
			get
			{
				return _IsAutoConnection;
			}
			set
			{
				if (_IsAutoConnection != value)
				{
					_IsAutoConnection = value;
					OnPropertyChanged("IsAutoConnection");
				}
			}
		}

		/// <summary>工作站是否已经关闭并释放资源
		/// </summary>
		public bool IsClose
		{
			get
			{
				return _IsClose;
			}
			set
			{
				if (_IsClose != value)
				{
					_IsClose = value;
					OnPropertyChanged("IsClose");
				}
			}
		}

		/// <summary>写入点位时，输出日志
		/// </summary>
		public bool IsLog_WritePoint
		{
			get
			{
				return _IsLog_WritePoint;
			}
			set
			{
				if (_IsLog_WritePoint != value)
				{
					_IsLog_WritePoint = value;
					OnPropertyChanged("IsLog_WritePoint");
				}
			}
		}

		/// <summary>读取点位时，输出日志
		/// </summary>
		public bool IsLog_ReadPoint
		{
			get
			{
				return _IsLog_ReadPoint;
			}
			set
			{
				if (_IsLog_ReadPoint != value)
				{
					_IsLog_ReadPoint = value;
					OnPropertyChanged("IsLog_ReadPoint");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>扫描完点位值后事件
		/// </summary>
		public event EventHandler<EventArgs> ScanPointEndEvent;

		public PsWorkStationBase(string myCode)
		{
			Code = myCode;
			ChildsStaion = new StationCollection<iWorkStation>(this);
			ChildsPoint = new PointCollection<TStation, TPoint>((TStation)this);
			ChildsTrigger = new PsWorkTiggerCollection<iWorkTrigger>(this);
			ChildsWorkFlow = new FlowCollection<iWorkFlow>(this);
			LogManager = new LogManager("Station-" + myCode);
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void Close(bool myAllChildsStation = true)
		{
			mdsCloseTask();
			mdsCloseConnection();
			mdsCloseData();
			if (!myAllChildsStation || ChildsStaion == null)
			{
				return;
			}
			foreach (iWorkStation item in ChildsStaion)
			{
				item.Close(myAllChildsStation);
			}
		}

		/// <summary>初始化或者加载数据
		/// 添加点位,子工作站节点,本站位的触发式工作流集合, 
		/// </summary>
		public abstract void Init_LoadData();

		/// <summary>连接工作站及下属工作站
		/// </summary>
		public void Connection(CancellationToken myCancellationToken)
		{
			CancellationTokenSource myTokenSource = new CancellationTokenSource();
			try
			{
				mdsRunTask_AutoConnection(myTokenSource.Token);
			}
			catch
			{
			}
			foreach (iWorkStation item in ChildsStaion)
			{
				item.Connection(myTokenSource.Token);
			}
		}

		/// <summary>开始加载和工作
		/// </summary>
		public virtual void Init_StarTask()
		{
			mdsInit_ThisTask();
			foreach (iWorkStation item in ChildsStaion)
			{
				item.Init_StarTask();
			}
		}

		/// <summary>用指定的编码查找点位
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public TPoint FindPointByCode(string myCode)
		{
			return ChildsPoint.FirstOrDefault((TPoint p) => p.Code == myCode);
		}

		/// <summary>重置工作站连接
		/// 注意 IsConnection IsException 两个属性要正确配置执行后的结果
		/// </summary>
		protected abstract void mdsResetConnection(CancellationToken myCancellationToken);

		/// <summary>关闭工作站连接
		/// 注意 IsConnection IsException 两个属性要置为 false
		/// </summary>
		protected abstract void mdsCloseConnection();

		/// <summary>关闭工作站时释放数据
		/// 一般情况下,工作站关闭时,只需要关闭它的Task 进程即可
		/// 此方法一般不用写代码,但如果少数特殊的工作站,确实需要释放数据的,可以写在这里
		/// </summary>
		protected virtual void mdsCloseData()
		{
		}

		private void mdsCloseTask()
		{
			IsClose = true;
		}

		/// <summary>加载工作流的执行Task
		/// </summary>
		private void mdsInit_ThisTask()
		{
			string myTaskID2 = Para_TaskID;
			if (string.IsNullOrEmpty(Para_TaskID))
			{
				myTaskID2 = "Station " + Code;
			}
			TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID2, Desc, (CancellationToken myCancel) => mdsRunTask_AutoConnection(myCancel), Para_RefreshTimeDelay);
			TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID2, Desc, (CancellationToken myCancel) => mdsRunTask_ScanPoint(myCancel), Para_RefreshTimeDelay);
			TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID2, Desc, (CancellationToken myCancel) => mdsRunTask_ScanPointEndEvent(myCancel), Para_RefreshTimeDelay);
			foreach (iWorkTrigger item in ChildsTrigger)
			{
				item.Init_StarTask();
			}
			string myTaskID = "Station.FlowCollection " + Code;
			TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID, Desc, (CancellationToken myCancel) => mdsRunTask_FlowCollection(myCancel), Para_FlowTimeDelay);
			TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID, Desc, (CancellationToken myCancel) => mdsRunTask_CheckStarFlow(myCancel), Para_FlowTimeDelay);
		}

		/// <summary>线程请求函数的回调方法,用于检查是否需要首次连接,或者断线重新连接
		/// 用这个方法的优点,是工作站不管是首次还是以后,出异常后可以断线重新连接
		/// </summary>
		/// <param name="myCancellationToken"></param>
		/// <returns></returns>
		private bool mdsRunTask_AutoConnection(CancellationToken myCancellationToken)
		{
			if (IsClose)
			{
				return false;
			}
			if (!m_IsFirstConnection && !IsConnection)
			{
				try
				{
					mdsResetConnection(myCancellationToken);
					LogManager.AddMachineKeyLog("连接成功", null, myIsException: false, "连接成功", 1000);
				}
				catch (Exception ex2)
				{
					IsException = true;
					string myExcMsg2 = "连接异常:" + ex2.Message;
					LogManager.AddMachineKeyLog(myExcMsg2, null, myIsException: true, "ConnectionException", 2000);
				}
				if (IsConnection)
				{
					m_IsFirstConnection = true;
				}
			}
			if (m_IsFirstConnection && IsAutoConnection && (!IsConnection || IsException))
			{
				try
				{
					mdsResetConnection(myCancellationToken);
				}
				catch (Exception ex)
				{
					IsException = true;
					string myExcMsg = "重新连接异常:" + ex.Message;
					LogManager.AddMachineKeyLog(myExcMsg, null, myIsException: true, "ConnectionException", 2000);
				}
			}
			return true;
		}

		/// <summary>线程请求函数的回调方法,用于检查各个工作流是否已经结束
		/// </summary>
		/// <param name="myCancellationToken"></param>
		/// <returns></returns>
		private bool mdsRunTask_FlowCollection(CancellationToken myCancellationToken)
		{
			if (IsClose)
			{
				return false;
			}
			foreach (iWorkFlow myFlow in ChildsWorkFlow.CloneList())
			{
				if (myFlow.IsStop)
				{
					ChildsWorkFlow.Remove(myFlow);
				}
			}
			return true;
		}

		/// <summary>线程请求函数的回调方法,开始自动扫描各点位值
		/// </summary>
		/// <param name="myCancellationToken"></param>
		/// <returns></returns>
		private bool mdsRunTask_ScanPoint(CancellationToken myCancellationToken)
		{
			if (IsClose)
			{
				return false;
			}
			mdsRunTask_ScanPoint2(myCancellationToken);
			return true;
		}

		/// <summary>自动刷新各点位值，允许派生类中重写, 针对那些可能点位特别多，支持批量更新的
		/// </summary>
		/// <param name="myCancellationToken"></param>
		/// <returns></returns>
		protected virtual bool mdsRunTask_ScanPoint2(CancellationToken myCancellationToken)
		{
			foreach (TPoint myPoint in ChildsPoint)
			{
				if (myCancellationToken.IsCancellationRequested)
				{
					break;
				}
				if (myPoint.IsAutoRefresh && (myPoint.ReadWriteType == emPointReadWriteType.ReadWrite || myPoint.ReadWriteType == emPointReadWriteType.ReadOnly))
				{
					try
					{
						myPoint.ReadValueObject();
					}
					catch (Exception)
					{
						Task.Delay(15).Wait();
					}
				}
			}
			return true;
		}

		/// <summary>扫描完点位后追加处理事件
		/// </summary>
		/// <param name="myCancellationToken"></param>
		/// <returns></returns>
		private bool mdsRunTask_ScanPointEndEvent(CancellationToken myCancellationToken)
		{
			if (IsClose)
			{
				return false;
			}
			this.ScanPointEndEvent?.Invoke(this, new EventArgs());
			return true;
		}

		/// <summary>检查每一个 Flow 的 Task 是否已经启动,没有启动时,额外启动
		/// </summary>
		/// <param name="myCancellationToken"></param>
		/// <returns></returns>
		private bool mdsRunTask_CheckStarFlow(CancellationToken myCancellationToken)
		{
			if (IsClose)
			{
				return false;
			}
			foreach (iWorkFlow myFlow in ChildsWorkFlow)
			{
				if (!myFlow.IsStarFlowTask)
				{
					myFlow.Init_StarTask();
				}
			}
			return true;
		}

		/// <summary>添加当前类型所有标记为 TriggerConditionAttribute 的函数为条件
		/// </summary>
		protected void mdsInitAttributePoint()
		{
			foreach (StationPointAttrData myProperty in StationHelp.GetStationPointAttributeList(GetType()))
			{
				string myCode = myProperty.Attribute.Code;
				if (string.IsNullOrEmpty(myCode))
				{
					myCode = myProperty.PropertyInfo.Name;
				}
				Type myCreateType = myProperty.PropertyInfo.PropertyType;
				if (myProperty.Attribute.CreateType != null)
				{
					myCreateType = myProperty.Attribute.CreateType;
				}
				if (!typeof(TPoint).IsAssignableFrom(myCreateType))
				{
					throw new Exception("创建点位类型[" + myCreateType.Name + "] 与当前工作站指定的点位类型[" + typeof(TPoint).Name + "] 不匹配");
				}
				TPoint myPoint = null;
				myPoint = (TPoint)Activator.CreateInstance(myCreateType, this, myCode);
				myPoint.Desc = myProperty.Attribute.Desc;
				myPoint.ReadWriteType = myProperty.Attribute.ReadWriteType;
				myPoint.IsAutoRefresh = myProperty.Attribute.IsAutoRefresh;
				myPoint.IsLocalDataPoint = myProperty.Attribute.IsLocalDataPoint;
				myProperty.PropertyInfo.SetValue(this, myPoint, null);
			}
		}
	}
}
