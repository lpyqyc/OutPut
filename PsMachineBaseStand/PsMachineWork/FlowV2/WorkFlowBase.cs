using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using PsMachineBaseHy;

namespace PsMachineWork.FlowV2
{
	/// <summary>工作流基础类，用于从此类派生
	/// </summary>
	/// <typeparam name="TStep">流程步骤的类</typeparam>
	/// <typeparam name="TStarCondition">流程启动条件的类</typeparam>
	/// <typeparam name="TCreateCondition">自动创建流程的类</typeparam>
	public abstract class WorkFlowBase<TStation, TFlow, TStep, TStarCondition> : iWorkFlow, INotifyPropertyChanged where TStation : iWorkStation where TFlow : WorkFlowBase<TStation, TFlow, TStep, TStarCondition> where TStep : WorkFlowStep<TStation, TFlow, TStep, TStarCondition> where TStarCondition : WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition>
	{
		private string _Para_TaskID;

		private int _Para_TimeDelay = 100;

		private bool _IsStart;

		private string _RunDesc;

		private bool _IsStop;

		private DateTime _CreateDate;

		private DateTime? _StarDate;

		private iWorkFlowStep _SelectStep;

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

		/// <summary>所在的工作站
		/// </summary>
		public TStation ParentStation { get; set; }

		/// <summary>所在的工作站
		/// </summary>
		iWorkStation iWorkFlow.ParentStation => ParentStation;

		/// <summary>工作步骤
		/// </summary>
		public FlowStepCollection<TFlow, TStep> Steps { get; }

		/// <summary>工作步骤
		/// </summary>
		iWorkFlowStepCollection<iWorkFlowStep> iWorkFlow.Steps => Steps;

		/// <summary>工作流启动时的限制条件
		/// </summary>
		public WorkFlowStarConditionCollection<TFlow, TStarCondition> FlowStarConditionList { get; }

		/// <summary>工作流启动时的限制条件
		/// </summary>
		iWorkFlowStarConditionCollection<iWorkFlowStarCondition> iWorkFlow.FlowStarConditionList => FlowStarConditionList;

		/// <summary>异常处理步骤
		/// </summary>
		public PsWorkFlowStepExceptionCollection<TFlow, PsWorkFlowStepException> StepExceptionList { get; }

		/// <summary>异常处理步骤
		/// </summary>
		iWorkFlowStepExceptionCollection<iWorkFlowStepException> iWorkFlow.StepExceptionList => StepExceptionList;

		/// <summary>引用的工作站列表
		/// </summary>
		public FlowRefStationCollection<TFlow, WorkFlowRefStation> RefStationList { get; }

		/// <summary>引用的工作站列表
		/// </summary>
		iWorkFlowRefStationCollection<iWorkFlowRefStation> iWorkFlow.RefStationList => RefStationList;

		/// <summary>运行日志
		/// </summary>
		public LogManager LogManager { get; }

		/// <summary>运行日志
		/// </summary>
		ILogManager iWorkFlow.LogManager => LogManager;

		/// <summary>起始第1步
		/// </summary>
		public TStep StarStep { get; set; }

		/// <summary>编码
		/// </summary>
		public string Code { get; protected set; }

		/// <summary>注释
		/// </summary>
		public string Desc { get; set; }

		/// <summary>流程已经启动
		/// </summary>
		public bool IsStart
		{
			get
			{
				return _IsStart;
			}
			set
			{
				if (_IsStart != value)
				{
					_IsStart = value;
					OnPropertyChanged("IsStart");
				}
			}
		}

		/// <summary>运行描述
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

		/// <summary>流程已经结束或者取消
		/// </summary>
		public bool IsStop
		{
			get
			{
				return _IsStop;
			}
			set
			{
				if (_IsStop != value)
				{
					_IsStop = value;
					OnPropertyChanged("IsStop");
				}
			}
		}

		/// <summary>创建时间
		/// </summary>
		public DateTime CreateDate
		{
			get
			{
				return _CreateDate;
			}
			set
			{
				if (_CreateDate != value)
				{
					_CreateDate = value;
					OnPropertyChanged("CreateDate");
				}
			}
		}

		/// <summary>启动时间
		/// </summary>
		public DateTime? StarDate
		{
			get
			{
				return _StarDate;
			}
			set
			{
				if (_StarDate != value)
				{
					_StarDate = value;
					OnPropertyChanged("StarDate");
				}
			}
		}

		/// <summary>选择的步骤，用于在界面绑定时，根据执行步骤跳到不同的步骤界面
		/// </summary>
		public iWorkFlowStep SelectStep
		{
			get
			{
				return _SelectStep;
			}
			set
			{
				if (_SelectStep != value)
				{
					_SelectStep = value;
					OnPropertyChanged("SelectStep");
				}
			}
		}

		/// <summary>是否已经启动事务流程事务
		/// </summary>
		public bool IsStarFlowTask { get; protected internal set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public WorkFlowBase(TStation myStation, string myCode)
		{
			Code = myCode;
			ParentStation = myStation;
			Steps = new FlowStepCollection<TFlow, TStep>((TFlow)this);
			FlowStarConditionList = new WorkFlowStarConditionCollection<TFlow, TStarCondition>((TFlow)this);
			StepExceptionList = new PsWorkFlowStepExceptionCollection<TFlow, PsWorkFlowStepException>((TFlow)this);
			RefStationList = new FlowRefStationCollection<TFlow, WorkFlowRefStation>((TFlow)this);
			LogManager = new LogManager("Flow-" + myCode);
			CreateDate = DateTime.Now;
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

		/// <summary>流程结束或者强制结束
		/// </summary>
		public virtual void Action_Stop()
		{
			mdsFlowStop();
		}

		/// <summary>加载基础数据, 比如有哪些工作步骤,哪些启动触发条件,哪些创建触发条件
		/// 这个在创建流程和执行流程应该是一样的
		/// </summary>
		protected abstract void mdsInitData();

		/// <summary>加载工作流的执行Task
		/// </summary>
		protected void mdsInit_FlowTask()
		{
			if (!IsStarFlowTask)
			{
				string myTaskID = Para_TaskID;
				if (string.IsNullOrEmpty(myTaskID))
				{
					myTaskID = "Flow " + ParentStation.Code + "." + Code;
				}
				TaskManagerProvider.DefaultTaskManager.CreateTask(myTaskID, Desc, (CancellationToken myCancel) => mdsTaskRun_FlowWork(myCancel), Para_TimeDelay);
				IsStarFlowTask = true;
			}
		}

		/// <summary>线程入口,执行工作流,注意不要再次执行时返回 false
		/// </summary>
		/// <param name="myCancellationToken"></param>
		protected bool mdsTaskRun_FlowWork(CancellationToken myCancellationToken)
		{
			if (IsStop)
			{
				return false;
			}
			if (mdsCheckAllowStar(myCancellationToken))
			{
				IsStart = true;
				TStep myStarStep = null;
				myStarStep = StarStep;
				if (myStarStep == null)
				{
					myStarStep = Steps[0];
				}
				if (myStarStep != null)
				{
					myStarStep.IsStart = true;
				}
			}
			mdsRunStep(myCancellationToken);
			return true;
		}

		/// <summary>流程结束,这是真正的执行代码处, Action_Stop 只是标记结束
		/// </summary>
		protected internal virtual void mdsFlowStop()
		{
			IsStop = true;
		}

		/// <summary>检查工作流是否需要触发启动, 
		/// 如果允许触发启动返回True
		/// 如果已经启动或者停止，将不会再次触发启动
		/// </summary>
		private bool mdsCheckAllowStar(CancellationToken myCancellationToken)
		{
			if (IsStart || IsStop)
			{
				return false;
			}
			bool myIsCheckOk = true;
			try
			{
				foreach (TStarCondition myCondition in FlowStarConditionList)
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

		/// <summary>当任务已经启动时，扫描并执行各步骤
		/// </summary>
		private void mdsRunStep(CancellationToken myCancellationToken)
		{
			if (myCancellationToken.IsCancellationRequested || !IsStart || IsStop)
			{
				return;
			}
			foreach (TStep myStep in Steps)
			{
				if (myCancellationToken.IsCancellationRequested || IsStop)
				{
					break;
				}
				myStep.mdsRun();
			}
		}

		/// <summary>添加当前类型所有标记为 TriggerConditionAttribute 的函数为条件
		/// </summary>
		protected void mdsInitAttributeStep<TStepType>(int myStarIndex) where TStepType : TStep
		{
			List<FlowStepAttrData> myMethodList = StationHelp.GetFlowStepAttributeList(GetType());
			myMethodList = myMethodList.OrderBy((FlowStepAttrData p) => p.StepCode).ToList();
			for (int index = 0; index < myMethodList.Count; index++)
			{
				FlowStepAttrData myMethod = myMethodList[index];
				string myCode = "Z" + myStarIndex.ToString("0000");
				myStarIndex++;
				if (!string.IsNullOrEmpty(myMethod.StepCode))
				{
					myCode = myMethod.StepCode;
				}
				Type myCreateType = typeof(TStepType);
				if (myMethod.Attribute.CreateType != null)
				{
					myCreateType = myMethod.Attribute.CreateType;
				}
				if (!typeof(TStep).IsAssignableFrom(myCreateType))
				{
					throw new Exception("创建Step类型[" + myCreateType.Name + "] 与当前工作流指定的Step类型[" + typeof(TStep).Name + "] 不匹配");
				}
				TStepType myStep = (TStepType)Activator.CreateInstance(myCreateType, this, myCode);
				myStep.Desc = myMethod.Attribute.Desc;
				myStep.InnerFuncCode = myMethod.MethodInfo.Name;
				myStep.InnerValidateCode = myMethod.Attribute.ValidateCode;
				string myNextStepCode = myMethod.Attribute.NextStep;
				if (string.IsNullOrEmpty(myNextStepCode))
				{
					if (index < myMethodList.Count - 1)
					{
						myStep.NextStep = myMethodList[index + 1].MethodInfo.Name;
					}
					continue;
				}
				switch (myNextStepCode)
				{
				case "*":
				case "/":
				case "**":
				case "//":
					continue;
				}
				myStep.NextStep = myNextStepCode;
			}
		}

		/// <summary>添加当前类型所有标记为 TriggerConditionAttribute 的函数为条件
		/// </summary>
		protected void mdsInitAttributeStarCondition<TStarConditionType>(int myStarIndex) where TStarConditionType : TStarCondition
		{
			foreach (FlowStarConditionAttrData myMethod in StationHelp.GetFlowStarConditionAttributeList(GetType()))
			{
				string myCode = myStarIndex.ToString();
				myStarIndex++;
				Type myCreateType = typeof(TStarConditionType);
				if (myMethod.Attribute.CreateType != null)
				{
					myCreateType = myMethod.Attribute.CreateType;
				}
				if (!typeof(TStarCondition).IsAssignableFrom(myCreateType))
				{
					throw new Exception("创建StarCondition类型[" + myCreateType.Name + "] 与当前工作流指定的StarCondition类型[" + typeof(TStarCondition).Name + "] 不匹配");
				}
				TStarConditionType obj = (TStarConditionType)Activator.CreateInstance(myCreateType, this, myCode);
				obj.Desc = myMethod.Attribute.Desc;
				obj.InnerFuncCode = myMethod.MethodInfo.Name;
			}
		}
	}
}
