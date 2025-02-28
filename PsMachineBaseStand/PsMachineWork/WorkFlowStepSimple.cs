using PsMachineWork.FlowV2;

namespace PsMachineWork
{
	/// <summary>简易的执行步骤封装，直接执行
	/// </summary>
	public abstract class WorkFlowStepSimple<TStation, TFlow, TStep, TStarCondition> : WorkFlowStep<TStation, TFlow, TStep, TStarCondition> where TStation : iWorkStation where TFlow : WorkFlowBase<TStation, TFlow, TStep, TStarCondition> where TStep : WorkFlowStep<TStation, TFlow, TStep, TStarCondition> where TStarCondition : WorkFlowStarCondition<TStation, TFlow, TStep, TStarCondition>
	{
		public WorkFlowStepSimple(TFlow myFlow, string myCode)
			: base(myFlow, myCode)
		{
		}

		public override void ValidateAllowRunStep()
		{
			if (!string.IsNullOrEmpty(base.InnerValidateCode))
			{
				mdsGotoFunction(base.InnerValidateCode);
			}
		}

		public override void RunStep()
		{
			if (!string.IsNullOrEmpty(base.InnerFuncCode))
			{
				mdsGotoFunction(base.InnerFuncCode);
			}
		}
	}
}
