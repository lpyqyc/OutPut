using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(bool?), typeof(Visibility))]
	public class DataConvert_NullBool2_Visibility : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? myValue = (bool?)value;
			if (!myValue.HasValue)
			{
				return Visibility.Visible;
			}
			if (myValue == false)
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
