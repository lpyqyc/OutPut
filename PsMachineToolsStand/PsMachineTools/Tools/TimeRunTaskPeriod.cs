using System;

namespace PsMachineTools.Tools
{
	/// <summary>定时不重复执行某些任务
	/// </summary>
	public class TimeRunTaskPeriod
	{
		/// <summary>间隔周期，以毫秒为单位
		/// </summary>
		public int PeriodLen { get; private set; }

		/// <summary>不执行时间段的起点
		/// </summary>
		public DateTime? PeriodStar { get; protected set; }

		/// <summary>不执行时间段的终点
		/// </summary>
		public DateTime? PeriodStop { get; protected set; }

		/// <summary>初始化一个对象
		/// </summary>
		/// <param name="periodLen">时间间隔，以毫秒为单位，小于1时自动转换为1</param>
		public TimeRunTaskPeriod(int periodLen)
		{
			if (periodLen < 1)
			{
				periodLen = 1;
			}
			PeriodLen = periodLen;
		}

		/// <summary>检查当前是否可运行，只有在周期外才可运行
		/// </summary>
		/// <returns></returns>
		public bool IsRunCheck()
		{
			DateTime myNow = DateTime.Now;
			if (!PeriodStar.HasValue || !PeriodStop.HasValue)
			{
				return true;
			}
			DateTime value = myNow;
			DateTime? periodStop = PeriodStop;
			if (!(value >= periodStop))
			{
				value = myNow;
				periodStop = PeriodStar;
				return value < periodStop;
			}
			return true;
		}

		/// <summary>根据预定周期设置下一个运行周期
		/// </summary>
		public void SetNextPeriod()
		{
			DateTime myNow = DateTime.Now;
			PeriodStar = myNow;
			PeriodStop = myNow.AddMilliseconds(PeriodLen);
		}

		/// <summary>根据预定周期设置下一个运行周期
		/// </summary>
		public void SetNextPeriod(DateTime myNow)
		{
			PeriodStar = myNow;
			PeriodStop = myNow.AddMilliseconds(PeriodLen);
		}

		/// <summary>根据预定周期设置下一个运行周期
		/// </summary>
		public void SetNextPeriod(int myPeriodTimeLen)
		{
			DateTime myNow = DateTime.Now;
			if (myPeriodTimeLen < 1)
			{
				myPeriodTimeLen = 1;
			}
			PeriodStar = myNow;
			PeriodStop = myNow.AddMilliseconds(myPeriodTimeLen);
		}

		/// <summary>根据预定周期设置下一个运行周期
		/// </summary>
		public void SetNextPeriod(DateTime myNow, int myPeriodTimeLen)
		{
			if (myPeriodTimeLen < 1)
			{
				myPeriodTimeLen = 1;
			}
			PeriodStar = myNow;
			PeriodStop = myNow.AddMilliseconds(myPeriodTimeLen);
		}
	}
}
