using System;
using System.Collections.Concurrent;

namespace PerlWebApi
{
    public class AppLogItem
    {
        public string Type { get; set; }

        public string[] Info { get; set; }
    }

    public sealed class AppLog
    {
        private static AppLog _instance = null;

        private static ConcurrentStack<AppLogItem> _logStack = null;

        private AppLog()
        {
            _logStack = new ConcurrentStack<AppLogItem>();
        }

        public static AppLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppLog();
                }
                return _instance;
            }
        }
        public void LogInformation(params string[] info)
        {
            _logStack.Push(new AppLogItem { Type = "Information", Info = info });
        }
        public void LogDBConnection(params string[] info)
        {
            _logStack.Push(new AppLogItem { Type = "DBConnection", Info = info });
        }

        public void LogException(Exception ex)
        {
            _logStack.Push(new AppLogItem
            {
                Type = "Exception",
                Info = new string[] { ex.Message, ex.InnerException.Message }
            });
        }

        public AppLogItem[] ToArray()
        {
            return _logStack.ToArray();
        }
    }
}
