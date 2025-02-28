namespace ServiceData.XRAY
{
	public class CheckAndCreateReelID_Rtn : JsonObject
	{
		public string Code { get; set; }

		public string Msg { get; set; }

		public CheckAndCreateReelID_RtnDtl Data { get; set; }
	}
}
