using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(bool?), typeof(bool))]
	public class DataConvert_NullBool_Not : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? myValue = (bool?)value;
			if (!myValue.HasValue)
			{
				return true;
			}
			return !myValue.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool myValue = (bool)value;
			return !myValue;
		}
	}
}
