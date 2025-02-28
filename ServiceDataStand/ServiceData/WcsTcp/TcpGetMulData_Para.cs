namespace ServiceData.WcsTcp
{
	public class TcpGetMulData_Para : JsonObject
	{
		public string Station { get; set; }

		public string[] Points { get; set; }
	}
}
