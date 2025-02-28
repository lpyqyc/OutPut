using System.Collections.Generic;

namespace ServiceData.WcsTcp
{
	public class TcpPostMulData_Rtn : TcpReplyBase
	{
		public string Station { get; set; }

		public List<TcpPostMulData_RtnData> Data { get; set; } = new List<TcpPostMulData_RtnData>();

	}
}
