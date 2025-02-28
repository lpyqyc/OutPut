using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	public class FirstNameLastNameToFullNameConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values != null)
			{
				return values[0].ToString() + " " + values[1].ToString();
			}
			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			string[] values = null;
			if (value != null && value.ToString() != "")
			{
				return values = value.ToString()!.Split(' ');
			}
			return null;
		}
	}
}
