using System.Collections.Generic;

namespace ServiceData.StoreMachine
{
	public class GoodsBarcodeData
	{
		public List<string> Barcodes;

		public string OrgBarcode { get; set; }

		public string Rid { get; set; }

		public string PartNO { get; set; }

		public string DataCode { get; set; }

		public double Qty { get; set; }

		public string SuppCode { get; set; }

		public string Verdor { get; set; }

		public string OrgPartNO { get; set; }

		public string OrgDateCode { get; set; }

		public string Factory { get; set; }

		public string LotNO { get; set; }

		public string InnerSeqNO { get; set; }

		public string PO { get; set; }

		public string SpOrder { get; set; }

		public string ErrorMsg { get; set; }
	}
}
