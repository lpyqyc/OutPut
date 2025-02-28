using Newtonsoft.Json;

namespace ServiceData
{
	public abstract class JsonObject
	{
		public string GetJsonText()
		{
			JsonSerializerSettings jsetting = new JsonSerializerSettings();
			jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
			return JsonConvert.SerializeObject(this, Formatting.None, jsetting);
		}

		public static T Parse<T>(string myJsonText)
		{
			return JsonConvert.DeserializeObject<T>(myJsonText);
		}
	}
}
