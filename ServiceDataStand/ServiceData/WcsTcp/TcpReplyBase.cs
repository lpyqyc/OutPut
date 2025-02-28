namespace ServiceData.WcsTcp
{
	public class TcpReplyBase : JsonObject
	{
		public string Code { get; set; } = "0";


		public string RtnMsg { get; set; } = "Success";


		public virtual bool IsOK()
		{
			return Code == "0";
		}

		public virtual bool IsError()
		{
			if (Code != "0")
			{
				return Code != "01";
			}
			return false;
		}
	}
}
