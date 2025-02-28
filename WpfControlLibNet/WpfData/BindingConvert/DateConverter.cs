using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(DateTime), typeof(string))]
	public class DateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime)value).ToShortDateString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string strValue = value as string;
			if (DateTime.TryParse(strValue, out var resultDateTime))
			{
				return resultDateTime;
			}
			return DependencyProperty.UnsetValue;
		}
	}
}
