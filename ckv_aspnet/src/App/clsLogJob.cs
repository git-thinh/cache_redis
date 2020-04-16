using Newtonsoft.Json;
using Sider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace ckv_aspnet
{
    public interface ILogJob {
        void write(string group, string key, int index, params object[] paras);
    }

    public class clsLogJob : ILogJob
    {
        static long ID_INCREMENT = 0;
        static RedisClient _redis;
        static bool _inited = false;
        void _init() {
            if (_inited == false) {
                _inited = true;
                _redis = new RedisClient("127.0.0.1", 11111);
            }
        }

        public void write(string group, string key, int index, params object[] paras)
        {
            if (paras == null) return;

            _init();
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
                _redis.HSet(group, id, bi.ToString());
            }
            catch { }
        }
    }
}