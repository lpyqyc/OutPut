namespace PsMachineWork
{
	/// <summary>读取值
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface iStationPointWrite<T>
	{
		T ValueObj { get; }

		void WriteValue(T value);
	}
}
