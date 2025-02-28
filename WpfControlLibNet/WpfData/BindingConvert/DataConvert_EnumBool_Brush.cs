using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfData.BindingConvert
{
	[ValueConversion(typeof(Enum), typeof(Brush))]
	public class DataConvert_EnumBool_Brush<Ttype> : IValueConverter where Ttype : Enum
	{
		public Ttype TrueValue { get; set; }

		public Brush TrueBrush { get; set; } = Brushes.Red;


		public Brush FalseBrush { get; set; } = Brushes.Black;


		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return FalseBrush;
			}
			Ttype data = (Ttype)value;
			if (object.Equals(data, TrueValue))
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
				return TrueValue;
			}
			return 0;
		}
	}
}
