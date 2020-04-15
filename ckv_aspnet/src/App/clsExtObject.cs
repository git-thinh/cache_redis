using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ckv_aspnet
{
    public static class clsExtObject
    {
        public static T getValueByKey<T>(this ConcurrentDictionary<string, object> dic, string key)
        {
            if (dic != null && key != null && dic.ContainsKey(key))
            {
                try
                {
                    object val = dic[key];
                    if (val != null) return (T)val;
                }
                catch { }
            }
            var v = default(T);
            return v;
        }

        public static T getValueByKey<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic != null && key != null && dic.ContainsKey(key))
            {
                try
                {
                    object val = dic[key];
                    if (val != null) return (T)val;
                }
                catch { }
            }
            var v = default(T);
            return v;
        }

    }
}