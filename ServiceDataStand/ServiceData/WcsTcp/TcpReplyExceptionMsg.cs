namespace ServiceData.WcsTcp
{
	public class TcpReplyExceptionMsg : TcpReplyBase
	{
		public TcpReplyExceptionMsg()
		{
			base.Code = "100";
			base.RtnMsg = null;
		}

		public TcpReplyExceptionMsg(string msg)
		{
			base.Code = "100";
			base.RtnMsg = msg;
		}
	}
}
