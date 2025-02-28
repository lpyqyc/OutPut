using System;
using System.ComponentModel;
using PsMachineWork;

namespace PsMachineBaseHy
{
	/// <summary>简单的应用程序日志
	/// </summary>
	public class LogData : ILogData, INotifyPropertyChanged
	{
		private bool _IsShowDesc;

		/// <summary>创建时间
		/// </summary>
		public DateTime CreateTime { get; private set; }

		/// <summary>消息主体
		/// </summary>
		public string Message { get; set; }

		/// <summary>消息附加的详细内容
		/// </summary>
		public string Desc { get; set; }

		/// <summary>是否属于异常消息
		/// </summary>
		public bool IsException { get; set; }

		/// <summary>消息和时间+文本提示
		/// </summary>
		public string TimeAndMessage => CreateTime.ToString("HH:mm:ss") + " " + Message;

		/// <summary>显示日志和附加消息
		/// </summary>
		public bool IsShowDesc
		{
			get
			{
				return _IsShowDesc;
			}
			set
			{
				if (_IsShowDesc != value)
				{
					_IsShowDesc = value;
					OnPropertyChanged("IsShowDesc");
				}
			}
		}

		/// <summary>属性变更事件
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>创建简单的应用程序日志
		/// </summary>
		public LogData()
		{
			CreateTime = DateTime.Now;
		}

		/// <summary>输出字符串文本，含消息主体和附加消息
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return TimeAndMessage + ((Desc != null) ? "\r\n" : null) + Desc;
		}

		/// <summary>虚拟触发属性变更事件
		/// </summary>
		/// <param name="myPropertyName"></param>
		public void OnPropertyChanged(string myPropertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(myPropertyName));
		}
	}
}
