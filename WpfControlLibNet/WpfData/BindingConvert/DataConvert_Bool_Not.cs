using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(bool), typeof(bool))]
	public class DataConvert_Bool_Not : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool data = (bool)value;
			return !data;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool data = (bool)value;
			return !data;
		}
	}
}
