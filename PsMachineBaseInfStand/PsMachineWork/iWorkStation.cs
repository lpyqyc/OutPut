using System.ComponentModel;
using System.Threading;

namespace PsMachineWork
{
	/// <summary>工作站点接口
	/// </summary>
	public interface iWorkStation : INotifyPropertyChanged
	{
		/// <summary>所属的上一级工作站
		/// </summary>
		iWorkStation ParentStation { get; }

		/// <summary>包含的子工作站
		/// </summary>
		iWorkStationCollection<iWorkStation> ChildsStaion { get; }

		/// <summary>包含的直属点位
		/// </summary>
		iStationPointCollection<iStationPoint> ChildsPoint { get; }

		/// <summary>包含的触发器集合
		/// </summary>
		iWorkTriggerCollection<iWorkTrigger> ChildsTrigger { get; }

		/// <summary>包含的执行工作流
		/// </summary>
		iWorkFlowCollection<iWorkFlow> ChildsWorkFlow { get; }

		/// <summary>运行日志
		/// </summary>
		ILogManager LogManager { get; }

		/// <summary>站位的本地编码
		/// </summary>
		string Code { get; }

		/// <summary>注释说明
		/// </summary>
		string Desc { get; }

		/// <summary>是否连接
		/// </summary>
		bool IsConnection { get; }

		/// <summary>连接异常
		/// </summary>
		bool IsException { get; }

		/// <summary>是否支持断线后自动重新连接
		/// </summary>
		bool IsAutoConnection { get; }

		/// <summary>工作站是否已经关闭并释放资源
		/// </summary>
		bool IsClose { get; }

		/// <summary>加载数据结构对象
		/// </summary>
		void Init_LoadData();

		/// <summary>连接工作站及下属工作站
		/// </summary>
		void Connection(CancellationToken myCancellationToken);

		/// <summary>启动工作对象自检Task
		/// </summary>
		void Init_StarTask();

		void Close(bool myAllChildsStation = true);
	}
}
