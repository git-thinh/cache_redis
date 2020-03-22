using System;
using System.Collections.Generic;
using System.Text;

namespace cache_redis
{
    public class oRedis
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
        public string dir_ui { set; get; }
        public int port_api { set; get; }
        public int port_ws { set; get; }
        public List<oRedis> list_redis { set; get; }
    }
}
