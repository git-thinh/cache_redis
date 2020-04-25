using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ckv_site
{
    public interface IApi {
        bool cache_exist(string cache_name);
        oResult execute(string cache_name, string do_action, string id = "", Dictionary<string, object> parameters = null);
    }

    public class clsApi: IApi
    {
        public ICache m_cache { get; private set; }
        public clsApi(ICache cache) { m_cache = cache; }

        public bool cache_exist(string cache_name) { return m_cache.Exist(cache_name); }

        public oResult execute(string cache_name, string do_action, string id = "", Dictionary<string, object> parameters = null) {
            return m_cache.ExecuteAPI(cache_name, do_action, id, parameters);
        }
    }
}