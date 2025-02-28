namespace ServiceData.StoreMachine
{
	public interface iClientBarcodeParseRule2D
	{
		string Code { get; }

		string Desc { get; }

		string SplitText { get; }

		GoodsBarcodeData ParseBarcodeText(string myBarcodeText);
	}
}
