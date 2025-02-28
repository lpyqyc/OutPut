using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PsMachineTools.Tools
{
	/// <summary>HTTP WEB API帮助类
	/// 简易封装了几个 GetAsync,PostAsync,PutAsync,DeleteAsync 等操作，用于支持简单的 http web api 操作
	/// </summary>
	public class HttpClientHelper
	{
		/// <summary>初始化一个HTTP WEB API帮助类
		/// </summary>
		public HttpClientHelper()
		{
		}

		/// <summary>
		/// 发起GET异步请求
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回string</returns>
		public async Task<string> GetAsync(string url, Dictionary<string, string> headers = null, int timeOut = 30)
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(timeOut);
			if (headers != null && headers.Count > 0)
			{
				foreach (string key in headers.Keys)
				{
					client.DefaultRequestHeaders.Add(key, headers[key]);
				}
			}
			using HttpResponseMessage response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStringAsync();
			}
			return string.Empty;
		}

		/// <summary>
		/// 发起POST异步请求
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="body">POST提交的内容</param>
		/// <param name="bodyMediaType">POST内容的媒体类型，如：application/xml、application/json</param>
		/// <param name="responseContentType">HTTP响应上的content-type内容头的值,如：application/xml、application/json、application/text、application/x-www-form-urlencoded等</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回string</returns>
		public async Task<string> PostAsync(string url, string body, string bodyMediaType = null, string responseContentType = null, Dictionary<string, string> headers = null, int timeOut = 30)
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(timeOut);
			if (headers != null && headers.Count > 0)
			{
				foreach (string key in headers.Keys)
				{
					client.DefaultRequestHeaders.Add(key, headers[key]);
				}
			}
			StringContent content = new StringContent(body, Encoding.UTF8, bodyMediaType);
			if (!string.IsNullOrWhiteSpace(responseContentType))
			{
				content.Headers.ContentType = MediaTypeHeaderValue.Parse(responseContentType);
			}
			else
			{
				content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
			}
			using HttpResponseMessage response = await client.PostAsync(url, content);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStringAsync();
			}
			return string.Empty;
		}

		/// <summary>
		/// 发起POST异步请求
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="body">POST提交的内容</param>
		/// <param name="bodyMediaType">POST内容的媒体类型，如：application/xml、application/json</param>
		/// <param name="responseContentType">HTTP响应上的content-type内容头的值,如：application/xml、application/json、application/text、application/x-www-form-urlencoded等</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回string</returns>
		public async Task<string> PutAsync(string url, string body, string bodyMediaType = null, string responseContentType = null, Dictionary<string, string> headers = null, int timeOut = 30)
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(timeOut);
			if (headers != null && headers.Count > 0)
			{
				foreach (string key in headers.Keys)
				{
					client.DefaultRequestHeaders.Add(key, headers[key]);
				}
			}
			StringContent content = new StringContent(body, Encoding.UTF8, bodyMediaType);
			if (!string.IsNullOrWhiteSpace(responseContentType))
			{
				content.Headers.ContentType = MediaTypeHeaderValue.Parse(responseContentType);
			}
			using HttpResponseMessage response = await client.PutAsync(url, content);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStringAsync();
			}
			return string.Empty;
		}

		/// <summary>
		/// 发起GET异步请求
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回string</returns>
		public async Task<string> DeleteAsync(string url, Dictionary<string, string> headers = null, int timeOut = 30)
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(timeOut);
			if (headers != null && headers.Count > 0)
			{
				foreach (string key in headers.Keys)
				{
					client.DefaultRequestHeaders.Add(key, headers[key]);
				}
			}
			using HttpResponseMessage response = await client.DeleteAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStringAsync();
			}
			return string.Empty;
		}

		/// <summary>
		/// 发起GET异步请求
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="url">请求地址</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回T</returns>
		public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null, int timeOut = 30) where T : new()
		{
			string responseString = await GetAsync(url, headers, timeOut);
			if (!string.IsNullOrWhiteSpace(responseString))
			{
				return JsonConvert.DeserializeObject<T>(responseString);
			}
			return default(T);
		}

		/// <summary>
		/// 发起POST异步请求
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="url">请求地址</param>
		/// <param name="body">POST提交的内容</param>
		/// <param name="bodyMediaType">POST内容的媒体类型，如：application/xml、application/json</param>
		/// <param name="responseContentType">HTTP响应上的content-type内容头的值,如：application/xml、application/json、application/text、application/x-www-form-urlencoded等</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回T</returns>
		public async Task<T> PostAsync<T>(string url, string body, string bodyMediaType = null, string responseContentType = null, Dictionary<string, string> headers = null, int timeOut = 15) where T : new()
		{
			string responseString = await PostAsync(url, body, bodyMediaType, responseContentType, headers, timeOut);
			if (!string.IsNullOrWhiteSpace(responseString))
			{
				return JsonConvert.DeserializeObject<T>(responseString);
			}
			return default(T);
		}

		/// <summary>
		/// 发起POST异步请求
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="body">POST提交的内容</param>
		/// <param name="bodyMediaType">POST内容的媒体类型，如：application/xml、application/json</param>
		/// <param name="responseContentType">HTTP响应上的content-type内容头的值,如：application/xml、application/json、application/text、application/x-www-form-urlencoded等</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回string</returns>
		public async Task<string> PostAGVAsync(string url, string body, string bodyMediaType = null, string responseContentType = null, Dictionary<string, string> headers = null, int timeOut = 30)
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(timeOut);
			if (headers != null && headers.Count > 0)
			{
				foreach (string key in headers.Keys)
				{
					client.DefaultRequestHeaders.Add(key, headers[key]);
				}
			}
			StringContent content = new StringContent(body, Encoding.UTF8, bodyMediaType);
			if (!string.IsNullOrWhiteSpace(responseContentType))
			{
				content.Headers.ContentType = MediaTypeHeaderValue.Parse(responseContentType);
			}
			using HttpResponseMessage response = await client.PostAsync(url, content);
			if (response.StatusCode == HttpStatusCode.Created)
			{
				return await response.Content.ReadAsStringAsync();
			}
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> PostMESAsync(string url, string body, string bodyMediaType = null, string responseContentType = null, Dictionary<string, string> headers = null, int timeOut = 30)
		{
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(timeOut);
			if (headers != null && headers.Count > 0)
			{
				foreach (string key in headers.Keys)
				{
					client.DefaultRequestHeaders.Add(key, headers[key]);
				}
			}
			StringContent content = new StringContent(body, Encoding.UTF8, bodyMediaType);
			if (!string.IsNullOrWhiteSpace(responseContentType))
			{
				content.Headers.ContentType = MediaTypeHeaderValue.Parse(responseContentType);
			}
			using HttpResponseMessage response = await client.PostAsync(url, content);
			if (response.StatusCode == HttpStatusCode.Created)
			{
				return await response.Content.ReadAsStringAsync();
			}
			return await response.Content.ReadAsStringAsync();
		}

		/// <summary>
		/// 发起PUT异步请求
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="url">请求地址</param>
		/// <param name="body">POST提交的内容</param>
		/// <param name="bodyMediaType">POST内容的媒体类型，如：application/xml、application/json</param>
		/// <param name="responseContentType">HTTP响应上的content-type内容头的值,如：application/xml、application/json、application/text、application/x-www-form-urlencoded等</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回T</returns>
		public async Task<T> PutAsync<T>(string url, string body, string bodyMediaType = null, string responseContentType = null, Dictionary<string, string> headers = null, int timeOut = 30) where T : new()
		{
			string responseString = await PutAsync(url, body, bodyMediaType, responseContentType, headers, timeOut);
			if (!string.IsNullOrWhiteSpace(responseString))
			{
				return JsonConvert.DeserializeObject<T>(responseString);
			}
			return default(T);
		}

		/// <summary>
		/// 发起DELETE异步请求
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="url">请求地址</param>
		/// <param name="headers">请求头信息</param>
		/// <param name="timeOut">请求超时时间，单位秒</param>
		/// <returns>返回T</returns>
		public async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> headers = null, int timeOut = 30) where T : new()
		{
			string responseString = await DeleteAsync(url, headers, timeOut);
			if (!string.IsNullOrWhiteSpace(responseString))
			{
				return JsonConvert.DeserializeObject<T>(responseString);
			}
			return default(T);
		}

		/// <summary>
		/// 获取请求的主机名
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		private static string GetHostName(string url)
		{
			if (!string.IsNullOrWhiteSpace(url))
			{
				return url.Replace("https://", "").Replace("http://", "").Split(new char[1] { '/' })[0];
			}
			return "AnyHost";
		}
	}
}
