namespace ServiceData.WcsTcp
{
	public class TestData_Para : JsonObject
	{
		public string Data { get; set; }

		public TestData_Para()
		{
		}

		public TestData_Para(string data)
		{
			Data = data;
		}
	}
}
