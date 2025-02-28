using System;
using System.ComponentModel;
using System.Reflection;
using PsMachineWork.触发器.接口;

namespace PsMachineWork
{
	/// <summary>触发器的启动条件
	/// </summary>
	public abstract class PsWorkTriggerCondition<Tstation, Ttrigger, TtrigerCondition> : iWorkTriggerCondition, INotifyPropertyChanged where Tstation : iWorkStation where Ttrigger : PsWorkTrigger<Tstation, Ttrigger, TtrigerCondition> where TtrigerCondition : PsWorkTriggerCondition<Tstation, Ttrigger, TtrigerCondition>
	{
		private bool _IsComplate;

		private bool _IsException;

		private string _RunDesc;

		private DateTime? _LastRunTime;

		/// <summary>所在的触发器
		/// </summary>
		public Ttrigger WorkTrigger { get; set; }

		/// <summary>所在的触发器
		/// </summary>
		iWorkTrigger iWorkTriggerCondition.WorkTrigger => WorkTrigger;

		/// <summary>编码
		/// </summary>
		public string Code { get; set; }

		/// <summary>内部编码
		/// </summary>
		public string InnerCode { get; set; }

		/// <summary>注释
		/// </summary>
		public string Desc { get; set; }

		/// <summary>验证通过
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

		/// <summary>验证异常
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

		/// <summary>运行时注释
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

		/// <summary>最后运行时间
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

		public PsWorkTriggerCondition(Ttrigger myTrig, string myCode)
		{
			Code = myCode;
			WorkTrigger = myTrig;
			myTrig.ConditionCollection.Add((TtrigerCondition)this);
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>执行验证,如果验证不通过会抛出异常
		/// 注意这里只是正常的逻辑封装，并不需要包含异常时各种善后处理
		/// </summary>
		public abstract void RunValidate();

		/// <summary>内部执行方法,执行验证,返回验证成功或者失败
		/// 此方法会拦截异常,有异常时会标记为异常
		/// </summary>
		internal bool mdsRun()
		{
			try
			{
				LastRunTime = DateTime.Now;
				RunValidate();
				IsException = false;
				RunDesc = null;
				IsComplate = true;
				return true;
			}
			catch (Exception ex)
			{
				mdsOnException(ex);
				return false;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		private void mdsOnException(Exception ex)
		{
			TargetInvocationException myRefExcep = ex as TargetInvocationException;
			if (myRefExcep != null)
			{
				if (myRefExcep.InnerException != null)
				{
					mdsOnException(myRefExcep.InnerException);
					return;
				}
				IsException = true;
				RunDesc = ex.Message;
				IsComplate = false;
			}
			else if (ex is MachineWaitException)
			{
				IsException = false;
				RunDesc = ex.Message;
				IsComplate = false;
			}
			else
			{
				IsException = true;
				RunDesc = ex.Message;
				IsComplate = false;
			}
		}

		/// <summary>根据 调用工作流中对应方法名
		/// </summary>
		/// <exception cref="T:System.Exception"></exception>
		protected void mdsGotoFunction(string myFunctionCode)
		{
			MethodInfo method = WorkTrigger.GetType().GetMethod(myFunctionCode, BindingFlags.Instance | BindingFlags.Public);
			if (method == null)
			{
				throw new Exception("没有找到对应的函数:" + myFunctionCode);
			}
			method.Invoke(WorkTrigger, null);
		}
	}
}
