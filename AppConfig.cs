using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlWebApi
{
    public sealed class AppConfig //Configures JSON db string
    {
        private static AppConfig _instance = null; 
        private static readonly object instanceLock = new(); //ReadOnly
        private static IConfigurationRoot _configuration;

#if DEBUG //If debug mode
        private string _appsettingfile = "appsettings.Development.json";
#else
        private string _appsettingfile = "appsettings.json";
#endif

        private AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) //Set path
                .AddJsonFile(_appsettingfile, optional: true, reloadOnChange: true); //Specified JSON file, reloadOnChange

            _configuration = builder.Build(); //Build optionsbuilder
        }
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                lock(instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppConfig();
                    }
                    return _configuration;
                }
            }
        }

    }
}
