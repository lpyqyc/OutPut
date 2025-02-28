using System;
using System.IO;
using System.Text;

namespace PsMachineTools.Tools
{
	/// <summary>静态类，提供一些扩展功能函数
	/// </summary>
	public static class HyExpFunction
	{
		public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;

		/// <summary>移除文件夹下面所有内容,包括子文件夹和文件
		/// </summary>
		/// <param name="myDirectoryInfo"></param>
		public static void ClearFolderChildren(this DirectoryInfo myDirectoryInfo)
		{
			DirectoryInfo[] directories = myDirectoryInfo.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(recursive: true);
			}
			FileInfo[] files = myDirectoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
		}

		/// <summary>复制文件夹及文件
		/// </summary>
		/// <param name="sourceFolder">原文件路径</param>
		/// <param name="destFolder">目标文件路径</param>
		/// <returns></returns>
		public static void CopyFolder(string sourceFolder, string destFolder, bool myFileOverride)
		{
			try
			{
				if (!Directory.Exists(destFolder))
				{
					Directory.CreateDirectory(destFolder);
				}
				string[] files = Directory.GetFiles(sourceFolder);
				foreach (string obj in files)
				{
					string name = Path.GetFileName(obj);
					string dest = Path.Combine(destFolder, name);
					File.Copy(obj, dest, myFileOverride);
				}
				files = Directory.GetDirectories(sourceFolder);
				foreach (string obj2 in files)
				{
					string name2 = Path.GetFileName(obj2);
					string dest2 = Path.Combine(destFolder, name2);
					CopyFolder(obj2, dest2, myFileOverride);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("复制文件夹 " + sourceFolder + " 内容出错=\r\n" + ex.Message);
			}
		}

		/// <summary>合并HTTP文件夹地址和文件名称
		/// </summary>
		/// <param name="myHttpServer"></param>
		/// <param name="myHttpFile"></param>
		/// <returns></returns>
		public static string CombinHttpUrl(string myHttpServer, string myHttpFile)
		{
			if (string.IsNullOrWhiteSpace(myHttpFile))
			{
				return myHttpServer;
			}
			if (string.IsNullOrEmpty(myHttpServer))
			{
				return myHttpFile;
			}
			myHttpServer = myHttpServer.Trim();
			char num = myHttpServer[myHttpServer.Length - 1];
			string myValue = null;
			if (num != '/')
			{
				return myHttpServer + "/" + myHttpFile;
			}
			return myHttpServer + myHttpFile;
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T CreateInstance<T>()
		{
			return Activator.CreateInstance<T>();
		}

		public static object CreateInstance(this Type type)
		{
			return Activator.CreateInstance(type);
		}

		/// <summary>验证字段值不能为空
		/// </summary>
		public static void CheckFieldValudNotNull(string myFieldValue, string myFieldName)
		{
			if (string.IsNullOrEmpty(myFieldValue))
			{
				throw new Exception(myFieldName + " 值不能为空");
			}
		}

		/// <summary>验证字段值不能为空
		/// </summary>
		public static void CheckFieldValudNotNull(object myFieldValue, string myFieldName)
		{
			if (myFieldValue == null)
			{
				throw new Exception(myFieldName + " 值不能为空");
			}
		}

		/// <summary>验证字段值不能为空
		/// </summary>
		public static void CheckFieldValudNotNull2(string myFieldValue, string myExceptionMsg)
		{
			if (string.IsNullOrEmpty(myFieldValue))
			{
				throw new Exception(myExceptionMsg);
			}
		}

		/// <summary>验证字段值不能为空
		/// </summary>
		public static void CheckFieldValudNotNull2(object myFieldValue, string myExceptionMsg)
		{
			if (myFieldValue == null)
			{
				throw new Exception(myExceptionMsg);
			}
		}

		/// <summary>文件夹中清除超过天数的文件夹
		/// </summary>
		public static void DirectoryClearOldFolder(string myDirectory, int mySaveDays = 200)
		{
			DateTime my检查日期 = DateTime.Now.Date.AddDays(-mySaveDays);
			Directory.CreateDirectory(myDirectory);
			DirectoryInfo[] my子文件夹s = new DirectoryInfo(myDirectory).GetDirectories();
			for (int myIndex = my子文件夹s.Length - 1; myIndex >= 0; myIndex--)
			{
				DirectoryInfo my子文件夹Info = my子文件夹s[myIndex];
				if (my子文件夹Info.LastWriteTime < my检查日期)
				{
					my子文件夹Info.Delete(recursive: true);
				}
			}
		}

		/// <summary>文件夹中清除超过天数的文件, 不包文件夹
		/// </summary>
		/// <param name="myDirectory"></param>
		/// <param name="mySaveDays"></param>
		public static void DirectoryClearOldFile(string myDirectory, int mySaveDays = 200)
		{
			DateTime my检查日期 = DateTime.Now.Date.AddDays(-mySaveDays);
			Directory.CreateDirectory(myDirectory);
			FileInfo[] my子文件List = new DirectoryInfo(myDirectory).GetFiles();
			for (int myIndex = my子文件List.Length - 1; myIndex >= 0; myIndex--)
			{
				FileInfo my子文件夹Info = my子文件List[myIndex];
				if (my子文件夹Info.LastWriteTime < my检查日期)
				{
					my子文件夹Info.Delete();
				}
			}
		}

		/// <summary>获取错误的全部消息描述
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string GetErrMsg(Exception ex)
		{
			if (ex == null)
			{
				return null;
			}
			StringBuilder myStb = new StringBuilder();
			GetErrMsg(ref myStb, ex);
			return myStb.ToString();
		}

		private static void GetErrMsg(ref StringBuilder myStb, Exception ex)
		{
			myStb.AppendLine(ex.Message);
			if (ex.InnerException != null)
			{
				GetErrMsg(ref myStb, ex.InnerException);
			}
		}

		public static bool IsNullableType(Type theType)
		{
			if (theType.IsGenericType)
			{
				return theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
			}
			return false;
		}
	}
}
