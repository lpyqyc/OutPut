using System.ComponentModel;

namespace HyWPF.UCL
{
	public class MessageData : INotifyPropertyChanged
	{
		private string _Message;

		private bool _IsException;

		private int _TimeLen;

		public string Message
		{
			get
			{
				return _Message;
			}
			set
			{
				if (_Message != value)
				{
					_Message = value;
					OnPropertyChanged("Message");
				}
			}
		}

		public bool IsException
		{
			get
			{
				return _IsException;
			}
			set
			{
				if (_IsException != value)
				{
					_IsException = value;
					OnPropertyChanged("IsException");
				}
			}
		}

		public int TimeLen
		{
			get
			{
				return _TimeLen;
			}
			set
			{
				if (_TimeLen != value)
				{
					_TimeLen = value;
					OnPropertyChanged("TimeLen");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
