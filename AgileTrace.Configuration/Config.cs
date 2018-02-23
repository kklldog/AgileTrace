using System;
using System.Dynamic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AgileTrace.Configuration
{
    public class AppSetting
    {
        public dynamic sa { get; set; }

        public dynamic store { get; set; }

        public dynamic node { get; set; }
    }

    public class Config
    {
        private static IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
                AppSetting = BuildAppsetting(_configuration);

            }
        }

        public static AppSetting AppSetting { get; set; }

        private static AppSetting BuildAppsetting(IConfiguration configuration)
        {
            var setting = new AppSetting();

            setting.sa = new ExpandoObject();
            setting.sa.name = configuration["sa:name"];
            setting.sa.password = configuration["sa:password"];

            setting.store = new ExpandoObject();
            setting.store.type = configuration["store:type"];
            setting.store.connection = configuration["store:connection"];

            return setting;
        }
    }
}
