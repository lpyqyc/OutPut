using System.Reflection;

namespace PsMachineWork
{
	/// <summary>工作站点位声明特性
	/// </summary>
	public class StationPointAttrData
	{
		public PropertyInfo PropertyInfo { get; set; }

		public StationPointAttribute Attribute { get; set; }
	}
}
