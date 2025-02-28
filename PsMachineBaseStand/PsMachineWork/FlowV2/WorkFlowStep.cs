using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PsMachineWork.FlowV2
{
	/// <summary>工作步骤
	/// </summary>
	/// <typeparam name="Tthis">传入派生类中的本体类</typeparam>
	/// <typeparam name="TFlow"></typeparam>
	/// <typeparam name="TStepNext">传入下一步的派生类</typeparam>
	public abstract class WorkFlowStep<TStation, TFlow, TStep, TStarCondition> : iWorkFlowStep, INotifyPropertyChanged where TStation : iWorkStation where TFlow : WorkFlowBase<TStation, TFlow, TStep, TStarCondition> where TStep : WorkFlowStep<TStation, TFlow, TStep, TStarCondition> where TStarCondition : WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition>
	{
		private TStep _FrontStep;

		private bool _IsStart;

		private bool _IsException;

		private bool _IsComplate;

		private string _RunDesc;

		private DateTime? _LastRunTime;

		private bool _IsStarNextStep;

		private string _NextStep;

		/// <summary>所在的工作流
		/// </summary>
		public TFlow WorkFlow { get; set; }

		/// <summary>所在的工作流
		/// </summary>
		iWorkFlow iWorkFlowStep.WorkFlow => WorkFlow;

		/// <summary>前一个工作步骤
		/// </summary>
		public TStep FrontStep
		{
			get
			{
				return _FrontStep;
			}
			set
			{
				if (_FrontStep != value)
				{
					_FrontStep = value;
					OnPropertyChanged("FrontStep");
				}
			}
		}

		/// <summary>前一个工作步骤
		/// </summary>
		iWorkFlowStep iWorkFlowStep.FrontStep => FrontStep;

		/// <summary>工作步骤的编码
		/// </summary>
		public string Code { get; protected set; }

		/// <summary>工作步骤的内部编码, 也用于派生类中，标记执行函数名称
		/// </summary>
		public string InnerFuncCode { get; set; }

		/// <summary>无特定用途，也用于派生类中，标记调用的验证函数名称
		/// </summary>
		public string InnerValidateCode { get; set; }

		/// <summary>描述
		/// </summary>
		public string Desc { get; set; }

		/// <summary>开始执行
		/// </summary>
		public bool IsStart
		{
			get
			{
				return _IsStart;
			}
			protected internal set
			{
				if (_IsStart != value)
				{
					_IsStart = value;
					OnPropertyChanged("IsStart");
					OnPropertyChanged("CanRunAction");
				}
			}
		}

		/// <summary>运行异常
		/// </summary>
		public bool IsException
		{
			get
			{
				return _IsException;
			}
			protected internal set
			{
				if (_IsException != value)
				{
					_IsException = value;
					OnPropertyChanged("IsException");
				}
			}
		}

		/// <summary>执行结束
		/// </summary>
		public bool IsComplate
		{
			get
			{
				return _IsComplate;
			}
			protected internal set
			{
				if (_IsComplate != value)
				{
					_IsComplate = value;
					OnPropertyChanged("IsComplate");
					OnPropertyChanged("CanRunAction");
				}
			}
		}

		/// <summary>运行状态描述，如果有异常在此写入
		/// </summary>
		public string RunDesc
		{
			get
			{
				return _RunDesc;
			}
			protected internal set
			{
				if (_RunDesc != value)
				{
					_RunDesc = value;
					OnPropertyChanged("RunDesc");
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
			protected internal set
			{
				if (_LastRunTime != value)
				{
					_LastRunTime = value;
					OnPropertyChanged("LastRunTime");
				}
			}
		}

		/// <summary>步骤结束后，是否已经执行下一个步骤的分支
		/// 注意如果是结束的步骤，记得此值一定要默认配置为 True，否则计算分支时会报异常
		/// </summary>
		public bool IsStarNextStep
		{
			get
			{
				return _IsStarNextStep;
			}
			protected internal set
			{
				if (_IsStarNextStep != value)
				{
					_IsStarNextStep = value;
					OnPropertyChanged("IsStarNextStep");
				}
			}
		}

		/// <summary>下一步的内码
		/// </summary>
		public string NextStep
		{
			get
			{
				return _NextStep;
			}
			set
			{
				if (_NextStep != value)
				{
					_NextStep = value;
					OnPropertyChanged("NextStep");
				}
			}
		}

		/// <summary>状态是否可以执行,已经启动，并且未完成
		/// </summary>
		public bool CanRunAction
		{
			get
			{
				if (IsStart)
				{
					return !IsComplate;
				}
				return false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public WorkFlowStep(TFlow myFlow, string myCode)
		{
			Code = myCode;
			WorkFlow = myFlow;
			myFlow.Steps.Add((TStep)this);
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>验证是否允许运行本步骤,不通过时会抛出异常
		/// </summary>
		public abstract void ValidateAllowRunStep();

		/// <summary>执行本步, 供外部编写人员填写内容主体
		/// 执行完成时,同时要写入 BranchStatus 属性,定义下一步的分支
		/// </summary>
		public abstract void RunStep();

		/// <summary>工作步骤的内部执行函数, RunStep 函数是派生后的执行主体
		/// </summary>
		internal void mdsRun()
		{
			if (!IsStart)
			{
				return;
			}
			if (IsComplate)
			{
				mdsComputerNextStep();
				return;
			}
			bool myCheckAllowRun = false;
			IsComplate = false;
			IsException = false;
			RunDesc = null;
			LastRunTime = DateTime.Now;
			try
			{
				ValidateAllowRunStep();
				myCheckAllowRun = true;
			}
			catch (Exception ex2)
			{
				if (mdsRunException(ex2))
				{
					return;
				}
			}
			if (!myCheckAllowRun)
			{
				return;
			}
			bool myIsComplate = false;
			try
			{
				RunStep();
				myIsComplate = true;
			}
			catch (Exception ex)
			{
				if (mdsRunException(ex))
				{
					return;
				}
			}
			if (myIsComplate)
			{
				IsComplate = true;
				mdsComputerNextStep();
			}
		}

		/// <summary>执行时遇到异常及处理,要跳到其它步骤时返回 True, 不跳到其它异常处理步骤时返回 False
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		private bool mdsRunException(Exception ex)
		{
			TargetInvocationException exOrg = ex as TargetInvocationException;
			if (exOrg != null)
			{
				ex = exOrg.InnerException;
			}
			if (ex == null)
			{
				return false;
			}
			if (ex is MachineWaitException)
			{
				IsException = false;
				RunDesc = ex.Message;
				return false;
			}
			MachineExceptionFlowGotoStep ex2 = ex as MachineExceptionFlowGotoStep;
			if (ex2 != null)
			{
				mdsGotoException(ex2);
				return true;
			}
			IsException = true;
			RunDesc = ex.Message;
			return mdsCheckExceptionGotoOtherStep(ex);
		}

		/// <summary>计算执行结束后,后面运行哪个分支步骤
		/// </summary>
		private void mdsComputerNextStep()
		{
			if (!IsStarNextStep)
			{
				string myNextStepInnerCode = NextStep;
				if (string.IsNullOrEmpty(myNextStepInnerCode))
				{
					throw new Exception("没有定义NextStep分支");
				}
				TStep myFindNextStep = WorkFlow.Steps.FirstOrDefault((TStep p) => p.InnerFuncCode == myNextStepInnerCode);
				if (myFindNextStep == null)
				{
					throw new Exception("没有找到执行步骤:" + myNextStepInnerCode);
				}
				myFindNextStep.FrontStep = (TStep)this;
				myFindNextStep.IsStart = true;
				IsStarNextStep = true;
				WorkFlow.SelectStep = myFindNextStep;
			}
		}

		/// <summary>发生异常时，检查是否跳转到其它步骤
		/// 如果已经跳转其它步骤处理，返回 True, 否则返回 False
		/// </summary>
		/// <param name="ex"></param>
		internal bool mdsCheckExceptionGotoOtherStep(Exception ex)
		{
			Type myCurExpType = ex.GetType();
			foreach (PsWorkFlowStepException myExcepStep in WorkFlow.StepExceptionList)
			{
				if (myExcepStep.ExceptionType == null || !myExcepStep.ExceptionType.IsAssignableFrom(myCurExpType))
				{
					continue;
				}
				TStep myStep = WorkFlow.Steps.FirstOrDefault((TStep p) => p.InnerFuncCode == myExcepStep.StepInnerCode);
				if (myStep == null)
				{
					throw new Exception("异常步骤定义中，StepInnerCode:" + myExcepStep.StepInnerCode + " 不存在");
				}
				IExceptionLogMsgKey myMsgExcep = ex as IExceptionLogMsgKey;
				if (myMsgExcep != null)
				{
					if (!string.IsNullOrEmpty(ex.Message))
					{
						RunDesc = ex.Message;
						WorkFlow.LogManager.AddMachineKeyLog(Desc + " 跳转", ex.Message, myIsException: true, myMsgExcep.MsgKey, myMsgExcep.DelayTime);
					}
				}
				else if (!string.IsNullOrEmpty(ex.Message))
				{
					RunDesc = ex.Message;
					WorkFlow.LogManager.AddMachineLog(Desc + " 跳转", ex.Message, myIsException: true);
				}
				mdsGotoOtherStep(myStep);
				return true;
			}
			return false;
		}

		/// <summary>跳转到异常指定步骤
		/// </summary>
		/// <param name="ex"></param>
		internal void mdsGotoException(MachineExceptionFlowGotoStep ex)
		{
			string myOtherStepInnerCode = ex.GotoStepInnerCode;
			if (string.IsNullOrEmpty(myOtherStepInnerCode))
			{
				throw new Exception("未指定异常跳转步骤的执行步骤:MachineExceptionFlowGotoStep");
			}
			TStep myStep = WorkFlow.Steps.FirstOrDefault((TStep p) => p.InnerFuncCode == myOtherStepInnerCode);
			if (myStep == null)
			{
				throw new Exception("异常跳转步骤的执行步骤 " + myOtherStepInnerCode + " 不存在");
			}
			IExceptionLogMsgKey myMsgExcep = ex as IExceptionLogMsgKey;
			if (myMsgExcep != null)
			{
				if (!string.IsNullOrEmpty(ex.Message))
				{
					RunDesc = ex.Message;
					WorkFlow.LogManager.AddMachineKeyLog(Desc + " 跳转", ex.Message, myIsException: true, myMsgExcep.MsgKey, myMsgExcep.DelayTime);
				}
			}
			else if (!string.IsNullOrEmpty(ex.Message))
			{
				RunDesc = ex.Message;
				WorkFlow.LogManager.AddMachineLog(Desc + " 跳转", ex.Message, myIsException: true);
			}
			mdsGotoOtherStep(myStep);
		}

		/// <summary>跳转到其它步骤
		/// </summary>
		/// <param name="myNextStep"></param>
		private void mdsGotoOtherStep(TStep myNextStep)
		{
			IsStart = true;
			IsComplate = true;
			myNextStep.FrontStep = (TStep)this;
			myNextStep.IsStart = true;
			IsStarNextStep = true;
			WorkFlow.SelectStep = myNextStep;
		}

		/// <summary>根据 调用工作流中对应方法名
		/// </summary>
		/// <exception cref="T:System.Exception"></exception>
		protected void mdsGotoFunction(string myFunctionCode)
		{
			if (string.IsNullOrEmpty(myFunctionCode))
			{
				return;
			}
			MethodInfo myMethod = WorkFlow.GetType().GetMethod(myFunctionCode, BindingFlags.Instance | BindingFlags.Public);
			if (myMethod == null)
			{
				throw new Exception("没有找到对应的函数:" + myFunctionCode);
			}
			ParameterInfo[] myPars = myMethod.GetParameters();
			if (myPars != null && myPars.Length != 0)
			{
				if (myPars.Length != 1)
				{
					throw new Exception("无法识别的函数参数 " + myFunctionCode);
				}
				if (!myPars[0].ParameterType.IsAssignableFrom(GetType()))
				{
					throw new Exception("无法识别的函数参数 " + myFunctionCode);
				}
				myMethod.Invoke(WorkFlow, new object[1] { this });
			}
			else
			{
				myMethod.Invoke(WorkFlow, null);
			}
		}
	}
}
