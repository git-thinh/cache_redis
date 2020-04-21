using Newtonsoft.Json;
using Sider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace ckv_lib
{
    public interface ILogJob
    {
        void write(string scope_name, string key, string text);
    }

    public class clsLogJob : ILogJob
    {
        static long ID_INCREMENT = 0;
        static RedisClient _redis;
        static bool _connected = false;

        public clsLogJob(int port = 0) => _init(port == 0 ? _CONFIG.LOG_PORT_REDIS : port);

        void _init(int port)
        {
            try
            {
                if (_connected == false)
                {
                    _redis = new RedisClient("localhost", port);
                    _connected = true;
                }
            }
            catch { 
            }
        }

        public void write(string scope_name, string key, string text)
        {
            if (_connected == false) return;
            if (text == null) text = string.Empty;

            try
            {
                _redis.HSet(scope_name, key, text);
            }
            catch
            {
            }
        }

        void write1(string scope_name, string key, params object[] paras)
        {
            Interlocked.Increment(ref ID_INCREMENT);
            if (ID_INCREMENT == int.MaxValue) ID_INCREMENT = 0;

            string id = string.Format("{0}.{1}.{2}", DateTime.Now.ToString("yyMMddHHmm"), ID_INCREMENT, key);

            StringBuilder bi = new StringBuilder(DateTime.Now.ToString("yyMMdd.HHmmss.fff"));
            bi.Append(Environment.NewLine);
            try
            {
                for (var i = 0; i < paras.Length; i++)
                {
                    string type = paras[i].GetType().Name;
                    switch (type)
                    {
                        case "Boolean":
                        case "Byte":
                        case "SByte":
                        case "Char":
                        case "Decimal":
                        case "Double":
                        case "Single":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Int16":
                        case "UInt16":
                        case "String":
                            bi.Append(paras[i].ToString());
                            break;
                        default:
                            string text = JsonConvert.SerializeObject(paras[i], Formatting.Indented);
                            bi.Append(text);
                            bi.Append(Environment.NewLine);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string text = JsonConvert.SerializeObject(paras, Formatting.Indented);
                bi.Append(text);
                bi.Append(Environment.NewLine);
                bi.Append("ERROR_LOG: " + ex.Message);
            }

            try
            {
                _redis.HSet(scope_name, id, bi.ToString());
            }
            catch { }
        }
    }
}