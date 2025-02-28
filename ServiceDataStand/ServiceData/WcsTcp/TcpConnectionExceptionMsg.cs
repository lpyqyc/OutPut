namespace ServiceData.WcsTcp
{
	public class TcpConnectionExceptionMsg : TcpReplyBase
	{
		public TcpConnectionExceptionMsg()
		{
		}

		public TcpConnectionExceptionMsg(string msg)
		{
			base.RtnMsg = msg;
		}
	}
}
