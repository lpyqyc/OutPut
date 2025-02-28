using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace PsCameraNet
{
    /// <summary>条码信息
    /// </summary>
    public class CodeInfo
    {

        public emCodeType CodeType
        { get; set; }

        public string CodeValue
        { get; set; }

        public double CenterX
        { get; set; }

        public double CenterY
        { get; set; }

        /// <summary>条码与中心点的偏移角度
        /// </summary>
        public double CenterAngle
        { get; set; }

        /// <summary>条码方向偏移角度, 顺时针旋转偏移为正，逆时针旋转偏移为负数，一般是正 180 到负180之间
        /// </summary>
        public double Orientition
        { get; set; }

    }


}
