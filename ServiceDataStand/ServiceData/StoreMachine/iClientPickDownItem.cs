namespace ServiceData.StoreMachine
{
	public interface iClientPickDownItem
	{
		string Oid { get; }

		iClientStorePoint StorePoint { get; }

		iClientStorePanel StorePanel { get; }

		string ReelID { get; }

		bool IsWaitPick { get; }

		bool IsFullLoss { get; }

		string PanelInnerCode { get; }
	}
}
