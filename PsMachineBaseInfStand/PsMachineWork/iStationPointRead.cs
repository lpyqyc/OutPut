namespace PsMachineWork
{
	/// <summary>读取值
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface iStationPointRead<T>
	{
		T ValueObj { get; }

		T ReadValue();
	}
}
