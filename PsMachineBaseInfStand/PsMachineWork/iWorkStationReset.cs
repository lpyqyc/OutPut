using System.Threading;

namespace PsMachineWork
{
	/// <summary>工作站支持重新复位
	/// </summary>
	public interface iWorkStationReset
	{
		/// <summary>是否允许复位
		/// </summary>
		bool IsAllowReset { get; }

		void Reset(CancellationToken myCancellationToken);
	}
}
