using System;
using System.Collections.Generic;
using System.Text;

namespace PsCameraNet
{
    /// <summary>物料条码识别结果
    /// </summary>
    public class BarcodeCodeScanRusult
    {

        /// <summary>识别是否OK
        /// </summary>
        public bool IsOK
        { get; set; }


        /// <summary>拍照时锁定的需求时间
        /// 如果解析完成时，时间不对，则不能写入此值
        /// </summary>
        public DateTime? IssueTime
        { get; set; }


        /// <summary>返回结果的时间
        /// </summary>
        public DateTime? ResultTime
        { get; set; }


        /// <summary>读取的条码结果
        /// </summary>
        public List<CodeInfo> CodeList
        { get; set; } = new List<CodeInfo>();


        /// <summary>解析识别时的异常消息
        /// </summary>
        public string ExceptionMsg
        { get; set; }



    }

}
