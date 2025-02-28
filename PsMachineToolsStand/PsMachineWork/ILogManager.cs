using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PsMachineWork
{
	/// <summary>日志的管理器接口
	/// </summary>
	public interface ILogManager : IEnumerable<ILogData>, IEnumerable, INotifyCollectionChanged
	{
		/// <summary>添加新消息
		/// </summary>
		/// <param name="item"></param>
		void Add(ILogData item);

		/// <summary>添加新消息
		/// </summary>
		void AddMachineLog(Exception ex);

		/// <summary>添加新消息
		/// </summary>
		/// <param name="myMsg"></param>
		/// <param name="myIsException">是否异常消息</param>
		void AddMachineLog(string myMsg, bool myIsException = false);

		/// <summary>添加新消息
		/// </summary>
		/// <param name="myMsg"></param>
		/// <param name="myDesc">消息的详细注释</param>
		/// <param name="myIsException">是否异常消息</param>
		void AddMachineLog(string myMsg, string myDesc, bool myIsException = false);

		/// <summary>添加新消息, 但能够防一定时间内重复输出功能, 用于防止在循环体内,短时间内重复输出一堆相同的异常消息
		/// </summary>
		/// <param name="myMsg">主消息</param>
		/// <param name="myMsgDesc">消息的详细注释</param>
		/// <param name="myIsException">是否异常消息</param>
		/// <param name="myMsgKey">消息的KEY, 用于防止重复消息时使用</param>
		/// <param name="myTimeOut">消息的有效期限,以毫秒为单位,在此时间内输出相同KEY的消息会自动过滤掉不添加</param>
		void AddMachineKeyLog(string myMsg, string myMsgDesc, bool myIsException, string myMsgKey, int myTimeOut = 1000);
	}
}
