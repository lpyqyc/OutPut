using System.Windows.Controls;

namespace HyWPF.UCL
{
	public class LogListBox : ListBox
	{
		public LogListBox()
		{
			base.SelectionChanged += lbxLogList_SelectionChanged;
		}

		private void lbxLogList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				ListBox myLbxBox = sender as ListBox;
				if (myLbxBox.SelectedItem != null)
				{
					myLbxBox.ScrollIntoView(myLbxBox.SelectedItem);
				}
			}
			catch
			{
			}
		}
	}
}
