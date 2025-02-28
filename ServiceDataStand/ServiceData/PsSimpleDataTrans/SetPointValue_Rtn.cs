using Newtonsoft.Json;

namespace ServiceData.PsSimpleDataTrans
{
	public class SetPointValue_Rtn : JsonObject
	{
		public string RtnCode { get; set; }

		public string RtnMsg { get; set; }

		[JsonIgnore]
		public bool IsOK => RtnCode == "0";
	}
}
