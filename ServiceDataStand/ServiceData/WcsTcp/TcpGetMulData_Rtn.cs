using System.Collections.Generic;

namespace ServiceData.WcsTcp
{
	public class TcpGetMulData_Rtn : TcpReplyBase
	{
		public string Station { get; set; }

		public List<TcpGetMulData_RtnData> Data { get; set; } = new List<TcpGetMulData_RtnData>();

	}
}
