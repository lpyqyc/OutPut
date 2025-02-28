using System.Collections;
using System.Windows;
using PsMachineTools.Tools;

namespace HyWPF.UCL
{
	public class CollectionControl : FrameworkElement
	{
		public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(CollectionControl), new PropertyMetadata(null, DataSource_PropertyChangedCallback));

		public IEnumerable DataSource
		{
			get
			{
				return (IEnumerable)GetValue(DataSourceProperty);
			}
			set
			{
				SetValue(DataSourceProperty, value);
			}
		}

		public UiCollection UiCollection { get; private set; }

		public CollectionControl()
		{
			UiCollection = new UiCollection();
		}

		private static void DataSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionControl myThis = (CollectionControl)d;
			myThis.UiCollection.DataSource = (IEnumerable)e.NewValue;
		}
	}
}
