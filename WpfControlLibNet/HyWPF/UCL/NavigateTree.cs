using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PsMachineWork;

namespace HyWPF.UCL
{
	public class NavigateTree : TreeView
	{
		public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(iWorkStation), typeof(NavigateTree), new PropertyMetadata(null, DataSource_PropertyChangedCallback));

		public static DependencyProperty FlowListHeaderProperty = DependencyProperty.Register("FlowListHeader", typeof(DataTemplate), typeof(NavigateTree), new PropertyMetadata(null, FlowListHeader_PropertyChangedCallback));

		public static DependencyProperty StationListHeaderProperty = DependencyProperty.Register("StationListHeader", typeof(DataTemplate), typeof(NavigateTree), new PropertyMetadata(null, StationListHeader_PropertyChangedCallback));

		public static DependencyProperty StationHeaderProperty = DependencyProperty.Register("StationHeader", typeof(DataTemplate), typeof(NavigateTree), new PropertyMetadata(null, StationHeader_PropertyChangedCallback));

		public static DependencyProperty TrigListHeaderProperty = DependencyProperty.Register("TrigListHeader", typeof(DataTemplate), typeof(NavigateTree), new PropertyMetadata(null, TrigListHeader_PropertyChangedCallback));

		public static DependencyProperty TaskListHeaderProperty = DependencyProperty.Register("TaskListHeader", typeof(DataTemplate), typeof(NavigateTree), new PropertyMetadata(null, TaskListHeader_PropertyChangedCallback));

		public iWorkStation DataSource
		{
			get
			{
				return (iWorkStation)GetValue(DataSourceProperty);
			}
			set
			{
				SetValue(DataSourceProperty, value);
			}
		}

		public DataTemplate FlowListHeader
		{
			get
			{
				return (DataTemplate)GetValue(FlowListHeaderProperty);
			}
			set
			{
				SetValue(FlowListHeaderProperty, value);
			}
		}

		public DataTemplate StationListHeader
		{
			get
			{
				return (DataTemplate)GetValue(StationListHeaderProperty);
			}
			set
			{
				SetValue(StationListHeaderProperty, value);
			}
		}

		public DataTemplate StationHeader
		{
			get
			{
				return (DataTemplate)GetValue(StationHeaderProperty);
			}
			set
			{
				SetValue(StationHeaderProperty, value);
			}
		}

		public DataTemplate TrigListHeader
		{
			get
			{
				return (DataTemplate)GetValue(TrigListHeaderProperty);
			}
			set
			{
				SetValue(TrigListHeaderProperty, value);
			}
		}

		public DataTemplate TaskListHeader
		{
			get
			{
				return (DataTemplate)GetValue(TaskListHeaderProperty);
			}
			set
			{
				SetValue(TaskListHeaderProperty, value);
			}
		}

		private static void DataSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NavigateTree myUcl = d as NavigateTree;
			iWorkStation myStation = e.NewValue as iWorkStation;
			if (myStation == null)
			{
				myUcl.Items.Clear();
			}
			else
			{
				myUcl.mdsLoadStationNotes(myStation, null);
			}
		}

		private static void FlowListHeader_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static void StationListHeader_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static void StationHeader_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static void TrigListHeader_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static void TaskListHeader_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private void mdsLoadStationNotes(iWorkStation myWorkStation, BindingTreeViewItem myParentStationNote)
		{
			BindingTreeViewItem myStationNote = null;
			if (myParentStationNote == null)
			{
				base.Items.Clear();
				myStationNote = new BindingTreeViewItem();
				myStationNote.DataSource = myWorkStation;
				myStationNote.Header = new ContentPresenter
				{
					Content = myWorkStation,
					ContentTemplate = StationHeader
				};
				base.Items.Add(myStationNote);
			}
			else
			{
				myStationNote = new BindingTreeViewItem();
				myStationNote.DataSource = myWorkStation;
				myStationNote.Header = new ContentPresenter
				{
					Content = myWorkStation,
					ContentTemplate = StationHeader
				};
				myParentStationNote.Items.Add(myStationNote);
			}
			BindingTreeViewItem myTrigList = new BindingTreeViewItem();
			myTrigList.DataSource = myWorkStation.ChildsTrigger;
			myTrigList.Header = new ContentPresenter
			{
				Content = myWorkStation.ChildsTrigger,
				ContentTemplate = TrigListHeader
			};
			myStationNote.Items.Add(myTrigList);
			BindingTreeViewItem myFlowList = new BindingTreeViewItem();
			myFlowList.DataSource = myWorkStation.ChildsWorkFlow;
			myFlowList.Header = new ContentPresenter
			{
				Content = myWorkStation.ChildsWorkFlow,
				ContentTemplate = FlowListHeader
			};
			myStationNote.Items.Add(myFlowList);
			if (myWorkStation.ChildsStaion.Count() <= 0)
			{
				return;
			}
			BindingTreeViewItem myStationList = new BindingTreeViewItem();
			myStationList.DataSource = myWorkStation.ChildsStaion;
			myStationList.Header = new ContentPresenter
			{
				Content = myWorkStation.ChildsStaion,
				ContentTemplate = StationListHeader
			};
			myStationNote.Items.Add(myStationList);
			foreach (iWorkStation myItem in myWorkStation.ChildsStaion)
			{
				mdsLoadStationNotes(myItem, myStationList);
			}
		}

		public void LoadTaskNode()
		{
			BindingTreeViewItem myTaskListNote = new BindingTreeViewItem();
			iWorkTaskManager mySource = (iWorkTaskManager)(myTaskListNote.DataSource = TaskManagerProvider.DefaultTaskManager);
			myTaskListNote.Header = new ContentPresenter
			{
				Content = mySource,
				ContentTemplate = TaskListHeader
			};
			base.Items.Add(myTaskListNote);
		}
	}
}
