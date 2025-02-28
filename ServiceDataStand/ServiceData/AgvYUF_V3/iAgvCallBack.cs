using System;

namespace ServiceData.AgvYUF_V3
{
	public interface iAgvCallBack
	{
		string missionWorkId { get; }

		string markCode { get; }

		string agvCode { get; }

		string status { get; }

		string errorCode { get; }

		string errorMessage { get; }

		DateTime CreateDate { get; }
	}
}
