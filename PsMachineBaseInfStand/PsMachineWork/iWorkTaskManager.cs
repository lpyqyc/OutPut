using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace PsMachineWork
{
	/// <summary>工作线程集合的接口
	/// </summary>
	public interface iWorkTaskManager : iWorkTaskRunCollection<iWorkTaskRun>, IEnumerable<iWorkTaskRun>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		/// <summary>创建一个新的执行线程，如果KEY已经存在，则会往这个执行线程中添加新的入口函数，多个入口函数时，会循环执行
		/// </summary>
		/// <param name="myTaskID">一般不能为null</param>
		/// <param name="myRunFunc"></param>
		/// <param name="myTimeSleep">此Task 执行完一个循环之后,间隔的Delay时间,以毫秒为单位</param>
		/// <returns></returns>
		iWorkTaskRun CreateTask(string myTaskID, string myDesc, Func<CancellationToken, bool> myRunFunc, int myTimeDelay = 1000);

		/// <summary>关闭
		/// </summary>
		void Close();
	}
}
