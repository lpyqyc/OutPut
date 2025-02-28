using IoTClient;
using IoTClient.Clients.Modbus;
using Newtonsoft.Json.Linq;
using PsMachineWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Net;
using System.Text;
using System.Threading;

namespace PsMachine.Modbus485
{


    /// <summary>Modbus 工作站
    /// </summary>
    public abstract class PsModbus485ComStation : PsWorkStationBase<PsModbus485ComStation, PsModbus485ComPoint>
    {

        public PsModbus485ComStation(string myCode)
            : base(myCode)
        {
        }


        /// <summary>COM口
        /// </summary>
        [StationPara]
        public string Para_ComPort
        { get; set; } = "COM1";


        /// <summary>COM 波特率
        /// </summary>
        [StationPara]
        public int Para_ComBaudRate
        { get; set; } = 9600;


        /// <summary> 获取或设置每个字节的数据位的标准长度。一般是8位
        /// </summary>
        [StationPara]
        public int Para_ComDataBits
        { get; set; } = 8;


        /// <summary> 获取或设置每个字节的标准 stopbits 数。
        /// </summary>
        [StationPara]
        public StopBits Para_ComStopBits
        { get; set; } = StopBits.None;


        /// <summary>获取或设置奇偶校验协议。
        /// </summary>
        [StationPara]
        public Parity Para_ComParity
        { get; set; } = Parity.None;


        /// <summary>数据格式
        /// </summary>
        [StationPara]
        public IoTClient.Enums.EndianFormat Para_EndianFormat
        { get; set; } = IoTClient.Enums.EndianFormat.CDAB;



        /// <summary>控制器
        /// </summary>
        public ModbusRtuClient ModbusClient
        {
            get
            {
                if (_ModbusTcpClient == null)
                {
                    _ModbusTcpClient = mdsCreateModbusClient();
                }
                return _ModbusTcpClient;
            }
            private set
            {
                var myChanged = _ModbusTcpClient != value;
                if (myChanged)
                {
                    _ModbusTcpClient = value;
                    OnPropertyChanged(nameof(ModbusClient));
                }
            }
        }
        ModbusRtuClient _ModbusTcpClient;


        /// <summary>
        /// </summary>
        /// <returns></returns>
        private ModbusRtuClient mdsCreateModbusClient()
        {
            var myValue = new ModbusRtuClient(Para_ComPort, Para_ComBaudRate, Para_ComDataBits, Para_ComStopBits, Para_ComParity, 1500, Para_EndianFormat, false);

            return myValue;
        }


        protected override void mdsResetConnection(CancellationToken myCancellationToken)
        {
            mdsCloseConnection();

            var myPlc = ModbusClient = mdsCreateModbusClient();

            try
            {
                var myConnectionRtn = myPlc.Open();
                IsConnection = myConnectionRtn.IsSucceed;
            }
            catch (Exception ex)
            {
                LogManager.AddMachineLog($"Modbus ({Para_ComPort}) 连接异常: {ex.Message}");
            }
        }


        protected override void mdsCloseConnection()
        {
            try
            {
                if (ModbusClient != null)
                {
                    ModbusClient.Close();
                }
            }
            catch { }
            ModbusClient = null;

            IsConnection = false;
            IsException = false;
        }


        /// <summary>获取点位值, 支持类型 int16,unit16,int32,uint32, bool, float
        /// </summary>
        /// <param name="station">485站点编码</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public void GetPointValue<T>(byte station, string address, out T value)
        {
            string myValueTxt;
            try
            {
                lock (m_Lock_ReadWritePoint)
                {
                    mdsGetPointValue(station, address, out value, out myValueTxt);
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
        /// <param name="station">485站点编码</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="myValue"></param>
        /// <exception cref="Exception"></exception>
        public void SetPointValue<T>(byte station, string address, T myValue)
        {
            string myValueTxt = null;
            try
            {
                lock (m_Lock_ReadWritePoint)
                {
                    mdsSetPointValue(station, address, myValue, out myValueTxt);
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


        private void mdsGetPointValue<T>(byte station, string address, out T value, out string myValueTxt)
        {
            Type myType = typeof(T);
            myValueTxt = null;
            var myClient = ModbusClient;
            if (myType == typeof(short))
            {
                Result<short> myResult;
                myResult = myClient.ReadInt16(address, station);

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
                Result<ushort> myResult;
                myResult = myClient.ReadUInt16(address, station);

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
                Result<bool> myResult;
                myResult = myClient.ReadCoil(address, station);

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
                Result<float> myResult;
                myResult = myClient.ReadFloat(address, station);

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
                Result<Int32> myResult;
                myResult = myClient.ReadInt32(address, station);

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
                Result<UInt32> myResult;
                myResult = myClient.ReadUInt32(address, station);

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


        private void mdsSetPointValue<T>(byte station, string address, T myValue, out string myValueTxt)
        {
            Type myType = typeof(T);
            var myClient = ModbusClient;
            if (myType == typeof(short))
            {
                var myValue2 = (short)(object)myValue;
                myValueTxt = myValue.ToString();

                Result myResult;
                myResult = myClient.Write(address, myValue2, station);

                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(ushort))
            {
                var myValue2 = (ushort)(object)myValue;
                myValueTxt = myValue.ToString();

                Result myResult;
                myResult = myClient.Write(address, myValue2, station);

                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(bool))
            {
                var myValue2 = (bool)(object)myValue;
                myValueTxt = myValue.ToString();

                Result myResult;
                myResult = myClient.Write(address, myValue2, station);

                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(float))
            {
                var myValue2 = (float)(object)myValue;
                myValueTxt = myValue.ToString();

                Result myResult;
                myResult = myClient.Write(address, myValue2, station);

                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(int))
            {
                var myValue2 = (int)(object)myValue;
                myValueTxt = myValue.ToString();

                Result myResult;
                myResult = myClient.Write(address, myValue2, station);

                if (!myResult.IsSucceed)
                {
                    throw new Exception(myResult.Err);
                }
            }
            else if (myType == typeof(uint))
            {
                var myValue2 = (uint)(object)myValue;
                myValueTxt = myValue.ToString();

                Result myResult;
                myResult = myClient.Write(address, myValue2, station);

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


    }

}
