namespace ServiceData.WcsTcp
{
	public class TestData_Rtn : TcpReplyBase
	{
		public TestData_Rtn()
		{
		}

		public TestData_Rtn(string msg)
		{
			base.RtnMsg = msg;
		}
	}
}
