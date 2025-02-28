namespace PsMachineWork
{
	/// <summary>一些异常消息实体类实现间隔性输出日志的接口,
	/// 主要用在异常消息输出时,给异常消息添加此接口可以有效控制输出
	/// </summary>
	public interface IExceptionLogMsgKey
	{
		/// <summary>消息判别时使用的Key
		/// </summary>
		string MsgKey { get; }

		/// <summary>限制时长，以毫秒为单位
		/// </summary>
		int DelayTime { get; }
	}
}
