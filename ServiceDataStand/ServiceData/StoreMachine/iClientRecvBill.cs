namespace ServiceData.StoreMachine
{
	public interface iClientRecvBill
	{
		string Oid { get; }

		string Code { get; }

		string StoreGroup { get; }

		string Factory { get; }
	}
}
