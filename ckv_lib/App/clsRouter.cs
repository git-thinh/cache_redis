using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ckv_lib
{
    public class clsRouter
    {
        public static void _init()
        {
            _add("api/router", new Func<Dictionary<string, object>, oResult>((p) => { return oResult.Ok(m_functions.Keys, p); }));

            _add("api", new Func<Dictionary<string, object>, oResult>((p) => { return oResult.Ok(clsApi.api_names(), p); }));
            _add("api_js", new Func<Dictionary<string, object>, oResult>((p) => { return oResult.Ok(clsChakra.js_apis(), p); }));
            _add("api/all", new Func<Dictionary<string, object>, oResult>((p) => { return oResult.Ok(clsApi.api_list(), p); }));
            _add("api/reset", new Func<Dictionary<string, object>, oResult>((p) => { clsApi.api_reload(); return oResult.Ok(true, p); }));

            _add("api/global", new Func<Dictionary<string, object>, oResult>((p) => { return oResult.Ok(clsApi.api_global_names(), p); }));
            _add("api/global/reset", new Func<Dictionary<string, object>, oResult>((p) => { clsApi.api_global_reload(); return oResult.Ok(true, p); }));


            _add("api/curl/test-http", clsCURL.test_http);
            _add("api/curl/test-https", clsCURL.test_https);

            _add("api/chakra/test-1", clsChakra.test_1);
            _add("api/chakra/test-2", clsChakra.test_2);

            _add("api/js/test", clsChakra.run_api);
        }

        public static bool execute_api(HttpRequest Request, HttpResponse Response)
        {
            string path = Request.Url.AbsolutePath.Substring(1).ToLower();
            if (m_functions.ContainsKey(path))
            {
                string text = string.Empty, content_type = "text/plain";
                try
                {
                    if (Request.HttpMethod == "POST" || (Request.HttpMethod == "GET" && path.Contains('.') == false))
                        content_type = "application/json";

                    var v = _exe(Request);
                    if (v.ok)
                    {
                        switch (v.type)
                        {
                            case DATA_TYPE.HTML_TEXT:
                                content_type = "text/html";
                                text = v.data.ToString();
                                break;
                            case DATA_TYPE.JSON_TEXT:
                                content_type = "application/json";
                                text = JsonConvert.SerializeObject(v);
                                break;
                            case DATA_TYPE.JSON_RESPONSE:
                                content_type = "application/json";
                                text = v.data.ToString();
                                break;
                            case DATA_TYPE.TEXT_PLAIN:
                                content_type = "text/plain";
                                text = v.data.ToString();
                                break;
                            case DATA_TYPE.OBJECT:
                                break;
                            case DATA_TYPE.ARRAY_LIST:
                                break;
                            case DATA_TYPE.BUFFER:
                                break;
                        }
                    }
                    else
                    {
                        content_type = "application/json";
                        text = JsonConvert.SerializeObject(v);
                    }
                }
                catch (Exception e)
                {
                    content_type = "application/json";
                    text = JsonConvert.SerializeObject(new { ok = false, message = e.Message });
                }

                Response.Clear();
                Response.ContentType = content_type;
                Response.Write(text);
                Response.End();

                return true;
            }

            return false;
        }


        static ConcurrentDictionary<string, Func<Dictionary<string, object>, oResult>> m_functions = new ConcurrentDictionary<string, Func<Dictionary<string, object>, oResult>>();
        static void _add(string name, Func<Dictionary<string, object>, oResult> func) { if (func != null) m_functions.TryAdd(name, func); }

        static oResult _exe(HttpRequest Request)
        {
            string path = Request.Url.AbsolutePath.Substring(1).ToLower();
            Dictionary<string, object> para = new Dictionary<string, object>() {
                { "___domain", Request.Url.Host },
                { "___port", Request.Url.Port },
                { "___url", path },
                { "___method", Request.HttpMethod },
                { "___token", string.Empty },
            };

            try
            {
                if (m_functions.ContainsKey(path))
                {
                    var a = Request.QueryString.Keys.Cast<string>().ToArray();
                    foreach (var key in a) if (!para.ContainsKey(key)) para.Add(key, Uri.UnescapeDataString(Request.QueryString[key]));

                    if (Request.HttpMethod == "POST")
                    {
                        string s = new StreamReader(Request.InputStream).ReadToEnd();
                        if (s == null || string.IsNullOrWhiteSpace(s))
                            return oResult.Error("Body of POST is not null or emtpy");
                        else
                        {
                            s = s.Trim();
                            if (s.Length > 1 && s[0] == '{' && s[s.Length - 1] == '}')
                            {
                                try
                                {
                                    var d = JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
                                    foreach (var kv in d) para.Add(kv.Key, kv.Value);
                                }
                                catch (Exception e)
                                {
                                    return oResult.Error("Convert to json of body error: " + e.Message);
                                }
                            }
                            else
                            {
                                para.Add("___BODY", s);
                            }
                        }
                    }

                    var fn = m_functions[path];
                    var v = ((Func<Dictionary<string, object>, oResult>)fn)(para);
                    return v;
                }
                else return oResult.Error("Cannot found function: " + path, para);
            }
            catch (Exception e)
            {
                return oResult.Error(e.Message, para);
            }
        }
    }
}