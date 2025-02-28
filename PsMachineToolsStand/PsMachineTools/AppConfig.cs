using Microsoft.Extensions.Configuration;

namespace PsMachineTools
{
	/// <summary>应用程序配置，兼容老的部分程序架构，可以从 Configuration 静态属性中读取当前程序配置
	/// </summary>
	public class AppConfig
	{
		/// <summary>当前应用程序配置
		/// </summary>
		public static IConfiguration Configuration { get; private set; }

		/// <summary>把新的应用程序配置写入进来
		/// </summary>
		/// <param name="configuration"></param>
		public static void CreateConfiguration(IConfiguration configuration)
		{
			Configuration = configuration;
		}
	}
}
