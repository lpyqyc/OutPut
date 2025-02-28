namespace ServiceData.XRAY
{
	public class UpLoadReelData_Para : JsonObject
	{
		public string RecvCode { get; set; }

		public string OrgReelID { get; set; }

		public string NewReelID { get; set; }

		public string PartCode { get; set; }

		public string LotNO { get; set; }

		public string DateCode { get; set; }

		public double Qty { get; set; }
	}
}
