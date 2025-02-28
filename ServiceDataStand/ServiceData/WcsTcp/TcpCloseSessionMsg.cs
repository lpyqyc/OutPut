namespace ServiceData.WcsTcp
{
	public class TcpCloseSessionMsg : JsonObject
	{
		public string Msg { get; set; }

		public TcpCloseSessionMsg()
		{
		}

		public TcpCloseSessionMsg(string msg)
		{
			Msg = msg;
		}
	}
}
