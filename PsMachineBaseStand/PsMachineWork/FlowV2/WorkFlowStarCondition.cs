using System;
using System.ComponentModel;
using System.Reflection;

namespace PsMachineWork.FlowV2
{
	/// <summary>流程启动条件
	/// </summary>
	/// <typeparam name="TFlow"></typeparam>
	public abstract class WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition> : iWorkFlowStarCondition where TStation : iWorkStation where TFlow : WorkFlowBase<TStation, TFlow, TStep, TStarCondition> where TStep : WorkFlowStep<TStation, TFlow, TStep, TStarCondition> where TStarCondition : WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition>
	{
		private bool _IsComplate;

		private bool _IsException;

		private string _RunDesc;

		private DateTime? _LastRunTime;

		/// <summary>所在的工作流
		/// </summary>
		public TFlow WorkFlow { get; set; }

		/// <summary>所在的工作流
		/// </summary>
		iWorkFlow iWorkFlowStarCondition.WorkFlow => WorkFlow;

		/// <summary>条件的编码
		/// </summary>
		public string Code { get; protected set; }

		/// <summary>工作步骤的内部编码, 也用于派生类中，标记执行函数名称
		/// </summary>
		public string InnerFuncCode { get; set; }

		/// <summary>描述
		/// </summary>
		public string Desc { get; set; }

		/// <summary>验证通过,执行结束
		/// </summary>
		public bool IsComplate
		{
			get
			{
				return _IsComplate;
			}
			set
			{
				if (_IsComplate != value)
				{
					_IsComplate = value;
					OnPropertyChanged("IsComplate");
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
			set
			{
				if (_IsException != value)
				{
					_IsException = value;
					OnPropertyChanged("IsException");
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
			set
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
			set
			{
				if (_LastRunTime != value)
				{
					_LastRunTime = value;
					OnPropertyChanged("LastRunTime");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public WorkFlowStarCondition(TFlow myFlow, string myCode)
		{
			Code = myCode;
			WorkFlow = myFlow;
			myFlow.FlowStarConditionList.Add((TStarCondition)this);
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>执行验证,如果验证不通过会抛出异常
		/// </summary>
		public abstract void RunValidate();

		/// <summary>内部执行方法,执行验证,返回验证成功或者失败
		/// 此方法会拦截异常,有异常时会标记为异常
		/// </summary>
		internal bool mdsRun()
		{
			try
			{
				IsComplate = false;
				IsException = false;
				RunDesc = null;
				LastRunTime = DateTime.Now;
				RunValidate();
				IsComplate = true;
				return true;
			}
			catch (MachineWaitException ex)
			{
				RunDesc = ex.Message;
				return false;
			}
			catch (Exception ex2)
			{
				IsException = true;
				RunDesc = ex2.Message;
				return false;
			}
		}

		/// <summary>根据 调用工作流中对应方法名
		/// </summary>
		/// <exception cref="T:System.Exception"></exception>
		protected void mdsGotoFunction(string myFunctionCode)
		{
			if (!string.IsNullOrEmpty(myFunctionCode))
			{
				MethodInfo method = WorkFlow.GetType().GetMethod(myFunctionCode, BindingFlags.Instance | BindingFlags.Public);
				if (method == null)
				{
					throw new Exception("没有找到对应的函数:" + myFunctionCode);
				}
				method.Invoke(WorkFlow, null);
			}
		}
	}
}
