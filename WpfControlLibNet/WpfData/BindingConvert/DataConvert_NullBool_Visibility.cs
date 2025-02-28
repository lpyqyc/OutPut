using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(bool?), typeof(Visibility))]
	public class DataConvert_NullBool_Visibility : IValueConverter
	{
		public Visibility NullDefautValue { get; set; } = Visibility.Collapsed;


		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? myValue = (bool?)value;
			if (!myValue.HasValue)
			{
				return NullDefautValue;
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
