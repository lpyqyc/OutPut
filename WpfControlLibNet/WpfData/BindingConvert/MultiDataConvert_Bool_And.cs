using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfData.BindingConvert
{
	public class MultiDataConvert_Bool_And : IMultiValueConverter
	{
		public bool DefaultValue { get; set; } = false;


		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values != null)
			{
				bool myAnd = true;
				for (int i = 0; i < values.Length; i++)
				{
					bool myItem = (bool)values[i];
					myAnd = myAnd && myItem;
				}
				return myAnd;
			}
			return DefaultValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
