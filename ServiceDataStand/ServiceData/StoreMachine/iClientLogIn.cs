using System;

namespace ServiceData.StoreMachine
{
	public interface iClientLogIn
	{
		string StoreGroup { get; }

		DateTime? IssueDate { get; }

		string MachineCode { get; }

		string ReelID { get; }

		double AvaQty { get; }

		string RecvCode { get; }

		iClientStorePoint StorePoint { get; }

		iClientStorePanel StorePanel { get; }
	}
}
