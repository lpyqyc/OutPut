using System;
using Newtonsoft.Json;

namespace ServiceData.AgvYUF_V3
{
	public class AgvArriveEndCallBack_Para : JsonObject, iAgvCallBack
	{
		public string missionWorkId { get; set; }

		public string markCode { get; set; }

		public string agvCode { get; set; }

		public string Status { get; set; }

		public string ErrorCode { get; set; }

		public string ErrorMessage { get; set; }

		[JsonIgnore]
		public DateTime CreateDate { get; set; } = DateTime.Now;


		[JsonIgnore]
		string iAgvCallBack.status => Status;

		[JsonIgnore]
		string iAgvCallBack.errorCode => ErrorCode;

		[JsonIgnore]
		public string errorMessage => ErrorCode;
	}
}
