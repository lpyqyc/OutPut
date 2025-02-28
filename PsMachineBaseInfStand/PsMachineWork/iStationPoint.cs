using System;
using System.ComponentModel;

namespace PsMachineWork
{
	/// <summary>工作站的点位
	/// </summary>
	public interface iStationPoint : INotifyPropertyChanged
	{
		/// <summary>所在工作站
		/// </summary>
		iWorkStation ParentStation { get; }

		/// <summary>点位编码，英文大小写加数字加下划线，不要用其它符合，否则可能会引起其它解析异常
		/// </summary>
		string Code { get; }

		/// <summary>点位描述
		/// </summary>
		string Desc { get; }

		/// <summary>值描述，一般界面显示时，应该绑定此属性
		/// </summary>
		string ValueDesc { get; }

		/// <summary>点位值的Object类型,在具体的派生类中，应该封装自己的实际值类型 Value 属性
		/// </summary>
		object ValueObj { get; }

		/// <summary>最后读取设备更新时间
		/// </summary>
		DateTime? LastReadTime { get; }

		/// <summary>最后写入设备时间
		/// </summary>
		DateTime? LastWriteTime { get; }

		/// <summary>当前是否有异常
		/// </summary>
		bool IsException { get; }

		/// <summary>运行状态描述，如果有异常在此写入
		/// </summary>
		string RunDesc { get; }

		/// <summary>是否支持从设备端定时读取刷新数据
		/// </summary>
		bool IsAutoRefresh { get; }

		/// <summary>是否本地数据存储点位, 并非PLC或者控制器点位等异地缓存点位
		/// </summary>
		bool IsLocalDataPoint { get; }

		/// <summary>点位的可读写类型,
		/// 只读,只写,可读写,或者都不行
		/// </summary>
		emPointReadWriteType ReadWriteType { get; }

		/// <summary>读取点位值
		/// </summary>
		/// <returns></returns>
		object ReadValueObject();
	}
}
