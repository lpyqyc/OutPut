using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(double), typeof(double))]
	public class DataConvert_Double_MaxDouble : IValueConverter
	{
		public double RemoveDouble { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double data = (double)value;
			if (double.IsNaN(data))
			{
				data = 460.0;
			}
			return data - RemoveDouble;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double data = (double)value;
			return data + RemoveDouble;
		}
	}
}
