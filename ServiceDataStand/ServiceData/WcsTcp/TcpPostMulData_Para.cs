using System.Collections.Generic;

namespace ServiceData.WcsTcp
{
	public class TcpPostMulData_Para : JsonObject
	{
		public string Station { get; set; }

		public List<TcpPostMulData_ParaData> Data { get; set; } = new List<TcpPostMulData_ParaData>();

	}
}
