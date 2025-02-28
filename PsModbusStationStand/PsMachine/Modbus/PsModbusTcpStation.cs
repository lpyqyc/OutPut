using IoTClient.Clients.Modbus;
using Newtonsoft.Json.Linq;
using PsMachineWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Threading;

namespace PsMachine.Modbus
{


    /// <summary>Modbus 工作站
    /// </summary>
    public abstract class PsModbusTcpStation : PsWorkStationBase<PsModbusTcpStation, PsModbusTcpPoint>
    {

        public PsModbusTcpStation(string myCode)
            : base(myCode)
        {
        }


        #region  参数  ---------------------------------------------------------------


        /// <summary>IP地址
        /// </summary>
        [StationPara]
        public string Para_ModbusHost
        { get; set; }

        /// <summary>端口号,默认502
        /// </summary>
        [StationPara]
        public int Para_ModbusPort
        { get; set; } = 502;

        /// <summary>数据格式
        /// </summary>
        public IoTClient.Enums.EndianFormat Para_EndianFormat
        { get; set; } = IoTClient.Enums.EndianFormat.CDAB;


        #endregion  //参数  ==========================================================


        #region  Modbus读写功能  ----------------------------------------------------------


        /// <summary>控制器
        /// </summary>
        public ModbusTcpClient ModbusTcpClient
        {
            get
            {
                if (_ModbusTcpClient == null)
                {
                    _ModbusTcpClient = mdsCreateModbusTcpClient();
                }
                return _ModbusTcpClient;
            }
            private set
            {
                var myChanged = _ModbusTcpClient != value;
                if (myChanged)
                {
                    _ModbusTcpClient = value;
                    OnPropertyChanged(nameof(ModbusTcpClient));
                }
            }
        }
        ModbusTcpClient _ModbusTcpClient;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private ModbusTcpClient mdsCreateModbusTcpClient()
        {
            var myValue = new ModbusTcpClient(Para_ModbusHost, Para_ModbusPort, 1500, Para_EndianFormat, false);

            return myValue;
        }

        protected override void mdsResetConnection(CancellationToken myCancellationToken)
        {
            mdsCloseConnection();

            var myPlc = ModbusTcpClient = mdsCreateModbusTcpClient();

            try
            {
                var myConnectionRtn = myPlc.Open();
                IsConnection = myConnectionRtn.IsSucceed;
            }
            catch (Exception ex)
            {
                LogManager.AddMachineLog($"Modbus ({Para_ModbusHost}:{Para_ModbusPort}) 连接异常: {ex.Message}");
            }
        }

        protected override void mdsCloseConnection()
        {
            try
            {
                if (ModbusTcpClient != null)
                {
                    ModbusTcpClient.Close();
                }
            }
            catch { }
            ModbusTcpClient = null;

            IsConnection = false;
            IsException = false;
        }


        /// <summary>获取点位值, 支持类型 int16,unit16,int32,uint32, bool, float
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public void GetPointValue<T>(string address, out T value)
        {
            string myValueTxt;
            try
            {
                lock (m_Lock_ReadWritePoint)
                {
                    mdsGetPointValue(address, out value, out myValueTxt);
                }
            }
            catch (Exception ex)
            {
                IsException = true;
                var myMsgKey = "读取点位异常";
                var myExpMsg = $"读取点位[{address}]异常: {ex.Message}";
                LogManager.AddMachineKeyLog(myExpMsg, null, true, myMsgKey, 1000);
                throw new Exception(myExpMsg);
            }
        }

        /// <summary>写入点位值,支持类型 int16,unit16,int32,uint32, bool, float
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="myValue"></param>
        /// <exception cref="Exception"></exception>
        public void SetPointValue<T>(string address, T myValue)
        {
            string myValueTxt = null;
            try
            {
                lock (m_Lock_ReadWritePoint)
                {
                    mdsSetPointValue(address, myValue, out myValueTxt);
                }
            }
            catch (Exception ex)
            {
                IsException = true;
                var myExpMsg = $"写入点位[{address}]={myValueTxt} 异常: {ex.Message}";
                LogManager.AddMachineLog(myExpMsg);
                throw new Exception(myExpMsg);
            }
        }


        private void mdsGetPointValue<T>(string address, out T value, out string myValueTxt)
        {
            Type myType = typeof(T);
            myValueTxt = null;
            var myClient = ModbusTcpClient;
            if (myType == typeof(short))
            {
                var myResult = myClient.ReadInt16(address);
                if (myResult.IsSucceed)
                {
                    value = (T)(object)myResult.Value;
                    myValueTxt = myResult.Value.ToString();
                }
                else
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(ushort))
            {
                var myResult = myClient.ReadUInt16(address);
                if (myResult.IsSucceed)
                {
                    value = (T)(object)myResult.Value;
                    myValueTxt = myResult.Value.ToString();
                }
                else
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(bool))
            {
                var myResult = myClient.ReadCoil(address);
                if (myResult.IsSucceed)
                {
                    value = (T)(object)myResult.Value;
                    myValueTxt = myResult.Value.ToString();
                }
                else
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(float))
            {
                var myResult = myClient.ReadFloat(address);
                if (myResult.IsSucceed)
                {
                    value = (T)(object)myResult.Value;
                    myValueTxt = myResult.Value.ToString();
                }
                else
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(int))
            {
                var myResult = myClient.ReadInt32(address);
                if (myResult.IsSucceed)
                {
                    value = (T)(object)myResult.Value;
                    myValueTxt = myResult.Value.ToString();
                }
                else
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(uint))
            {
                var myResult = myClient.ReadUInt32(address);
                if (myResult.IsSucceed)
                {
                    value = (T)(object)myResult.Value;
                    myValueTxt = myResult.Value.ToString();
                }
                else
                {
                    throw new Exception(myResult.Err);
                }
            }
            else
            {
                throw new Exception($"不支持的点位值类型{myType.Name}");
            }

            if (IsLog_ReadPoint)  //写入日志
            {
                if (myValueTxt == null || myValueTxt.Length < 500)
                {
                    LogManager.AddMachineLog($"读取点位 [{address}] = {myValueTxt}");
                }
                else
                {
                    LogManager.AddMachineLog($"读取点位 [{address}] = {myValueTxt.Substring(0, 500)}...");
                }
            }
        }


        private void mdsSetPointValue<T>(string address, T myValue, out string myValueTxt)
        {
            Type myType = typeof(T);
            var myClient = ModbusTcpClient;
            if (myType == typeof(short))
            {
                var myValue2 = (short)(object)myValue;
                myValueTxt = myValue.ToString();
                var myResult = myClient.Write(address, myValue2);
                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(ushort))
            {
                var myValue2 = (ushort)(object)myValue;
                myValueTxt = myValue.ToString();
                var myResult = myClient.Write(address, myValue2);
                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(bool))
            {
                var myValue2 = (bool)(object)myValue;
                myValueTxt = myValue.ToString();
                var myResult = myClient.Write(address, myValue2);
                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(float))
            {
                var myValue2 = (float)(object)myValue;
                myValueTxt = myValue.ToString();
                var myResult = myClient.Write(address, myValue2);
                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(int))
            {
                var myValue2 = (int)(object)myValue;
                myValueTxt = myValue.ToString();
                var myResult = myClient.Write(address, myValue2);
                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(uint))
            {
                var myValue2 = (uint)(object)myValue;
                myValueTxt = myValue.ToString();
                var myResult = myClient.Write(address, myValue2);
                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else
            {
                throw new Exception($"不支持的点位值类型{myType.Name}");
            }

            if (IsLog_WritePoint) //写入日志
            {
                if (myValueTxt == null || myValueTxt.Length < 500)
                {
                    LogManager.AddMachineLog($"写入PLC点位 [{address}] = {myValueTxt}");
                }
                else
                {
                    LogManager.AddMachineLog($"写入PLC点位 [{address}] = {myValueTxt.Substring(0, 500)}...");
                }
            }
        }


        #endregion  //Modbus读写功能  =====================================================



    }

}
