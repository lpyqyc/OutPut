using System.Windows;
using System.Windows.Controls;

namespace HyWPF.UCL
{
	public class BindingTreeViewItem : TreeViewItem
	{
		public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(BindingTreeViewItem), new PropertyMetadata(null, DataSource_PropertyChangedCallback));

		public object DataSource
		{
			get
			{
				return GetValue(DataSourceProperty);
			}
			set
			{
				SetValue(DataSourceProperty, value);
			}
		}

		private static void DataSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
	}
}
