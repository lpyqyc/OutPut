using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PsMachineWork;
using WpfData.BindingConvert;

namespace HyWPF.UCL
{
	public class StationActionBar : StackPanel
	{
		public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(StationActionBar), new PropertyMetadata(null, DataSource_PropertyChangedCallback));

		private List<FrameworkElement> _SourceItems = new List<FrameworkElement>();

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

		public StationActionBar()
		{
			base.Orientation = Orientation.Horizontal;
		}

		private static void DataSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StationActionBar myThis = (StationActionBar)d;
			myThis.mdsClearOldSource(e.OldValue);
			if (e.NewValue != null)
			{
				myThis.mdsAddNewSource(e.NewValue);
			}
		}

		private void mdsClearOldSource(object myOldValue)
		{
			foreach (FrameworkElement item in _SourceItems)
			{
				base.Children.Remove(item);
			}
		}

		private void mdsAddNewSource(object myNewValue)
		{
			List<ActionAttrData> myActionDataList = StationHelp.GetActionList(myNewValue);
			foreach (ActionAttrData myItem in myActionDataList)
			{
				ToolSimpleButton myButton = new ToolSimpleButton
				{
					Content = myItem.GetCaption(),
					ActionData = myItem,
					MethodInfo = myItem.MethodInfo,
					BindingObject = myNewValue,
					Margin = new Thickness(4.0, 2.0, 4.0, 2.0),
					Padding = new Thickness(6.0, 2.0, 6.0, 2.0),
					MinWidth = 60.0
				};
				if (!string.IsNullOrEmpty(myItem.Attribue.VisibleProperty))
				{
					Binding myBinding2 = new Binding(myItem.Attribue.VisibleProperty);
					myBinding2.Source = myItem.DataSource;
					myBinding2.Converter = new DataConvert_Bool_Visibility();
					myButton.SetBinding(UIElement.VisibilityProperty, myBinding2);
				}
				if (!string.IsNullOrEmpty(myItem.Attribue.CriteriaProperty))
				{
					Binding myBinding = new Binding(myItem.Attribue.CriteriaProperty);
					myBinding.Source = myItem.DataSource;
					myButton.SetBinding(UIElement.IsEnabledProperty, myBinding);
				}
				_SourceItems.Add(myButton);
				base.Children.Add(myButton);
			}
		}
	}
}
