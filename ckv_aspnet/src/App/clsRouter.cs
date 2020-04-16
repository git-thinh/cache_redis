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
        public static void _init()
        {
            _add("api/router", () => m_functions.Keys);

            _add("api", clsApi.api_names);
            _add("api/all", clsApi.api_list);
            _add("api/reset", clsApi.api_reload);

            _add("api/global", clsApi.api_global_names);
            _add("api/global/reset", clsApi.api_global_reload);

            _add("api/curl/test-http", clsCURL.get_raw_http);
            _add("api/curl/test-https", clsCURL.get_raw_https);

            _add("api/chakra/test-1", clsChakra.js_chakra_run);
            _add("api/chakra/test-2", clsChakra.js_chakra_run_2);

            //_add("api/job/test-1", clsJobTest.test_create_job_1);
            //_add("api/job/test-2", clsJobTest.test_create_job_2);
            //_add("api/job/test-3", clsJobTest.test_create_job_3);
            //_add("api/job/test-4", clsJobTest.test_create_job_4);
            _add("api/job/create", (p) => { if (p != null) clsJob.create_schedule(p.ToString()); });
        }

        public static bool execute_api(HttpRequest Request, HttpResponse Response)
        {
            string path = Request.Url.AbsolutePath.Substring(1).ToLower();
            if (m_functions.ContainsKey(path))
            {
                string json = string.Empty, content_type = "text/plain";
                try
                {
                    if (Request.HttpMethod == "POST" || (Request.HttpMethod == "GET" && path.Contains('.') == false))
                        content_type = "application/json";

                    var rs = _exe(Request);
                    if (rs.ok) json = rs.data;
                    else json = JsonConvert.SerializeObject(rs);
                }
                catch (Exception e)
                {
                    content_type = "application/json";
                    json = JsonConvert.SerializeObject(new { ok = false, message = e.Message });
                }

                Response.Clear();
                Response.ContentType = content_type;
                Response.Write(json);
                Response.End();

                return true;
            }

            return false;
        }

        #region [ _add ]

        static ConcurrentDictionary<string, object> m_functions = new ConcurrentDictionary<string, object>();

        static void _add(string name, Action func)
        {
            if (func != null)
            {
                m_functions.TryAdd(name, func);
            }
        }

        static void _add(string name, Action<object> func)
        {
            if (func != null)
            {
                m_functions.TryAdd(name, func);
            }
        }

        static void _add<T>(string name, Func<T> func)
        {
            if (func != null)
            {
                m_functions.TryAdd(name, new Func<T>(() =>
                {
                    var val = func();
                    return val;
                }));
            }
        }

        static void _add<T>(string name, Func<object, T> func)
        {
            if (func != null)
            {
                m_functions.TryAdd(name, new Func<object, T>((para_) =>
                {
                    var val = func(para_);
                    return val;
                }));
            }
        }

        #endregion

        static oRouterResult _exe(HttpRequest Request)
        {
            string path = Request.Url.AbsolutePath.Substring(1).ToLower();

            try
            {
                if (m_functions.ContainsKey(path))
                {
                    object para = null;

                    if (Request.HttpMethod == "POST")
                    {
                        para = new StreamReader(Request.InputStream).ReadToEnd();
                        if (para == null || string.IsNullOrWhiteSpace(para.ToString()))
                            return oRouterResult.Error("Body of POST is not null or emtpy");
                    }

                    string url = Request.Url.ToString();
                    int pos = url.IndexOf('?');

                    if (Request.HttpMethod == "GET" && pos != -1)
                        para = Uri.UnescapeDataString(url.Substring(pos + 1));

                    string text = string.Empty;
                    object val = null;

                    var fn = m_functions[path];
                    string fn_type = fn.GetType().Name;
                    switch (fn_type)
                    {
                        case "Action":
                            ((Action)fn)();
                            break;
                        case "Action`1":
                            ((Action<object>)fn)(para);
                            break;
                        case "Func`1":
                            val = ((Func<object>)fn)();
                            break;
                        case "Func`2":
                            val = ((Func<object, object>)fn)(para);
                            break;
                    }

                    if (val != null)
                    {
                        string type = val.GetType().Name;
                        if (type[0] == 'o' || type.Contains('[') || type.Contains('`'))
                            text = JsonConvert.SerializeObject(val);
                        else
                            text = val.ToString();
                    }

                    return oRouterResult.Ok(text);
                }
                else return oRouterResult.Error("Cannot found function: " + path);
            }
            catch (Exception e)
            {
                return oRouterResult.Error(e.Message);
            }
        }

    }

}