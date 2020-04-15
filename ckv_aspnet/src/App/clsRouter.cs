using ckv_aspnet.src.Chakra;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ckv_aspnet
{
    public class clsRouter
    {
        static ConcurrentDictionary<string, object> m_functions = new ConcurrentDictionary<string, object>();
        public static void _init()
        {
            //FN.TryAdd("config", new Func<object, string>((o) => { return JsonConvert.SerializeObject(CF); }));
            //FN.TryAdd("pipe", new Func<object, string>((o) =>
            //{
            //    var client = new NamedPipeClientStream("ckv." + CF.name);
            //    client.Connect();
            //    string p = new StreamReader(client).ReadToEnd().Trim();
            //    return JsonConvert.SerializeObject(new { port = p });
            //}));

            //FN.TryAdd("file_reset", new Func<object, string>((o) => { file_load(); return JsonConvert.SerializeObject(file_data.Keys); }));

            //FN.TryAdd("clear", new Func<object, string>((o) => { mem_cache_clear(); return "OK"; }));
            //FN.TryAdd("load_db", new Func<object, string>(mem_cache_db));

            //FN.TryAdd("keys", new Func<object, string>((o) => { return JsonConvert.SerializeObject(mem_store.Keys); }));
            //FN.TryAdd("item", new Func<object, string>(mem_cache_item));
            //FN.TryAdd("all", new Func<object, string>(mem_cache_all));

            //FN.TryAdd("remove", new Func<object, string>(mem_cache_remove));
            //FN.TryAdd("update", new Func<object, string>(mem_cache_update));
            //FN.TryAdd("addnew", new Func<object, string>(mem_cache_addnew));

            m_functions.TryAdd("url_get_raw", new Func<object, string>(clsCURL.get_raw));
            m_functions.TryAdd("url_get_text", new Func<object, string>(clsCURL.get_text));

            //FN.TryAdd("v8_get_raw", new Func<object, string>(v8_get_raw));

            m_functions.TryAdd("api", new Func<object, string>((o) => JsonConvert.SerializeObject(m_functions.Keys)));


            m_functions.TryAdd("js_chakra-1", new Func<object, string>(clsChakra.js_chakra_run));
            m_functions.TryAdd("js_chakra-2", new Func<object, string>(clsChakra.js_chakra_run_2));
            m_functions.TryAdd("curl-https", new Func<object, string>(clsCURL.get_raw_https));
            m_functions.TryAdd("curl-http", new Func<object, string>(clsCURL.get_raw));
        }

        public static void set_router(HttpRequest Request, HttpResponse Response)
        {
            string url = Request.Url.ToString();
            string path = Request.Url.AbsolutePath.Substring(1);

            if (m_functions.ContainsKey(path)) {
                string json = string.Empty;
                try
                {
                    bool ok = true;
                    object pr = null;
                    if (Request.HttpMethod == "POST")
                    {
                        string sbody = new StreamReader(Request.InputStream).ReadToEnd();
                        if (string.IsNullOrWhiteSpace(sbody))
                        {
                            json = JsonConvert.SerializeObject(new { ok = false, message = "Body of POST is not null or emtpy" });
                            ok = false;
                        }
                        else pr = sbody;
                    }
                    if (ok)
                    {
                        var fn = (Func<object, string>)m_functions[path];
                        int pos = url.IndexOf('?');

                        if (Request.HttpMethod == "GET" && pos != -1)
                            pr = Uri.UnescapeDataString(url.Substring(pos + 1));

                        json = fn(pr);
                    }
                }
                catch (Exception e)
                {
                    json = JsonConvert.SerializeObject(new { ok = false, message = e.Message });
                }
                Response.Clear();
                Response.Write(json);
                Response.End();
                return;
            }             
        }
    }
}