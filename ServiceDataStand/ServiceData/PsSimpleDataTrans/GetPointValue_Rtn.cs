using Newtonsoft.Json;

namespace ServiceData.PsSimpleDataTrans
{
	public class GetPointValue_Rtn : JsonObject
	{
		public string Addr { get; set; }

		public string Value { get; set; }

		public string RtnCode { get; set; }

		public string RtnMsg { get; set; }

		[JsonIgnore]
		public bool IsOK => RtnCode == "0";
	}
}
