using System.Reflection;

namespace PsMachineWork
{
	/// <summary>
	/// </summary>
	public class TriggerConditionAttrData
	{
		public MethodInfo MethodInfo { get; set; }

		public TriggerConditionAttribute Attribute { get; set; }

		public string GetID()
		{
			return MethodInfo.Name;
		}

		public string GetDesc()
		{
			string myDesc = Attribute?.Desc;
			if (string.IsNullOrEmpty(myDesc))
			{
				myDesc = MethodInfo.Name;
			}
			return myDesc;
		}
	}
}
