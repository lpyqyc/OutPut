using System;
using System.Collections.Generic;

namespace ServiceData.StoreMachine
{
	public interface iClientPickDownBill
	{
		string Oid { get; }

		string StoreGroup { get; }

		string Code { get; }

		string WO { get; }

		DateTime? CreateDate { get; }

		IEnumerable<iClientPickDownItem> PickDownReelList { get; }
	}
}
