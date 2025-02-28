using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(Enum), typeof(Brush))]
	public class DataConvert_Bool_Brush : IValueConverter
	{
		public Brush TrueBrush { get; set; } = Brushes.Red;


		public Brush FalseBrush { get; set; } = Brushes.Black;


		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return TrueBrush;
			}
			return FalseBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush data = (Brush)value;
			if (data == TrueBrush)
			{
				return true;
			}
			return false;
		}
	}
}
