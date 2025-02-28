namespace ServiceData.StoreMachine
{
	public interface iClientStorePoint
	{
		string Oid { get; }

		string Code { get; }

		int ShelfIndex { get; }

		string ShelfCode { get; }

		int Column { get; }

		int Row { get; }

		int Z { get; }

		double PointSize { get; }

		double MaterialSize { get; }

		bool? IsException { get; }

		bool Voided { get; }

		bool IsSpace { get; }

		bool IsUseGoods { get; }

		bool IsUseSpace { get; }
	}
}
