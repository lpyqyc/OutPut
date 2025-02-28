namespace PsMachineWork
{
	/// <summary>向集合中添加一个新成员 接口
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface iAddItem
	{
		/// <summary>向集合中添加一个新成员
		/// </summary>
		/// <param name="item"></param>
		void Add(object item);
	}
}
