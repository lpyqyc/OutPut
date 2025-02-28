using System.Reflection;

namespace PsMachineWork
{
	/// <summary>从 FlowStepAttribute 
	/// </summary>
	public class FlowStepAttrData
	{
		public MethodInfo MethodInfo { get; set; }

		public FlowStepAttribute Attribute { get; set; }

		public string StepCode => Attribute?.Code;
	}
}
