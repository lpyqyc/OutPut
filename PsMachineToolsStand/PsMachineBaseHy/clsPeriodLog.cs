using System;

namespace PsMachineBaseHy
{
	/// <summary>间隔性输出日志
	/// </summary>
	internal class clsPeriodLog
	{
		/// <summary>消息Log
		/// </summary>
		public string Message;

		/// <summary>最后输出时间
		/// </summary>
		public DateTime LastOutTime { get; set; }
	}
}
