namespace ServiceData.AgvYUF_V3
{
	public class AgvTaskRequest_Para : JsonObject
	{
		public string missionId { get; set; }

		public string missionCode { get; set; }

		public string agvCode { get; set; }

		public string callbackUrl { get; set; }

		public string runtimeParam { get; set; }

		public string missionCallId { get; set; }

		public int sequence { get; set; }
	}
}
