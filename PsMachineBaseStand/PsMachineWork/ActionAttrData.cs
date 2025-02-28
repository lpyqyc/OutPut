using System;
using System.Reflection;

namespace PsMachineWork
{
	public class ActionAttrData
	{
		public object DataSource { get; internal set; }

		public string ID { get; internal set; }

		public StationActionAttribute Attribue { get; internal set; }

		public MethodInfo MethodInfo { get; internal set; }

		public bool GetVisible()
		{
			if (DataSource == null)
			{
				return false;
			}
			if (MethodInfo.IsStatic)
			{
				return true;
			}
			if (string.IsNullOrEmpty(Attribue.VisibleProperty))
			{
				return true;
			}
			PropertyInfo myProperty = DataSource.GetType().GetProperty(Attribue.VisibleProperty);
			if (myProperty == null)
			{
				throw new Exception("VisibleProperty 定义属性 [" + Attribue.VisibleProperty + "] 不存在");
			}
			if (myProperty.PropertyType != typeof(bool))
			{
				throw new Exception("VisibleProperty 定义属性 [" + Attribue.VisibleProperty + "] 返回值类型不正确");
			}
			return (bool)myProperty.GetValue(DataSource, null);
		}

		public bool GetEnabled()
		{
			if (DataSource == null)
			{
				return false;
			}
			if (MethodInfo.IsStatic)
			{
				return true;
			}
			if (string.IsNullOrEmpty(Attribue.CriteriaProperty))
			{
				return true;
			}
			PropertyInfo myProperty = DataSource.GetType().GetProperty(Attribue.CriteriaProperty);
			if (myProperty == null)
			{
				throw new Exception("CriteriaProperty 定义属性 [" + Attribue.CriteriaProperty + "] 不存在");
			}
			if (myProperty.PropertyType != typeof(bool))
			{
				throw new Exception("CriteriaProperty 定义属性 [" + Attribue.CriteriaProperty + "] 返回值类型不正确");
			}
			return (bool)myProperty.GetValue(DataSource, null);
		}

		public string GetCaption()
		{
			if (string.IsNullOrEmpty(Attribue.Caption))
			{
				return MethodInfo.Name;
			}
			return Attribue.Caption;
		}
	}
}
