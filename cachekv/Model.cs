﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace cachekv
{
    ////    public class oPost
    ////    {
    ////        public bool ok { set; get; }
    ////        public int code { set; get; }
    ////        public string message { set; get; }
    ////    }

    ////    public class oPostItem
    ////    {
    ////        public long ___tid { set; get; }
    ////        public string ___api { set; get; }
    ////        public string ___do { set; get; }
    ////        public string ___dbs { set; get; }
    ////        public long id { set; get; }
    ////        public Dictionary<string,object> input { set; get; }
    ////        public Dictionary<string,object> ouput { set; get; }
    ////    }

    ////    public class oError
    ////    {
    ////        public bool ok { set; get; }
    ////        public int code { set; get; }
    ////        public string message { set; get; }

    ////        public static string getJson(string message_, int code_ = 0)
    ////        {
    ////            var m = new oError() { ok = false, code = code_, message = message_ };
    ////            return JsonConvert.SerializeObject(m, Formatting.Indented);
    ////        }
    ////    }

    ////    public class oSearchResult
    ////    {
    ////        public long[] Keys { set; get; }
    ////        public string[] Errors { set; get; }
    ////        public oSearchResult()
    ////        {
    ////            this.Keys = new long[] { };
    ////            this.Errors = new string[] { };
    ////        }
    ////    }

    public class oRedisInfo
    {
        [JsonIgnore]
        public Process process { set; get; }

        public int id { set; get; }
        public string scope { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public bool enable { set; get; }
        public bool ready { set; get; }
        public bool busy { set; get; }
        public int port { set; get; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", name, port);
        }
    }

    public class oConfig
    {
        public string name { set; get; }
        public string description { set; get; }
        public Dictionary<string, string> db_connect { set; get; }

        public int port_api { set; get; }
        public int port_ws { set; get; }
        public int port_tcp { set; get; }

        public bool enable { set; get; }
        public bool ready { set; get; }
        public bool busy { set; get; }

        public List<oRedisInfo> list_cache { set; get; }

        public override string ToString()
        {
            return string.Format("API:{0}", port_api);
        }
    }

    ////public class oRedisCmd
    ////{
    ////    public object output { set; get; }
    ////    public int total { set; get; }
    ////    public int count { set; get; }
    ////    public string input { set; get; }
    ////    public string cmd { set; get; }
    ////    public bool ok { set; get; } 
    ////}

    ////public class oResponseJson
    ////{
    ////    public oRedisCmd[] requests { set; get; }
    ////    public string message { set; get; }
    ////    public bool ok { set; get; }

    ////    public oResponseJson() { }
    ////    public oResponseJson(oRedisCmd[] requests_ = null) => requests = requests_;

    ////    public oResponseJson(object data_ = null)
    ////    {
    ////        if (data_ != null)
    ////            requests = new oRedisCmd[] { new oRedisCmd() { output = data_ } };
    ////    }

    ////    public string Ok(string msg = "")
    ////    {
    ////        ok = true;
    ////        message = msg;
    ////        return JsonConvert.SerializeObject(this, Formatting.Indented);
    ////    }

    ////    public string Error(string msg = "")
    ////    {
    ////        ok = false;
    ////        message = msg;
    ////        return JsonConvert.SerializeObject(this, Formatting.Indented);
    ////    }
    ////}

}
