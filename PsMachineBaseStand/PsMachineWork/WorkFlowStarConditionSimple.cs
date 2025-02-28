using PsMachineWork.FlowV2;

namespace PsMachineWork
{
	/// <summary>流程启动条件,简易封装
	/// </summary>
	public abstract class WorkFlowStarConditionSimple<TStation, TFlow, TStep, TStarCondition> : WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition> where TStation : iWorkStation where TFlow : WorkFlowBase<TStation, TFlow, TStep, TStarCondition> where TStep : WorkFlowStep<TStation, TFlow, TStep, TStarCondition> where TStarCondition : WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition>
	{
		public WorkFlowStarConditionSimple(TFlow myFlow, string myCode)
			: base(myFlow, myCode)
		{
		}

		public override void RunValidate()
		{
			mdsGotoFunction(base.InnerFuncCode);
		}
	}
}
