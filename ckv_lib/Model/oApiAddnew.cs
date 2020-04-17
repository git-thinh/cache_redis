using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ckv_lib
{

    public class oApiAddnew
    {
        public bool ok { set; get; }
        public string name { set; get; }
        public string path { set; get; }
        public string error { set; get; }
        public string sql_insert { set; get; }
        public Dictionary<string, object> schema { set; get; }
        public Dictionary<string, object> valid { set; get; }

        void _init()
        {
            this.name = string.Empty;
            this.sql_insert = string.Empty;
            this.path = string.Empty;
            this.schema = new Dictionary<string, object>();
            this.valid = new Dictionary<string, object>();
        }

        public oApiAddnew() => _init();

        public oApiAddnew(string dir) : base()
        {
            if (Directory.Exists(dir))
            {
                this.path = dir;
                this.name = Path.GetFileName(dir).ToLower();
                string file = Path.Combine(dir, "addnew.json");
                if (File.Exists(file))
                {
                    try
                    {
                        this.schema = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(file));
                    }
                    catch (Exception e)
                    {
                        //_init();
                        this.error = "Error read json file addnew.json: " + e.Message;
                        return;
                    }
                }

                file = Path.Combine(dir, "addnew.sql");
                if (File.Exists(file)) this.sql_insert = File.ReadAllText(file);

                file = Path.Combine(dir, "addnew.valid");
                if (File.Exists(file))
                {
                    try
                    {
                        this.valid = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(file));
                    }
                    catch (Exception e)
                    {
                        //_init();
                        this.error = "Error read json file addnew.valid: " + e.Message;
                        return;
                    }
                }

                this.ok = true;
            }
        }
    }
}