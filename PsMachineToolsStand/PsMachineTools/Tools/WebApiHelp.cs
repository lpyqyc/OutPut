using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PsMachineTools.Tools
{
	public static class WebApiHelp
	{
		public static string HttpApi(string Url, string myFunctionName, string myParaNameAndValue, string myDataText, string myGetPostType, string myContentType, Dictionary<string, string> myHeaders, int myTimeLen = 180000)
		{
			if (string.Equals(myGetPostType, "Post", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(myDataText))
			{
				myDataText = " ";
			}
			if (string.Equals(myGetPostType, "Get", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(myDataText))
			{
				throw new Exception("Http Get方式下, 不能有Post内容");
			}
			if (string.IsNullOrEmpty(myContentType))
			{
				myContentType = "application/json";
			}
			string myUserUrl = Url;
			if (!string.IsNullOrEmpty(myFunctionName))
			{
				myUserUrl = HyExpFunction.CombinHttpUrl(myUserUrl, myFunctionName);
			}
			if (!string.IsNullOrEmpty(myParaNameAndValue))
			{
				myUserUrl = myUserUrl + "?" + myParaNameAndValue;
			}
			Encoding encoding = Encoding.UTF8;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUserUrl);
			request.Accept = "text/html,application/xhtml+xml,*/*";
			request.ContentType = myContentType;
			if (myHeaders != null && myHeaders.Count > 0)
			{
				foreach (KeyValuePair<string, string> myItem in myHeaders)
				{
					request.Headers.Add(myItem.Key, myItem.Value);
				}
			}
			request.Method = myGetPostType.ToUpper();
			request.Timeout = myTimeLen;
			if (!string.IsNullOrEmpty(myDataText))
			{
				byte[] buffer = encoding.GetBytes(myDataText);
				request.ContentLength = buffer.Length;
				request.GetRequestStream().Write(buffer, 0, buffer.Length);
			}
			_ = "WEB服务调用 " + myUserUrl + "\r\n" + myDataText;
			HttpWebResponse obj = (HttpWebResponse)request.GetResponse();
			string myReturnText = null;
			using (StreamReader reader = new StreamReader(obj.GetResponseStream(), Encoding.UTF8))
			{
				myReturnText = reader.ReadToEnd();
			}
			_ = "WEB服务回传 " + myUserUrl + " 回传=\r\n" + myReturnText;
			return myReturnText;
		}

		public static void HttpApiVoid(string Url, string myFunctionName, string myParaNameAndValue, string myDataText, string myGetPostType, string myContentType, Dictionary<string, string> myHeaders, int myTimeLen = 180000)
		{
			if (string.Equals(myGetPostType, "Post", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(myDataText))
			{
				myDataText = " ";
			}
			if (string.Equals(myGetPostType, "Get", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(myDataText))
			{
				throw new Exception("Http Get方式下, 不能有Post内容");
			}
			if (string.IsNullOrEmpty(myContentType))
			{
				myContentType = "application/json";
			}
			string myUserUrl = Url;
			if (!string.IsNullOrEmpty(myFunctionName))
			{
				myUserUrl = HyExpFunction.CombinHttpUrl(myUserUrl, myFunctionName);
			}
			if (!string.IsNullOrEmpty(myParaNameAndValue))
			{
				myUserUrl = myUserUrl + "?" + myParaNameAndValue;
			}
			Encoding encoding = Encoding.UTF8;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUserUrl);
			request.Accept = "text/html,application/xhtml+xml,*/*";
			request.ContentType = myContentType;
			if (myHeaders != null && myHeaders.Count > 0)
			{
				foreach (KeyValuePair<string, string> myItem in myHeaders)
				{
					request.Headers.Add(myItem.Key, myItem.Value);
				}
			}
			request.Method = myGetPostType.ToUpper();
			request.Timeout = myTimeLen;
			if (!string.IsNullOrEmpty(myDataText))
			{
				byte[] buffer = encoding.GetBytes(myDataText);
				request.ContentLength = buffer.Length;
				request.GetRequestStream().Write(buffer, 0, buffer.Length);
			}
		}
	}
}
