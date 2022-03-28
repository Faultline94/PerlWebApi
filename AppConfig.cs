using Microsoft.Extensions.Configuration;
using System.IO;

namespace PerlWebApi
{
	public sealed class AppConfig
	{
		public static object ConfigurationRoot { get; internal set; }
	}
}
