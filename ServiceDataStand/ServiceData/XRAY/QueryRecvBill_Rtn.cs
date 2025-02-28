using System.Collections.Generic;

namespace ServiceData.XRAY
{
	public class QueryRecvBill_Rtn : JsonObject
	{
		public string Code { get; set; }

		public string Msg { get; set; }

		public List<QueryRecvBill_RtnData> Data { get; set; } = new List<QueryRecvBill_RtnData>();

	}
}
