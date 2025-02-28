using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PsMachineWork
{
	/// <summary>
	/// </summary>
	public class StationHelp
	{
		/// <summary>从一个类中反射 StaionActionAttribute 声明的所有方法和执行体
		/// </summary>
		/// <param name="mySource"></param>
		/// <returns></returns>
		public static List<ActionAttrData> GetActionList(object mySource)
		{
			List<ActionAttrData> myValue = new List<ActionAttrData>();
			if (mySource == null)
			{
				return myValue;
			}
			Type myType = mySource.GetType();
			mdsGetActionList(mySource, myType, myValue);
			return myValue;
		}

		private static void mdsGetActionList(object mySource, Type myRefType, List<ActionAttrData> myValue)
		{
			MethodInfo[] methods = myRefType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
			foreach (MethodInfo myMethod in methods)
			{
				StationActionAttribute myAttr = myMethod.GetCustomAttribute<StationActionAttribute>(inherit: true);
				if (myAttr != null)
				{
					ActionAttrData myNew2 = new ActionAttrData
					{
						DataSource = mySource,
						ID = myMethod.Name,
						Attribue = myAttr,
						MethodInfo = myMethod
					};
					if (!myValue.Any((ActionAttrData p) => p.ID == myNew2.ID))
					{
						myValue.Add(myNew2);
					}
				}
			}
			methods = myRefType.GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo myMethod2 in methods)
			{
				StationActionAttribute myAttr2 = myMethod2.GetCustomAttribute<StationActionAttribute>(inherit: true);
				if (myAttr2 != null)
				{
					ActionAttrData myNew = new ActionAttrData
					{
						DataSource = mySource,
						ID = myMethod2.Name,
						Attribue = myAttr2,
						MethodInfo = myMethod2
					};
					if (!myValue.Any((ActionAttrData p) => p.ID == myNew.ID))
					{
						myValue.Add(myNew);
					}
				}
			}
		}

		/// <summary>获取用特性声明的点位
		/// </summary>
		/// <returns></returns>
		public static List<StationPointAttrData> GetStationPointAttributeList(Type myRefType)
		{
			List<StationPointAttrData> myValue = new List<StationPointAttrData>();
			PropertyInfo[] properties = myRefType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (PropertyInfo myProperty in properties)
			{
				StationPointAttribute myAttr = myProperty.GetCustomAttribute<StationPointAttribute>(inherit: true);
				if (myAttr != null)
				{
					StationPointAttrData myItem = new StationPointAttrData
					{
						Attribute = myAttr,
						PropertyInfo = myProperty
					};
					myValue.Add(myItem);
				}
			}
			return myValue;
		}

		/// <summary>获取触发器中的条件定义函数
		/// </summary>
		/// <param name="myRefType"></param>
		/// <returns></returns>
		public static List<TriggerConditionAttrData> GetTriggerConditionAttributeList(Type myRefType)
		{
			List<TriggerConditionAttrData> myValue = new List<TriggerConditionAttrData>();
			MethodInfo[] methods = myRefType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo myMethod in methods)
			{
				ParameterInfo[] myParaList = myMethod.GetParameters();
				if (myParaList == null || myParaList.Length == 0)
				{
					TriggerConditionAttribute myAttr = myMethod.GetCustomAttribute<TriggerConditionAttribute>(inherit: true);
					if (myAttr != null)
					{
						TriggerConditionAttrData myItem = new TriggerConditionAttrData
						{
							Attribute = myAttr,
							MethodInfo = myMethod
						};
						myValue.Add(myItem);
					}
				}
			}
			return myValue;
		}

		/// <summary>查找标记了 FlowStepAttribute 的函数
		/// </summary>
		/// <param name="myRefType"></param>
		/// <returns></returns>
		public static List<FlowStepAttrData> GetFlowStepAttributeList(Type myRefType)
		{
			List<FlowStepAttrData> myValue = new List<FlowStepAttrData>();
			MethodInfo[] methods = myRefType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo myMethod in methods)
			{
				FlowStepAttribute myAttr = myMethod.GetCustomAttribute<FlowStepAttribute>(inherit: true);
				if (myAttr != null)
				{
					FlowStepAttrData myItem = new FlowStepAttrData
					{
						Attribute = myAttr,
						MethodInfo = myMethod
					};
					myValue.Add(myItem);
				}
			}
			return myValue;
		}

		/// <summary>查找标记了 FlowStepAttribute 的函数
		/// </summary>
		/// <param name="myRefType"></param>
		/// <returns></returns>
		public static List<FlowStarConditionAttrData> GetFlowStarConditionAttributeList(Type myRefType)
		{
			List<FlowStarConditionAttrData> myValue = new List<FlowStarConditionAttrData>();
			MethodInfo[] methods = myRefType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo myMethod in methods)
			{
				ParameterInfo[] myParaList = myMethod.GetParameters();
				if (myParaList == null || myParaList.Length == 0)
				{
					FlowStarConditionAttribute myAttr = myMethod.GetCustomAttribute<FlowStarConditionAttribute>(inherit: true);
					if (myAttr != null)
					{
						FlowStarConditionAttrData myItem = new FlowStarConditionAttrData
						{
							Attribute = myAttr,
							MethodInfo = myMethod
						};
						myValue.Add(myItem);
					}
				}
			}
			return myValue;
		}
	}
}
