using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using PsMachineWork;

namespace HyWPF.UCL
{
	public class ToolSimpleButton : Button
	{
		public object BindingObject { get; internal set; }

		public ActionAttrData ActionData { get; internal set; }

		public MethodInfo MethodInfo { get; internal set; }

		public ToolSimpleButton()
		{
			base.Margin = new Thickness(4.0, 2.0, 4.0, 2.0);
			base.Click += ToolSimpleButton_Click;
		}

		private void ToolSimpleButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MethodInfo?.Invoke(BindingObject, null);
			}
			catch
			{
			}
		}
	}
}
