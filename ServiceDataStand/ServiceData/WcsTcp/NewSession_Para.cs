namespace ServiceData.WcsTcp
{
	public class NewSession_Para : JsonObject
	{
		public string LocalStation { get; set; }

		public bool IsProvideService { get; set; }
	}
}
