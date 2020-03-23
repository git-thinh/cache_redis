using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cache_redis
{
    public class oRedisInfo
    {
        public int id { set; get; }
        public string scope { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public bool enable { set; get; }
    }

    public class oConfig
    {
        public string name { set; get; }
        public string description { set; get; }
        public int port_api { set; get; }
        public int port_ws { set; get; }
        public int port_tcp { set; get; }
        public List<oRedisInfo> list_redis { set; get; }
    }

    public class oRedisCmd
    {
        public string data { set; get; }
        public int total { set; get; }
        public int count { set; get; }
        public string text { set; get; }
        public string cmd { set; get; }
        public bool ok { set; get; } 
    }

    public class oResponseJson
    {
        public oRedisCmd[] requests { set; get; }
        public string message { set; get; }
        public bool ok { set; get; }

        public string Ok(string msg = "")
        {
            ok = true;
            message = msg;
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string Error(string msg = "")
        {
            ok = false;
            message = msg;
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

}
