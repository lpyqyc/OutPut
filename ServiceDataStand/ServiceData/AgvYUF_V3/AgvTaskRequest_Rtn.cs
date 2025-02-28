namespace ServiceData.AgvYUF_V3
{
	public class AgvTaskRequest_Rtn : JsonObject
	{
		public string id { get; set; }

		public string agvCode { get; set; }

		public string missionId { get; set; }

		public string schedulePlanId { get; set; }

		public string callbackUrl { get; set; }

		public string name { get; set; }

		public int sequence { get; set; }

		public string description { get; set; }

		public string status { get; set; }

		public string message { get; set; }

		public string runtimeParam { get; set; }

		public string startTime { get; set; }

		public string endTime { get; set; }

		public string agvGroupId { get; set; }

		public string agvType { get; set; }

		public string createTime { get; set; }

		public string updateTime { get; set; }

		public string currentActionSequence { get; set; }

		public string errorCode { get; set; }

		public string agvName { get; set; }

		public string missionGroupId { get; set; }

		public string missionCallId { get; set; }
	}
}
