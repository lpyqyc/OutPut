using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(int?), typeof(string))]
	public class DataConvert_NullInt_String : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			return ((int?)value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string data = (string)value;
			if (data == null)
			{
				return null;
			}
			return int.Parse(data);
		}
	}
}
