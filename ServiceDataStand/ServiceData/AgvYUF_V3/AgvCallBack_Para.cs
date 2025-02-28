using System;
using Newtonsoft.Json;

namespace ServiceData.AgvYUF_V3
{
	public class AgvCallBack_Para : JsonObject, iAgvCallBack
	{
		public string missionWorkId { get; set; }

		public string markCode { get; set; }

		public string agvCode { get; set; }

		public string status { get; set; }

		public string errorCode { get; set; }

		public string errorMessage { get; set; }

		[JsonIgnore]
		public DateTime CreateDate { get; set; } = DateTime.Now;

	}
}
