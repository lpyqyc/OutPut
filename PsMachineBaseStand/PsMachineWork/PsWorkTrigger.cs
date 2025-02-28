using System;
using System.ComponentModel;
using System.Threading;
using PsMachineBaseHy;
using PsMachineWork.触发器.接口;

namespace PsMachineWork
{
	/// <summary>触发器
	/// </summary>
	public abstract class PsWorkTrigger<Tstation, Ttrigger, TtrigerCondition> : iWorkTrigger, INotifyPropertyChanged where Tstation : iWorkStation where Ttrigger : PsWorkTrigger<Tstation, Ttrigger, TtrigerCondition> where TtrigerCondition : PsWorkTriggerCondition<Tstation, Ttrigger, TtrigerCondition>
	{
		private string _Para_TaskID;

		private int _Para_TimeDelay = 100;

		private bool _IsEnabled;

		private DateTime? _LastRunTime;

		private string _RunDesc;

		/// <summary>工作流启动时，默认的TaskID，如果为空，将会按照默认ID申请
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

		/// <summary>工作流的处理时间延时
		/// </summary>
		public int Para_TimeDelay
		{
			get
			{
				return _Para_TimeDelay;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (_Para_TimeDelay != value)
				{
					_Para_TimeDelay = value;
					OnPropertyChanged("Para_TimeDelay");
				}
			}
		}

		/// <summary>所属的上一级工作站
		/// </summary>
		public Tstation ParentStation { get; set; }

		/// <summary>所属的上一级工作站
		/// </summary>
		iWorkStation iWorkTrigger.ParentStation => ParentStation;

		/// <summary>包含的触发条件
		/// </summary>
		public PsWorkTriggerConditionCollection<Ttrigger, TtrigerCondition> ConditionCollection { get; }

		/// <summary>包含的触发条件
		/// </summary>
		iWorkTriggerConditionCollection<iWorkTriggerCondition> iWorkTrigger.ConditionCollection => ConditionCollection;

		/// <summary>运行日志
		/// </summary>
		public LogManager LogManager { get; }

		/// <summary>运行日志
		/// </summary>
		ILogManager iWorkTrigger.LogManager => LogManager;

		/// <summary>工作步骤的编码
		/// </summary>
		public string Code { get; set; }

		/// <summary>描述
		/// </summary>
		public string Desc { get; set; }

		/// <summary>启用或禁用触发器
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return _IsEnabled;
			}
			protected internal set
			{
				if (_IsEnabled != value)
				{
					_IsEnabled = value;
					OnPropertyChanged("IsEnabled");
				}
			}
		}

		/// <summary>最后执行时间
		/// </summary>
		public DateTime? LastRunTime
		{
			get
			{
				return _LastRunTime;
			}
			set
			{
				if (_LastRunTime != value)
				{
					_LastRunTime = value;
					OnPropertyChanged("LastRunTime");
				}
			}
		}

		/// <summary>执行描述
		/// </summary>
		public string RunDesc
		{
			get
			{
				return _RunDesc;
			}
			set
			{
				if (_RunDesc != value)
				{
					_RunDesc = value;
					OnPropertyChanged("RunDesc");
				}
			}
		}

		public virtual bool Can_Action_Enabled_True => !IsEnabled;

		public virtual bool Can_Action_Enabled_False => IsEnabled;

		public event PropertyChangedEventHandler PropertyChanged;

		public PsWorkTrigger(Tstation myParentStation, string myCode)
		{
			Code = myCode;
			IsEnabled = true;
			ParentStation = myParentStation;
			ConditionCollection = new PsWorkTriggerConditionCollection<Ttrigger, TtrigerCondition>((Ttrigger)this);
			LogManager = new LogManager("Trigger-" + myCode);
			PropertyChanged += Trig_PropertyChanged;
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>加载数据结构
		/// 如果加载过程发生异常要直接抛出，应用程序需要终止程序继续执行
		/// </summary>
		public void Init_LoadData()
		{
			mdsInitData();
		}

		/// <summary>启动工作对象自检Task
		/// </summary>
		public void Init_StarTask()
		{
			mdsInit_FlowTask();
		}

		/// <summary>启动禁用触发器
		/// </summary>
		/// <param name="myEnabled"></param>
		public virtual void Action_Enabled(bool myEnabled)
		{
			IsEnabled = myEnabled;
		}

		/// <summary>执行触发器的命令
		/// </summary>
		public abstract void RunTrigger();

		/// <summary>加载基础数据, 比如有哪些工作步骤,哪些启动触发条件,哪些创建触发条件
		/// 这个在创建流程和执行流程应该是一样的
		/// </summary>
		protected abstract void mdsInitData();

		/// <summary>加载触发器的执行Task
		/// </summary>
		protected virtual void mdsInit_FlowTask()
		{
			string myTaskID = Para_TaskID;
			if (string.IsNullOrEmpty(myTaskID))
			{
				myTaskID = "Trig " + ParentStation.Code + "." + Code;
			}
			TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID, Desc, (CancellationToken myCancel) => mdsTaskRun_Work(myCancel), Para_TimeDelay);
		}

		/// <summary>线程入口,执行工作流,注意不要再次执行时返回 false
		/// </summary>
		/// <param name="myCancellationToken"></param>
		protected bool mdsTaskRun_Work(CancellationToken myCancellationToken)
		{
			if (!IsEnabled)
			{
				return true;
			}
			if (ParentStation.IsClose)
			{
				return false;
			}
			if (mdsCheckAllowStar(myCancellationToken))
			{
				LastRunTime = DateTime.Now;
				try
				{
					RunTrigger();
					RunDesc = null;
				}
				catch (Exception ex)
				{
					RunDesc = ex.Message;
				}
			}
			return true;
		}

		/// <summary>检查工作流是否需要触发启动, 
		/// 如果允许触发启动返回True
		/// 如果已经启动或者停止，将不会再次触发启动
		/// </summary>
		private bool mdsCheckAllowStar(CancellationToken myCancellationToken)
		{
			bool myIsCheckOk = true;
			try
			{
				foreach (TtrigerCondition myCondition in ConditionCollection)
				{
					if (myCancellationToken.IsCancellationRequested)
					{
						myIsCheckOk = false;
						return false;
					}
					bool myIsCheckOk2 = myCondition.mdsRun();
					myIsCheckOk = myIsCheckOk && myIsCheckOk2;
				}
				return myIsCheckOk;
			}
			catch
			{
				return false;
			}
		}

		[StationAction("启用", CriteriaProperty = "Can_Action_Enabled_True")]
		public void Action_Enabled_True()
		{
			IsEnabled = true;
		}

		[StationAction("禁用", CriteriaProperty = "Can_Action_Enabled_False")]
		public void Action_Enabled_False()
		{
			IsEnabled = false;
		}

		protected virtual void Trig_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsEnabled")
			{
				OnPropertyChanged("Can_Action_Enabled_True");
				OnPropertyChanged("Can_Action_Enabled_False");
			}
		}

		/// <summary>添加当前类型所有标记为 TriggerConditionAttribute 的函数为条件
		/// </summary>
		protected void mdsInitAttributeCondition<TCondition>(int myStarIndex) where TCondition : TtrigerCondition
		{
			foreach (TriggerConditionAttrData myMethod in StationHelp.GetTriggerConditionAttributeList(GetType()))
			{
				string myCode = myStarIndex.ToString();
				myStarIndex++;
				TCondition obj = (TCondition)Activator.CreateInstance(typeof(TCondition), this, myCode);
				obj.Desc = myMethod.GetDesc();
				obj.InnerCode = myMethod.GetID();
			}
		}
	}
}
