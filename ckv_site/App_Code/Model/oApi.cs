using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ckv_site
{
    public class oApi
    {
        public bool ok { set; get; }
        public string name { set; get; }
        public string path { set; get; }
        public string sql_cache { set; get; }
        public string error { set; get; }
        public Dictionary<string, object> config { set; get; }
        public oApiAddnew addnew { set; get; }

        [JsonIgnore]
        public ConcurrentDictionary<string, string> apis_data { set; get; }
        public string[] apis { get { return apis_data.Keys.ToArray(); } }
        public void apis_reset()
        {
            if (apis_data == null) apis_data = new ConcurrentDictionary<string, string>();
            else apis_data.Clear();

            string[] fs = Directory.GetFiles(this.path, "*.js");
            foreach (string f in fs)
            {
                string file = Path.GetFileName(f);
                file = file.Substring(0, file.Length - 3).ToLower();
                apis_data.TryAdd(file, File.ReadAllText(f));
            }
        }

        void _init()
        {
            this.apis_data = new ConcurrentDictionary<string, string>();
            this.config = new Dictionary<string, object>();
            this.addnew = new oApiAddnew();
            this.error = string.Empty;
            this.sql_cache = string.Empty;
            this.path = string.Empty;
            if (string.IsNullOrWhiteSpace(this.name)) this.name = Guid.NewGuid().ToString();
        }

        public oApi() { _init(); }

        public oApi(string dir) : base()
        {
            if (Directory.Exists(dir))
            {
                this.path = dir;
                this.name = Path.GetFileName(dir).ToLower();
                string file = Path.Combine(dir, "config.json");
                if (File.Exists(file) == false)
                {
                    //_init();
                    this.error = "Cannot found file config.json";
                    return;
                }
                try
                {
                    this.config = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(file));
                }
                catch (Exception e)
                {
                    //_init();
                    this.error = "Error read json file config.json: " + e.Message;
                    return;
                }

                file = Path.Combine(dir, "cache.sql");
                if (File.Exists(file)) this.sql_cache = File.ReadAllText(file);

                this.apis_reset();

                this.ok = true;
            }
        }
    }
}