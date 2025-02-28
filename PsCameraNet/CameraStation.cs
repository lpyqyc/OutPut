using PsMachineWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PsCameraNet
{
    /// <summary>相机站位
    /// </summary>
    public class CameraStation : PsWorkStationBase<CameraStation, CameraPoint>
    {

        public CameraStation(string myCode)
            : base(myCode)
        {
        }

        public override void Init_LoadData()
        {
            mdsInitAttributePoint();
        }


        protected override void mdsCloseConnection()
        {
            this.IsConnection = false;
            this.IsException = false;
        }

        protected override void mdsResetConnection(CancellationToken myCancellationToken)
        {
            this.IsConnection = true;
        }


        #region  引用资源  -------------------------------------------------------------


        /// <summary>拍照需求时间，当前有新的需求时，写入时间
        /// 当料盘移走时，清空此值, 拍照前需要获取和锁定此时间，拍照返回结果时，如果时间与此值对不上，需要清空
        /// </summary>
        [StationPoint("NeedTime", "拍照需求时间", emPointReadWriteType.ReadWrite, false, IsLocalDataPoint = true)]
        public CameraPoint<DateTime?> Point_NeedTime
        { get; protected set; }


        /// <summary>拍照识别结果, null 未执行或者正在执行, false 识别失败, True 识别成功
        /// </summary>
        [StationPoint("Scaned", "拍照识别结果", emPointReadWriteType.ReadWrite, false, IsLocalDataPoint = true)]
        public CameraPoint<BarcodeCodeScanRusult> Point_Scaned
        { get; protected set; }


        #endregion  //引用资源  ========================================================




    }

}
