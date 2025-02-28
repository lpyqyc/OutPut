using System;
using System.ComponentModel;
using System.Reflection;
using PsMachineTools.Tools;

namespace PsMachineWork
{
	/// <summary>工作站点位
	/// </summary>
	/// <typeparam name="TStation"></typeparam>
	/// <typeparam name="TPoint">指向本类</typeparam>
	public abstract class PsWorkStationPointBase<TStation, TPoint> : iStationPoint, INotifyPropertyChanged where TStation : PsWorkStationBase<TStation, TPoint> where TPoint : PsWorkStationPointBase<TStation, TPoint>
	{
		private TStation _ParentStation;

		private string _Code;

		private string _Desc;

		private object _ValueObj;

		private DateTime? _MachineLastReadTime;

		private DateTime? _MachineLastWriteTime;

		private bool _IsException;

		private string _RunDesc;

		private emPointReadWriteType _ReadWriteType;

		private bool _IsAutoRefresh;

		private bool _IsLocalDataPoint;

		/// <summary>所在工作站
		/// </summary>
		public TStation ParentStation
		{
			get
			{
				return _ParentStation;
			}
			set
			{
				if ((_ParentStation != null || value != null) && (_ParentStation == null || value == null || !_ParentStation.Equals(value)))
				{
					_ParentStation = value;
					OnPropertyChanged("ParentStation");
				}
			}
		}

		/// <summary>所在工作站
		/// </summary>
		iWorkStation iStationPoint.ParentStation => ParentStation;

		/// <summary>点位编码，英文大小写加数字加下划线，不要用其它符合，否则可能会引起其它解析异常
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

		/// <summary>点位描述
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

		/// <summary>值描述
		/// </summary>
		public virtual string ValueDesc
		{
			get
			{
				if (IsException)
				{
					return "(Error)";
				}
				if (ValueObj == null)
				{
					return null;
				}
				return ValueObj.ToString();
			}
		}

		/// <summary>点位值的Object类型,在具体的派生类中，应该封装自己的实际值类型 Value 属性
		/// </summary>
		public object ValueObj
		{
			get
			{
				return _ValueObj;
			}
			protected set
			{
				if (_ValueObj != value)
				{
					_ValueObj = value;
					OnPropertyChanged("ValueObj");
					OnPropertyChanged("ValueDesc");
				}
			}
		}

		/// <summary>最后读取设备更新时间
		/// </summary>
		public DateTime? LastReadTime
		{
			get
			{
				return _MachineLastReadTime;
			}
			set
			{
				if (_MachineLastReadTime != value)
				{
					_MachineLastReadTime = value;
					OnPropertyChanged("LastReadTime");
				}
			}
		}

		/// <summary>最后写入设备时间
		/// </summary>
		public DateTime? LastWriteTime
		{
			get
			{
				return _MachineLastWriteTime;
			}
			set
			{
				if (_MachineLastWriteTime != value)
				{
					_MachineLastWriteTime = value;
					OnPropertyChanged("LastWriteTime");
				}
			}
		}

		/// <summary>当前是否有异常
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
					OnPropertyChanged("ValueDesc");
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

		/// <summary>点位的可读写类型,
		/// 只读,只写,可读写,或者都不行
		/// </summary>
		public emPointReadWriteType ReadWriteType
		{
			get
			{
				return _ReadWriteType;
			}
			set
			{
				if (_ReadWriteType != value)
				{
					_ReadWriteType = value;
					OnPropertyChanged("ReadWriteType");
				}
			}
		}

		/// <summary>是否支持从设备端定时读取刷新数据
		/// </summary>
		public bool IsAutoRefresh
		{
			get
			{
				return _IsAutoRefresh;
			}
			set
			{
				if (_IsAutoRefresh != value)
				{
					_IsAutoRefresh = value;
					OnPropertyChanged("IsAutoRefresh");
				}
			}
		}

		/// <summary>是否本地数据存储点位, 并非PLC或者控制器点位等异地缓存点位
		/// </summary>
		public bool IsLocalDataPoint
		{
			get
			{
				return _IsLocalDataPoint;
			}
			set
			{
				if (_IsLocalDataPoint != value)
				{
					_IsLocalDataPoint = value;
					OnPropertyChanged("IsLocalDataPoint");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public PsWorkStationPointBase(TStation myStation, string myCode)
		{
			ParentStation = myStation;
			Code = myCode;
			myStation.ChildsPoint.Add((TPoint)this);
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>读取点位值
		/// </summary>
		/// <returns></returns>
		public abstract object ReadValueObject();

		/// <summary>封装读取点位值的外围函数，主要是异常拦截
		/// 真正读取的逻辑，应该封装在 mdsReadValue_Logic 函数中
		/// </summary>
		/// <typeparam name="Tv"></typeparam>
		/// <param name="myValue"></param>
		protected void mdsReadValue<Tv>(out Tv myValue)
		{
			LastReadTime = DateTime.Now;
			try
			{
				if (ReadWriteType != emPointReadWriteType.ReadWrite && ReadWriteType != emPointReadWriteType.ReadOnly)
				{
					throw new Exception("点位不支持读取");
				}
				if (IsLocalDataPoint)
				{
					mdsReadValue_Local_Logic<Tv>(out myValue);
				}
				else
				{
					mdsReadValue_Logic<Tv>(out myValue);
					ValueObj = myValue;
				}
				IsException = false;
				RunDesc = null;
			}
			catch (Exception ex)
			{
				IsException = true;
				RunDesc = "读取异常:" + ex.Message;
				throw;
			}
		}

		/// <summary>封装写入点位值的外围函数,主要是异常拦截
		/// 真正写入的逻辑，应该封装在 mdsWriteValue_Logic 函数中
		/// </summary>
		/// <typeparam name="Tv"></typeparam>
		/// <param name="value"></param>
		protected void mdsWriteValue<Tv>(Tv value)
		{
			LastWriteTime = DateTime.Now;
			try
			{
				if (ReadWriteType != emPointReadWriteType.ReadWrite && ReadWriteType != emPointReadWriteType.WriteOnly)
				{
					throw new Exception("点位不支持写值");
				}
				if (IsLocalDataPoint)
				{
					mdsWriteValue_Local_Logic(value);
				}
				else
				{
					mdsWriteValue_Logic(value);
					ValueObj = value;
				}
				IsException = false;
				RunDesc = null;
			}
			catch (Exception ex)
			{
				IsException = true;
				RunDesc = "写入异常:" + ex.Message;
				throw;
			}
		}

		/// <summary>读取节点内容的逻辑函数,不需要处理异常
		/// </summary>
		protected abstract void mdsReadValue_Logic<Tv>(out Tv myValue);

		/// <summary>写入节点内容的逻辑函数,不需要处理异常
		/// </summary>
		protected abstract void mdsWriteValue_Logic<Tv>(Tv myValue);

		protected virtual void mdsReadValue_Local_Logic<Tv>(out Tv myValue)
		{
			string myValueTxt = null;
			TStation myStation = ParentStation;
			try
			{
				if (ValueObj != null)
				{
					Type myType = typeof(Tv);
					myValueTxt = ((myType.IsPrimitive || myType.IsEnum) ? ValueObj.ToString() : ((myType == typeof(string)) ? ((string)ValueObj) : ((!HyExpFunction.IsNullableType(myType)) ? "{...}" : ValueObj.ToString())));
				}
				if (ValueObj == null)
				{
					myValue = default(Tv);
				}
				else
				{
					myValue = (Tv)ValueObj;
				}
				if (myStation.IsLog_ReadPoint)
				{
					if (myValueTxt == null || myValueTxt.Length < 500)
					{
						myStation.LogManager.AddMachineLog("读取点位 [" + Code + "] = " + myValueTxt);
						return;
					}
					myStation.LogManager.AddMachineLog("读取点位 [" + Code + "] = " + myValueTxt.Substring(0, 500) + "...");
				}
			}
			catch (Exception ex)
			{
				myStation.IsException = true;
				string myMsgKey = "读取点位异常";
				string myExpMsg = "读取点位[" + Code + "]异常: " + ex.Message;
				myStation.LogManager.AddMachineKeyLog(myExpMsg, null, myIsException: true, myMsgKey, 1000);
				throw new Exception(myExpMsg);
			}
		}

		/// <summary>
		/// </summary>
		protected virtual void mdsWriteValue_Local_Logic<Tv>(Tv myValue)
		{
			string myValueTxt = null;
			TStation myStation = ParentStation;
			try
			{
				ValueObj = myValue;
				if (ValueObj != null)
				{
					Type myType = typeof(Tv);
					myValueTxt = ((myType.IsPrimitive || myType.IsEnum) ? ValueObj.ToString() : ((myType == typeof(string)) ? ((string)ValueObj) : ((!HyExpFunction.IsNullableType(myType)) ? "{...}" : ValueObj.ToString())));
				}
				if (myStation.IsLog_WritePoint)
				{
					if (myValueTxt == null || myValueTxt.Length < 500)
					{
						myStation.LogManager.AddMachineLog("写入PLC点位 [" + Code + "] = " + myValueTxt);
						return;
					}
					myStation.LogManager.AddMachineLog("写入PLC点位 [" + Code + "] = " + myValueTxt.Substring(0, 500) + "...");
				}
			}
			catch (Exception ex)
			{
				myStation.IsException = true;
				string myExpMsg = "写入点位[" + Code + "]=" + myValueTxt + " 异常: " + ex.Message;
				myStation.LogManager.AddMachineLog(myExpMsg);
				throw new Exception(myExpMsg);
			}
		}

		/// <summary>根据 调用工作流中对应方法名
		/// </summary>
		/// <exception cref="T:System.Exception"></exception>
		protected void mdsGotoFunction(string myFunctionCode)
		{
			MethodInfo method = ParentStation.GetType().GetMethod(myFunctionCode, BindingFlags.Instance | BindingFlags.Public);
			if (method == null)
			{
				throw new Exception("没有找到对应的函数:" + myFunctionCode);
			}
			method.Invoke(ParentStation, null);
		}
	}
}
