namespace PsMachineWork
{
	/// <summary>任务管理器提供者
	/// </summary>
	public static class TaskManagerProvider
	{
		private static iWorkTaskManager _DefaultTaskManager;

		public static iWorkTaskManager DefaultTaskManager
		{
			get
			{
				if (_DefaultTaskManager == null)
				{
					_DefaultTaskManager = new WorkTaskRunCollection();
				}
				return _DefaultTaskManager;
			}
		}
	}
}
