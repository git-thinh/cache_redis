using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ckv_lib
{
    public class _CONFIG
    {
        const int LOG_PORT_REDIS_DEFAULT = 11111; 

        public static string PATH_ROOT = string.Empty;
        public static int LOG_PORT_REDIS = LOG_PORT_REDIS_DEFAULT;
        public static string PATH_DATA_FILE = string.Empty;

        public static void _init() {
            PATH_ROOT = AppDomain.CurrentDomain.BaseDirectory;

            PATH_DATA_FILE = WebConfigurationManager.AppSettings["PATH_DATA_FILE"];
            if (string.IsNullOrWhiteSpace(PATH_DATA_FILE)) {
                PATH_DATA_FILE = Path.Combine(PATH_ROOT, "_data");
                if (!Directory.Exists(PATH_DATA_FILE)) Directory.CreateDirectory(PATH_DATA_FILE);
            }

            string portLogRedis = WebConfigurationManager.AppSettings["LOG_PORT_REDIS"];
            if (!string.IsNullOrWhiteSpace(portLogRedis))  {
                int.TryParse(portLogRedis, out LOG_PORT_REDIS);
                if (LOG_PORT_REDIS <= 0) LOG_PORT_REDIS = LOG_PORT_REDIS_DEFAULT;
            }
        }
    }
}