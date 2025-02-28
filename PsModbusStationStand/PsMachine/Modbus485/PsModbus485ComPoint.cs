using System;
using PsMachineWork;

namespace PsMachine.Modbus485
{
	/// <summary>Modbus 点位 目前支持5种值类型 int16, uint16, int32, uint32, bool, float
	/// Modbus 485 点位比较特殊，它即有数据类型，必须指定是哪个站点，它不同数值类型的点位编码会重复，
	/// 点位Code的命名规范，485站点编码-数值类型 点位编码
	/// 如 点位 1-U3  2-B10  
	/// 所以添加的点位规则的时候，点位编码前面要加个英文
	/// 前缀规范 bool:B  int16:S int32:I uint16:U  uint32:J  float:F
	/// </summary>
	public class PsModbus485ComPoint<T> : PsModbus485ComPoint
	{
		private bool _IsAllowActionChangeValue = true;

		/// <summary>是否允许自动数值Action操作
		/// </summary>
		public bool IsAllowActionChangeValue
		{
			get
			{
				return _IsAllowActionChangeValue;
			}
			set
			{
				if (_IsAllowActionChangeValue != value)
				{
					_IsAllowActionChangeValue = value;
					OnPropertyChanged("IsAllowActionChangeValue");
				}
			}
		}

		/// <summary>能否执行 Action:数值增减
		/// </summary>
		public virtual bool Can_Action_Num_Change
		{
			get
			{
				Type myType = typeof(T);
				if ((base.ReadWriteType == emPointReadWriteType.ReadWrite || base.ReadWriteType == emPointReadWriteType.WriteOnly) && IsAllowActionChangeValue)
				{
					if (!(myType == typeof(short)) && !(myType == typeof(ushort)) && !(myType == typeof(float)) && !(myType == typeof(int)))
					{
						return myType == typeof(uint);
					}
					return true;
				}
				return false;
			}
		}

		public virtual bool Visible_Action_Num_Change
		{
			get
			{
				Type myType = typeof(T);
				if ((base.ReadWriteType == emPointReadWriteType.ReadWrite || base.ReadWriteType == emPointReadWriteType.WriteOnly) && IsAllowActionChangeValue)
				{
					if (!(myType == typeof(short)) && !(myType == typeof(ushort)) && !(myType == typeof(float)) && !(myType == typeof(int)))
					{
						return myType == typeof(uint);
					}
					return true;
				}
				return false;
			}
		}

		/// <summary>能否执行 Action:数值增减
		/// </summary>
		public virtual bool Can_Action_Bool_NotValue
		{
			get
			{
				Type myType = typeof(T);
				if ((base.ReadWriteType == emPointReadWriteType.ReadWrite || base.ReadWriteType == emPointReadWriteType.WriteOnly) && IsAllowActionChangeValue)
				{
					return myType == typeof(bool);
				}
				return false;
			}
		}

		public virtual bool Visible_Action_Bool_NotValue
		{
			get
			{
				Type myType = typeof(T);
				if ((base.ReadWriteType == emPointReadWriteType.ReadWrite || base.ReadWriteType == emPointReadWriteType.WriteOnly) && IsAllowActionChangeValue)
				{
					return myType == typeof(bool);
				}
				return false;
			}
		}

		public PsModbus485ComPoint(PsModbus485ComStation myStation, string myCode)
			: base(myStation, myCode)
		{
		}

		protected override void mdsReadValue_Logic<Tv>(out Tv myValue)
		{
			if (!base.IsLocalDataPoint)
			{
				byte station = 1;
				mds解析485点位编码(base.Code, out station, out var myCode);
				base.ParentStation.GetPointValue<Tv>(station, myCode, out myValue);
			}
			else if (base.ValueObj == null)
			{
				myValue = default(Tv);
			}
			else
			{
				myValue = (Tv)base.ValueObj;
			}
		}

		protected override void mdsWriteValue_Logic<Tv>(Tv myValue)
		{
			if (!base.IsLocalDataPoint)
			{
				byte station = 1;
				mds解析485点位编码(base.Code, out station, out var myCode);
				base.ParentStation.SetPointValue(station, myCode, myValue);
			}
			else
			{
				base.ValueObj = myValue;
			}
		}

		public T ReadValue()
		{
			mdsReadValue<T>(out var myValue);
			return myValue;
		}

		public void WriteValue(T value)
		{
			mdsWriteValue(value);
		}

		/// <summary>读取点位值 object 类型
		/// </summary>
		/// <returns></returns>
		public override object ReadValueObject()
		{
			return ReadValue();
		}

		/// <summary>数值-100
		/// </summary>
		[StationAction("-100", CriteriaProperty = "Can_Action_Num_Change", VisibleProperty = "Visible_Action_Num_Change")]
		public void Action_Num_Dec100()
		{
			mdsValueChange(-100.0);
		}

		/// <summary>数值-10
		/// </summary>
		[StationAction("-10", CriteriaProperty = "Can_Action_Num_Change", VisibleProperty = "Visible_Action_Num_Change")]
		public void Action_Num_Dec10()
		{
			mdsValueChange(-10.0);
		}

		/// <summary>数值-1
		/// </summary>
		[StationAction("-1", CriteriaProperty = "Can_Action_Num_Change", VisibleProperty = "Visible_Action_Num_Change")]
		public void Action_Num_Dec1()
		{
			mdsValueChange(-1.0);
		}

		/// <summary>数值+1
		/// </summary>
		[StationAction("+1", CriteriaProperty = "Can_Action_Num_Change", VisibleProperty = "Visible_Action_Num_Change")]
		public void Action_Num_Add1()
		{
			mdsValueChange(1.0);
		}

		/// <summary>数值+10
		/// </summary>
		[StationAction("+10", CriteriaProperty = "Can_Action_Num_Change", VisibleProperty = "Visible_Action_Num_Change")]
		public void Action_Num_Add10()
		{
			mdsValueChange(10.0);
		}

		/// <summary>数值+100
		/// </summary>
		[StationAction("+100", CriteriaProperty = "Can_Action_Num_Change", VisibleProperty = "Visible_Action_Num_Change")]
		public void Action_Num_Add100()
		{
			mdsValueChange(100.0);
		}

		/// <summary>逻辑取反
		/// </summary>
		[StationAction("逻辑取反", CriteriaProperty = "Can_Action_Bool_NotValue", VisibleProperty = "Visible_Action_Bool_NotValue")]
		public void Action_Bool_NotValue()
		{
			if (typeof(T) == typeof(bool))
			{
				bool myValue = (bool)(object)ReadValue();
				myValue = !myValue;
				WriteValue((T)(object)myValue);
			}
		}

		private void mdsValueChange(double myChange)
		{
			Type myType = typeof(T);
			if (myType == typeof(short))
			{
				short myValue5 = (short)(object)ReadValue();
				myValue5 = (short)(myValue5 + (short)myChange);
				WriteValue((T)(object)myValue5);
				return;
			}
			if (myType == typeof(ushort))
			{
				ushort myValue4 = (ushort)(object)ReadValue();
				myValue4 = (ushort)(myValue4 + (ushort)myChange);
				WriteValue((T)(object)myValue4);
				return;
			}
			if (myType == typeof(float))
			{
				float myValue3 = (float)(object)ReadValue();
				myValue3 += (float)myChange;
				WriteValue((T)(object)myValue3);
				return;
			}
			if (myType == typeof(int))
			{
				int myValue2 = (int)(object)ReadValue();
				myValue2 += (int)myChange;
				WriteValue((T)(object)myValue2);
				return;
			}
			if (myType == typeof(uint))
			{
				uint myValue = (uint)(object)ReadValue();
				myValue += (uint)myChange;
				WriteValue((T)(object)myValue);
				return;
			}
			throw new Exception("执行" + myChange + "操作时,未识别的数值类型");
		}

		private void mds解析485点位编码(string myPointCode485, out byte myStation485, out string myCode)
		{
			myStation485 = 1;
			myCode = "";
			if (string.IsNullOrEmpty(myPointCode485))
			{
				return;
			}
			string[] mySplitList = myPointCode485.Split(new char[1] { '-' });
			if (mySplitList.Length == 1)
			{
				myCode = mySplitList[0];
				char myFirstChar2 = myCode[0];
				if ((myFirstChar2 >= 'a' && myFirstChar2 <= 'z') || (myFirstChar2 >= 'A' && myFirstChar2 <= 'Z'))
				{
					myCode = myCode.Substring(1);
				}
				return;
			}
			myStation485 = byte.Parse(mySplitList[0]);
			myCode = mySplitList[1];
			char myFirstChar = myCode[0];
			if ((myFirstChar >= 'a' && myFirstChar <= 'z') || (myFirstChar >= 'A' && myFirstChar <= 'Z'))
			{
				myCode = myCode.Substring(1);
			}
		}
	}
	/// <summary>Modbus 485的点位
	/// </summary>
	public abstract class PsModbus485ComPoint : PsWorkStationPointBase<PsModbus485ComStation, PsModbus485ComPoint>
	{
		public PsModbus485ComPoint(PsModbus485ComStation myStation, string myCode)
			: base(myStation, myCode)
		{
		}
	}
}
