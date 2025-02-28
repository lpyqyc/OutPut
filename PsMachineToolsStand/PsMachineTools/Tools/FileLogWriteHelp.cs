using System;
using System.IO;
using System.Threading;

namespace PsMachineTools.Tools
{
	/// <summary>日志文件输出，辅助管理类
	/// 会自动在程序文件夹下，创建 Log 文件夹，然后按不同日期分文件夹，分别保存日志
	/// 对象会按不同小时，每小时的日志文件，输出日志时，可以通过 WriteLine 函数的参数指定输日志文件名称(不含日志路径)
	/// 为了兼容多线程情况下，输出日志文件时不影响原线程的运行速度，此对象使用后台异步线程方式
	/// 调用 WriteLine 函数输出日志时，只是写入对象集合中，然后立即返回，真正的写入文件的操作由后台线程完成
	/// </summary>
	public static class FileLogWriteHelp
	{
		/// <summary>
		/// </summary>
		private class clsWriteLog
		{
			public DateTime LogTime { get; }

			public string LogType { get; set; }

			public string Msg { get; set; }

			public clsWriteLog(string myLogType, string myMsg)
			{
				LogType = myLogType;
				Msg = myMsg;
				LogTime = DateTime.Now;
			}
		}

		private static Thread m_WorkThread;

		private static string _LogFoder;

		private static SynList<clsWriteLog> m_ReadTextList;

		/// <summary>前一次创建日志文件时间
		/// </summary>
		private static DateTime m_LastCreateFileTime;

		private static object _WriteLine_Lock;

		static FileLogWriteHelp()
		{
			_LogFoder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
			m_LastCreateFileTime = new DateTime(1900, 1, 1);
			_WriteLine_Lock = new object();
			m_ReadTextList = new SynList<clsWriteLog>();
		}

		private static void mdsWorkTask()
		{
			Directory.CreateDirectory(_LogFoder);
			int myWaitCount = 0;
			while (true)
			{
				try
				{
					if (m_ReadTextList.Count > 0)
					{
						myWaitCount = 0;
						clsWriteLog myWriteLog = m_ReadTextList[0];
						m_ReadTextList.RemoveAt(0);
						DateTime myNow = DateTime.Now;
						string my当前日志文件夹 = "";
						string my子文件夹名称 = myNow.ToString("yyyy-MM-dd");
						my当前日志文件夹 = Path.Combine(_LogFoder, my子文件夹名称);
						if (m_LastCreateFileTime.Date != myNow.Date)
						{
							Directory.CreateDirectory(my当前日志文件夹);
							m_LastCreateFileTime = myNow;
						}
						string filename = ((myWriteLog.LogType != null) ? (myWriteLog.LogType + " ") : null) + myNow.ToString("MM-dd HH") + ".txt";
						using StreamWriter sw = File.AppendText(Path.Combine(my当前日志文件夹, filename));
						sw.Write(myWriteLog.LogTime.ToString("HH:mm:ss "));
						sw.WriteLine(myWriteLog.Msg);
					}
					else
					{
						myWaitCount++;
						if (myWaitCount > 50)
						{
							break;
						}
					}
				}
				catch (ThreadAbortException)
				{
					break;
				}
				catch (Exception)
				{
				}
				Thread.Sleep(15);
			}
			m_WorkThread = null;
		}

		/// <summary>清除超过指定天数的文件夹,默认值200天
		/// </summary>
		public static void ClearOldFolder(int mySaveDays = 200)
		{
			DateTime my检查日期 = DateTime.Now.Date.AddDays(-mySaveDays);
			string logFoder = _LogFoder;
			Directory.CreateDirectory(logFoder);
			DirectoryInfo[] my子文件夹s = new DirectoryInfo(logFoder).GetDirectories();
			for (int myIndex = my子文件夹s.Length - 1; myIndex >= 0; myIndex--)
			{
				DirectoryInfo my子文件夹Info = my子文件夹s[myIndex];
				if (my子文件夹Info.LastWriteTime < my检查日期)
				{
					my子文件夹Info.Delete(recursive: true);
				}
			}
		}

		/// <summary>输出日志行
		/// <paramref name="myLogType">日志文件名</paramref>
		/// <paramref name="text">日志内容</paramref>
		/// </summary>
		public static void WriteLine(string myLogType, string text)
		{
			lock (_WriteLine_Lock)
			{
				if (m_WorkThread == null)
				{
					mdsCreateThread();
				}
			}
			m_ReadTextList.Add(new clsWriteLog(myLogType, text));
		}

		private static void mdsCreateThread()
		{
			m_WorkThread = new Thread(mdsWorkTask);
			m_WorkThread.Start();
		}

		/// <summary>关闭值守线程
		/// </summary>
		public static void Close()
		{
			if (m_WorkThread != null)
			{
				try
				{
					m_WorkThread.Abort();
					m_WorkThread = null;
				}
				catch
				{
				}
			}
		}
	}
}
