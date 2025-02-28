namespace PsMachineWork
{
	/// <summary>工作流引用的工作站
	/// </summary>
	public interface iWorkFlowRefStation
	{
		/// <summary>所在工作流
		/// </summary>
		iWorkFlow WorkFlow { get; }

		/// <summary>引用的工作站
		/// </summary>
		iWorkStation RefStation { get; }

		/// <summary>工作站的本地引用名，相当于属性名称
		/// 比如运输任务中，起始工作站，终止工作站
		/// </summary>
		string RefStationCode { get; }
	}
}
