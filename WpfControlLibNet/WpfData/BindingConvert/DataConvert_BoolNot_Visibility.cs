using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class DataConvert_BoolNot_Visibility : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Visibility)value == Visibility.Visible)
			{
				return false;
			}
			return true;
		}
	}
}
