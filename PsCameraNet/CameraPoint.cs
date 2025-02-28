using PsMachineWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsCameraNet
{
    public class CameraPoint<T> : CameraPoint
    {

        public CameraPoint(CameraStation myStation, string myCode)
            : base(myStation, myCode)
        {
            this.IsLocalDataPoint = true;
        }

        public T ReadValue()
        {
            T myValue;
            mdsReadValue<T>(out myValue);
            return myValue;
        }

        public void WriteValue(T value)
        {
            mdsWriteValue<T>(value);
        }

        /// <summary>读取点位值 object 类型
        /// </summary>
        /// <returns></returns>
        public override object ReadValueObject()
        {
            return ReadValue();
        }


        /// <summary>一共有两类数据，一个从相机读取，一个本地内存读取
        /// 由于要显示日志，所以内存读取也要从
        /// </summary>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="myValue"></param>
        protected override void mdsReadValue_Logic<Tv>(out Tv myValue)
        {
            myValue = (Tv)this.ValueObj;
        }


        protected override void mdsWriteValue_Logic<Tv>(Tv myValue)
        {
            this.ValueObj = myValue;
        }



        /// <summary>是否允许自动数值Action操作
        /// </summary>
        public bool IsAllowActionChangeValue
        {
            get { return _IsAllowActionChangeValue; }
            set
            {
                var myChanged = _IsAllowActionChangeValue != value;
                if (myChanged)
                {
                    _IsAllowActionChangeValue = value;
                    OnPropertyChanged(nameof(IsAllowActionChangeValue));
                }
            }
        }
        bool _IsAllowActionChangeValue = true;


        /// <summary>数值-100
        /// </summary>
        [StationAction("-100", CriteriaProperty = nameof(Can_Action_Num_Change),
            VisibleProperty = nameof(Visible_Action_Num_Change))]
        public void Action_Num_Dec100()
        {
            mdsValueChange(-100d);
        }

        /// <summary>数值-10
        /// </summary>
        [StationAction("-10", CriteriaProperty = nameof(Can_Action_Num_Change),
            VisibleProperty = nameof(Visible_Action_Num_Change))]
        public void Action_Num_Dec10()
        {
            mdsValueChange(-10d);
        }

        /// <summary>数值-1
        /// </summary>
        [StationAction("-1", CriteriaProperty = nameof(Can_Action_Num_Change),
            VisibleProperty = nameof(Visible_Action_Num_Change))]
        public void Action_Num_Dec1()
        {
            mdsValueChange(-1d);
        }

        /// <summary>数值+1
        /// </summary>
        [StationAction("+1", CriteriaProperty = nameof(Can_Action_Num_Change),
            VisibleProperty = nameof(Visible_Action_Num_Change))]
        public void Action_Num_Add1()
        {
            mdsValueChange(1d);
        }

        /// <summary>数值+10
        /// </summary>
        [StationAction("+10", CriteriaProperty = nameof(Can_Action_Num_Change),
            VisibleProperty = nameof(Visible_Action_Num_Change))]
        public void Action_Num_Add10()
        {
            mdsValueChange(10d);
        }

        /// <summary>数值+100
        /// </summary>
        [StationAction("+100", CriteriaProperty = nameof(Can_Action_Num_Change),
            VisibleProperty = nameof(Visible_Action_Num_Change))]
        public void Action_Num_Add100()
        {
            mdsValueChange(100d);
        }

        /// <summary>逻辑取反
        /// </summary>
        [StationAction("逻辑取反", CriteriaProperty = nameof(Can_Action_Bool_NotValue),
            VisibleProperty = nameof(Visible_Action_Bool_NotValue))]
        public void Action_Bool_NotValue()
        {
            var myType = typeof(T);
            if (myType == typeof(bool))
            {
                var myValue = (bool)(object)this.ReadValue();
                myValue = !myValue;
                this.WriteValue((T)(object)myValue);
            }
        }



        private void mdsValueChange(double myChange)
        {
            var myType = typeof(T);
            if (myType == typeof(Int16))
            {
                var myValue = (Int16)(object)this.ReadValue();
                myValue += (Int16)myChange;
                this.WriteValue((T)(object)myValue);
            }
            else if (myType == typeof(UInt16))
            {
                var myValue = (UInt16)(object)this.ReadValue();
                myValue += (UInt16)myChange;
                this.WriteValue((T)(object)myValue);
            }
            else if (myType == typeof(float))
            {
                var myValue = (float)(object)this.ReadValue();
                myValue += (float)myChange;
                this.WriteValue((T)(object)myValue);
            }
            else if (myType == typeof(Int32))
            {
                var myValue = (Int32)(object)this.ReadValue();
                myValue += (Int32)myChange;
                this.WriteValue((T)(object)myValue);
            }
            else if (myType == typeof(UInt32))
            {
                var myValue = (UInt32)(object)this.ReadValue();
                myValue += (UInt32)myChange;
                this.WriteValue((T)(object)myValue);
            }
            else
            {
                var myMsg = $"执行{myChange.ToString()}操作时,未识别的数值类型";
                throw new Exception(myMsg);
                //this.ParentStation.LogManager.AddMachineLog(myMsg, true);
            }

        }


        /// <summary>能否执行 Action:数值增减
        /// </summary>
        public virtual bool Can_Action_Num_Change
        {
            get
            {
                //是数值,并且可写
                var myType = typeof(T);
                return (this.ReadWriteType == emPointReadWriteType.ReadWrite || this.ReadWriteType == emPointReadWriteType.WriteOnly)
                    && this.IsAllowActionChangeValue
                    && (myType == typeof(Int16) || myType == typeof(UInt16) || myType == typeof(float) || myType == typeof(Int32) || myType == typeof(UInt32));
            }
        }

        public virtual bool Visible_Action_Num_Change
        {
            get
            {
                //是数值,并且可写
                var myType = typeof(T);
                return (this.ReadWriteType == emPointReadWriteType.ReadWrite || this.ReadWriteType == emPointReadWriteType.WriteOnly)
                    && this.IsAllowActionChangeValue
                    && (myType == typeof(Int16) || myType == typeof(UInt16) || myType == typeof(float) || myType == typeof(Int32) || myType == typeof(UInt32));
            }
        }

        //Action_Bool_NotValue

        /// <summary>能否执行 Action:数值增减
        /// </summary>
        public virtual bool Can_Action_Bool_NotValue
        {
            get
            {
                //是bool,并且可写
                var myType = typeof(T);
                return (this.ReadWriteType == emPointReadWriteType.ReadWrite || this.ReadWriteType == emPointReadWriteType.WriteOnly)
                    && this.IsAllowActionChangeValue
                    && (myType == typeof(bool));
            }
        }

        public virtual bool Visible_Action_Bool_NotValue
        {
            get
            {
                //是bool,并且可写
                var myType = typeof(T);
                return (this.ReadWriteType == emPointReadWriteType.ReadWrite || this.ReadWriteType == emPointReadWriteType.WriteOnly)
                    && this.IsAllowActionChangeValue
                    && (myType == typeof(bool));
            }
        }


    }



    /// <summary>相机点位
    /// </summary>
    public abstract class CameraPoint : PsWorkStationPointBase<CameraStation, CameraPoint>
    {
        public CameraPoint(CameraStation myStation, string myCode)
            : base(myStation, myCode)
        {
        }


    }

}
