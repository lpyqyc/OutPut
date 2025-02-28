using System.Windows.Controls;

namespace HyWPF.UCL
{
	public class HyDataGrid : DataGrid
	{
		public HyDataGrid()
		{
			base.SelectionChanged += HyDataGrid_SelectionChanged;
			base.SelectionMode = DataGridSelectionMode.Single;
			base.SelectionUnit = DataGridSelectionUnit.FullRow;
		}

		private void HyDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			if (base.SelectedItem != null)
			{
				ScrollIntoView(base.SelectedItem);
			}
		}

		private void HyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (base.SelectedItem != null)
			{
				ScrollIntoView(base.SelectedItem);
			}
		}
	}
}
