using System;
using System.Collections.Concurrent;

namespace PerlWebApi
{
    public class AppLogItem
    {

    }

	public sealed class AppLog
	{
		public static object Instance { get; internal set; }
	}
}
