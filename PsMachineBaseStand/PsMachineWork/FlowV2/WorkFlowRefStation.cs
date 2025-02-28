namespace PsMachineWork.FlowV2
{
	/// <summary>工作流引用工作站
	/// </summary>
	public class WorkFlowRefStation : iWorkFlowRefStation
	{
		/// <summary>所在工作流
		/// </summary>
		public iWorkFlow WorkFlow { get; set; }

		/// <summary>引用的工作站
		/// </summary>
		public iWorkStation RefStation { get; set; }

		/// <summary>工作站的本地引用名，相当于属性名称
		/// 比如运输任务中，起始工作站，终止工作站
		/// </summary>
		public string RefStationCode { get; set; }

		public WorkFlowRefStation(iWorkFlow myFlow, string myRefLocalCode)
		{
			RefStationCode = myRefLocalCode;
			((iAddItem)myFlow.RefStationList).Add(this);
		}
	}
}
