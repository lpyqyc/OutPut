using System;
using System.ComponentModel;
using System.Threading;

namespace PsMachineWork
{
	/// <summary>一个TASK 中执行的入口函数之一
	/// </summary>
	internal class WorkTaskRunFunc : iWorkTaskRunFunc, INotifyPropertyChanged
	{
		private DateTime? _LastRunTime;

		/// <summary>任务注释
		/// </summary>
		public string TaskDesc { get; internal set; }

		/// <summary>最后执行时间
		/// </summary>
		public DateTime? LastRunTime
		{
			get
			{
				return _LastRunTime;
			}
			internal set
			{
				if (_LastRunTime != value)
				{
					_LastRunTime = value;
					OnPropertyChanged("LastRunTime");
				}
			}
		}

		/// <summary>任务的执行函数
		/// </summary>
		public Func<CancellationToken, bool> Func { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
