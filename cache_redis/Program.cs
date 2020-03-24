using ChakraHost.Hosting;
using Fleck2;
using Fleck2.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace cache_redis
{
    class Program
    {
        static readonly string ROOT_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static readonly string ROOT_PATH_UI = Path.Combine(ROOT_PATH, "ui");

        static oConfig m_config;
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(Program).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        byte[] buffer = new byte[stream.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                ms.Write(buffer, 0, read);
                            buffer = ms.ToArray();
                        }
                        asm = Assembly.Load(buffer);
                    }
                }
                return asm;
            };
        }

        #region [ ENGINE JAVASCRIPT ]

        static JavaScriptRuntime runtime;
        static JavaScriptContext context;
        static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        static bool js___connected = false;
        static string js___libs_text = string.Empty;

        static void js___init()
        {
            js___connected = true;

            Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime);
            Native.JsCreateContext(runtime, out context);
            Native.JsSetCurrentContext(context);

            if (File.Exists("lib.js")) js___libs_text = File.ReadAllText("lib.js");
            if (js___libs_text.Length > 0)
                Native.JsRunScript(js___libs_text, currentSourceContext++, "", out JavaScriptValue r1);
        }

        static void test_index(string json)
        {
            if (!js___connected) js___init();

            using (new JavaScriptContext.Scope(context))
            {
                try
                {
                    JavaScriptValue result;
                    result = JavaScriptContext.RunScript("(()=>{ var o = " + json + "; \r\n return ___index(o); })()", currentSourceContext++, "");

                    JavaScriptValue numberResult = result.ConvertToString();
                    var val = numberResult.ToString();
                }
                catch (JavaScriptScriptException e)
                {
                }
                catch (Exception e)
                {
                }
            }
        }

        static void test_search(string[] a)
        {
            List<long> ls = new List<long>() { };
            List<string> errs = new List<string>() { };

            if (!js___connected) js___init();

            if (a.Length > 0)
            {
                using (new JavaScriptContext.Scope(context))
                {
                    string fn = "___" + Guid.NewGuid().ToString().Replace('-', '_');
                    string filter = " ___fn." + fn + " = function(o){ try { return o.id != null && o.id % 2 == 0; }catch(e){ return { ok: false, code: 500, id: o.id, message: e.message }; } }; ";
                    JavaScriptContext.RunScript(filter, currentSourceContext++, "");

                    for (var i = 0; i < a.Length; i++)
                    {
                        try
                        {
                            string js_exe = "(()=>{ var o = " + a[i] + "; var ok = ___fn." + fn + "(o); if(ok == true) return o.id; else if(ok == false) return -1; else return JSON.stringify(ok); })()";
                            var result = JavaScriptContext.RunScript(js_exe, currentSourceContext++, "");
                            string v = result.ConvertToString().ToString();
                            if (v.Length > 20) errs.Add(v);
                            else
                            {
                                long id = -1;
                                long.TryParse(v, out id);
                                if (id != -1) ls.Add(id);
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    JavaScriptContext.RunScript(" delete ___fn." + fn, currentSourceContext++, "");
                }
            }
        }

        #endregion

        #region [ API BASE ]

        static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (Directory.Exists(path) == false) return new string[] { };

            if (string.IsNullOrEmpty(searchPattern) || searchPattern == "*.*")
                return Directory.GetFiles(path, "*.*", searchOption).Select(x => x.ToLower()).ToArray();

            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(Directory.GetFiles(path, sp, searchOption).Select(x => x.ToLower()));
            files.Sort();
            return files.ToArray();
        }

        static Stream api___stream_string(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void api___response_stream(string _extension, Stream input, HttpListenerContext context)
        {
            try
            {
                //Stream input = new FileStream(filename, FileMode.Open);
                //Stream input = api___stream_string(data);

                //Adding permanent http response headers
                string contentType = HTTPServerUI.GetContentType(_extension);
                context.Response.ContentType = contentType;
                context.Response.ContentLength64 = input.Length;
                //context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                //context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

                byte[] buffer = new byte[1024 * 16];
                int nbytes;
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
                input.Close();

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }

        static void api___response_json_error(string message, HttpListenerContext context)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new oResponseJson().Error(message));
                Stream input = api___stream_string(json);
                api___response_stream(".json", input, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.OutputStream.Close();
        }

        static void api___response_json_ok(object obj, HttpListenerContext context)
        {
            try
            {
                string json;

                if (obj == null)
                    json = JsonConvert.SerializeObject(new oResponseJson().Ok());
                else
                    json = JsonConvert.SerializeObject(new oResponseJson().Ok());

                Stream input = api___stream_string(json);
                api___response_stream(".json", input, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.OutputStream.Close();
        }

        #endregion

        #region [ API PROCESS ]

        static bool api___redis_check_ready(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api.Length == 0)
            {
                api___response_json_error("Uri must be / " + (a.Length > 0 ? a[1] : "[ACTION_NAME]") + "/[CACHE_NAME]", context);
                return false;
            }

            if (m_redis.ContainsKey(api) == false)
            {
                api___response_json_error("Cache engine " + api + " not exist", context);
                return false;
            }

            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            if (cf == null)
            {
                api___response_json_error("Config " + api + " not exist", context);
                return false;
            }

            if (m_config.db_connect == null || m_config.db_connect.Count == 0 || m_config.db_connect.ContainsKey(cf.scope) == false)
            {
                api___response_json_error("db_connect not contain connectString " + cf.scope, context);
                return false;
            }

            return true;
        }

        static bool api___redis_reset(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];


            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_search(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];

            string[] keys = redis.Keys;
            int max = keys.Length;
            string[] rs = new string[max];
            for (var i = 0; i < max; i++)
                rs[i] = ASCIIEncoding.UTF8.GetString(redis.Get(keys[i]));

            //test_index(rs[0]);
            test_search(rs);

            //string json = "[" + string.Join(",", rs) + "]";
            //Stream input = api___stream_string(json);
            //api___response_stream(".json", input, context);

            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_get_top(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];

            string[] keys = redis.Keys;
            int max = keys.Length < 10 ? keys.Length : 10;
            string[] rs = new string[max];
            for (var i = 0; i < max; i++)
                rs[i] = ASCIIEncoding.UTF8.GetString(redis.Get(keys[i]));
            string json = "[" + string.Join(",", rs) + "]";

            Stream input = api___stream_string(json);
            api___response_stream(".json", input, context);
            return true;
        }

        static bool api___redis_bgsave(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.BackgroundSave();
            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_save(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.Save();
            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_clean_all(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.FlushDb();
            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_reload_db(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];

            string file_sql = Path.Combine(ROOT_PATH, "config\\sql\\" + cf.name + ".sql");
            if (File.Exists(file_sql) == false)
            {
                api___response_json_error("File " + file_sql + " not exist", context);
                return false;
            }
            string sql_select = File.ReadAllText(file_sql);

            redis.FlushDb();

            string json;
            bool existID = false;
            Dictionary<string, string> rows = new Dictionary<string, string>() { };
            using (var cn = new SqlConnection(m_config.db_connect[cf.scope]))
            {
                cn.Open();
                try
                {
                    var cm = cn.CreateCommand();
                    cm.CommandText = sql_select;

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var columns = new string[reader.FieldCount];
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                columns[i] = reader.GetName(i).ToLower();
                                if (columns[i] == "id") existID = true;
                            }

                            int k = 0;
                            long id = 0;
                            while (reader.Read())
                            {
                                var dic = new Dictionary<string, object>();
                                for (var i = 0; i < reader.FieldCount; i++)
                                    dic.Add(columns[i], reader.GetValue(i));

                                id = k;
                                if (existID) long.TryParse(dic["id"].ToString(), out id);
                                json = JsonConvert.SerializeObject(dic);
                                rows.Add(id.ToString(), json);
                                k++;
                            }
                        }
                    }
                }
                catch (Exception e111)
                {

                }
                cn.Close();
            }

            redis.Set(rows);

            api___response_json_ok(null, context);
            return true;
        }

        static readonly Func<HttpListenerContext, oRedisCmd[], bool> HTTP_API_PROCESS_CMD = (context, cmds) =>
        {

            return false;
        };

        static readonly Func<HttpListenerContext, bool> HTTP_API_PROCESS = (context) =>
        {
            string method = context.Request.HttpMethod,
                path = context.Request.Url.AbsolutePath,
                text = string.Empty, filename = path.Substring(1);
            string[] files, dirs;
            Stream input;

            switch (method)
            {
                #region [ GET ]
                case "GET":
                    filename = path.Substring(1);
                    input = api___stream_string("Cannot found the file: " + path);

                    switch (filename)
                    {
                        case "list.html":
                        case "list":
                            #region
                            dirs = Directory.GetDirectories(ROOT_PATH_UI);
                            files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);

                            files = files.Select(x => string.Format(@"<a href=""{0}"" target=_blank>{0}</a></br>", Path.GetFileName(x))).ToArray();
                            dirs = dirs.Select(x => string.Format(@"<a href=""{0}"" target=_blank>{0}</a></br>", Path.GetFileName(x))).ToArray();

                            text = string.Join(string.Empty, dirs) + string.Join(string.Empty, files) +
                                //@"<a href=""/config"">config</a></br>" +
                                @"<a href=""/config"" target=_blank>config</a></br>";

                            input = api___stream_string(text);
                            filename = "list.html";
                            break;
                        #endregion
                        case "config":
                        case "config.html":
                            #region
                            text = JsonConvert.SerializeObject(m_config);
                            input = api___stream_string(text);
                            api___response_stream(".json", input, context);
                            break;
                        #endregion
                        default:
                            string[] a = context.Request.Url.Segments;
                            string cmd = a.Length > 0 ? a[1] : "";

                            switch (cmd)
                            {
                                case "reload_db/": return api___redis_reload_db(context);
                                case "reset/": return api___redis_reset(context);
                                case "bgsave/": return api___redis_bgsave(context);
                                case "save/": return api___redis_save(context);
                                case "clean_all/": return api___redis_clean_all(context);
                                case "top/": return api___redis_get_top(context);
                                case "search/": return api___redis_search(context);
                            }

                            #region

                            if (string.IsNullOrEmpty(filename))
                            {
                                files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);
                                string fileIndex = files.Where(x => x.EndsWith("index.htm") || x.EndsWith("index.html")).SingleOrDefault();
                                if (fileIndex != null) filename = Path.GetFileName(fileIndex);
                            }

                            filename = Path.Combine(ROOT_PATH_UI, filename);
                            if (File.Exists(filename))
                                input = new FileStream(filename, FileMode.Open);
                            else
                            {
                                if (Directory.Exists(filename))
                                {
                                    dirs = Directory.GetDirectories(filename);
                                    files = GetFiles(filename, "*.*", SearchOption.TopDirectoryOnly);

                                    files = files.Select(x => string.Format(@"<a href=""{0}/{1}"">{1}</a></br>", path, Path.GetFileName(x))).ToArray();
                                    dirs = dirs.Select(x => string.Format(@"<a href=""{0}/{1}"">{1}</a></br>", path, Path.GetFileName(x))).ToArray();

                                    text = string.Join(string.Empty, dirs) + string.Join(string.Empty, files);
                                    input = api___stream_string(text);
                                    filename = ".html";
                                }
                                else
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    context.Response.OutputStream.Close();
                                    return false;
                                }
                            }
                            #endregion
                            break;
                    }

                    //Console.WriteLine(path);
                    api___response_stream(Path.GetExtension(filename), input, context);

                    break;
                #endregion

                #region [ POST ]
                case "POST":
                    input = context.Request.InputStream;
                    if (input != null && input.Length > 0)
                    {
                        input.Position = 0;
                        using (StreamReader reader = new StreamReader(input, Encoding.UTF8))
                            text = reader.ReadToEnd();
                        oRedisCmd[] cmds = null;
                        try
                        {
                            cmds = JsonConvert.DeserializeObject<oRedisCmd[]>(text);
                            HTTP_API_PROCESS_CMD(context, cmds);
                        }
                        catch (Exception ex)
                        {
                            api___response_json_error("Error convert json input: " + ex.Message, context);
                            return false;
                        }
                    }
                    else
                    {
                        api___response_json_error("Body input is null", context);
                        return false;
                    }
                    break;
                    #endregion
            }
            return false;
        };

        #endregion

        #region [ FREE RESOURCE ]

        static int tcp___get_free_port()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        static HTTPServerUI http_api;
        static WebSocketServer server_ws;
        static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        static ConcurrentDictionary<string, Redis> m_redis
            = new ConcurrentDictionary<string, Redis>() { };
        static void app___free_resouce()
        {
            try
            {
                http_api.Stop();

                m_config.list_cache.ForEach(cf =>
                {
                    try { m_redis[cf.name].Dispose(); } catch (Exception e1) { }
                    try { cf.process.Kill(); } catch (Exception e2) { }
                });
                m_redis.Clear();

                allSockets.Clear();
                server_ws.Dispose();
            }
            catch (Exception err) { }
        }

        #endregion

        static void Main(string[] args)
        {
            #region [ CONFIG.JSON ]

            string file_config = Path.Combine(ROOT_PATH, "config.json");
            if (!File.Exists(file_config))
            {
                Console.WriteLine("Cannot find config.json");
                return;
            }

            try
            {
                string sconfig = File.ReadAllText(file_config);
                m_config = JsonConvert.DeserializeObject<oConfig>(sconfig);
            }
            catch (Exception e1)
            {
                Console.WriteLine("Error format JSON file config.json = ", e1.Message);
                return;
            }

            #endregion

            #region [ REDIS CACHE ]

            if (m_config != null && m_config.list_cache != null)
            {
                string file_conf_template = Path.Combine(ROOT_PATH, "redis.conf");
                if (File.Exists(file_conf_template) == false)
                {
                    Console.WriteLine("Cannot found the file: " + file_conf_template);
                    return;
                }
                string temp_conf = File.ReadAllText(file_conf_template);

                string path_conf = Path.Combine(ROOT_PATH, "config");
                if (Directory.Exists(path_conf) == false) Directory.CreateDirectory(path_conf);

                string PATH_DATA = Path.Combine(ROOT_PATH, "data");
                if (Directory.Exists(PATH_DATA) == false) Directory.CreateDirectory(PATH_DATA);

                m_config.busy = true;
                m_config.list_cache.ForEach(cf =>
                {
                    if (cf.enable)
                    {
                        int port = tcp___get_free_port();
                        string file_redis = Path.Combine(ROOT_PATH, "redis-server.exe");
                        if (File.Exists(file_redis))
                        {
                            string file_conf = Path.Combine(path_conf, cf.name + ".conf");
                            if (File.Exists(file_conf)) File.Delete(file_conf);

                            string conf = temp_conf
                                .Replace("[IP]", "127.0.0.1")
                                .Replace("[PORT]", port.ToString())
                                .Replace("[DATA_FILE]", cf.name)
                                .Replace("[DATA_PATH]", PATH_DATA.Replace('\\', '/'));
                            File.WriteAllText(file_conf, conf);

                            Process p = new Process();
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.RedirectStandardInput = true;
                            p.StartInfo.FileName = file_redis;
                            //string argument = @" """ + file_conf + @""" --port " + port.ToString();
                            string argument = @" """ + file_conf + @"""";
                            p.StartInfo.Arguments = argument;
                            p.Start();

                            cf.process = p;
                            cf.port = port;
                            cf.ready = true;

                            //RedisDataAccessProvider redis = new RedisDataAccessProvider();
                            //redis.Configuration = new TeamDevRedis.LanguageItems.Configuration() { Host = "127.0.0.1", Port = port };
                            //redis.Connect();
                            var redis = new Redis("127.0.0.1", port);

                            if (m_redis.ContainsKey(cf.name))
                                m_redis.TryAdd(cf.name, redis);
                            else
                                m_redis[cf.name] = redis;

                            Console.WriteLine(" -> " + cf.name + " " + cf.port.ToString());
                        }
                    }
                });
                m_config.busy = false;
            }

            #endregion

            #region [ WEB_SOCKET ]

            int port_ws = tcp___get_free_port();
            FleckLog.Level = LogLevel.Error;
            server_ws = new WebSocketServer("ws://127.0.0.1:" + port_ws);
            server_ws.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
            m_config.port_ws = port_ws;

            #endregion

            #region [ TCP_PUSH ]


            #endregion

            #region [ HTTP_API ]

            if (!Directory.Exists(ROOT_PATH_UI)) Directory.CreateDirectory(ROOT_PATH_UI);
            http_api = new HTTPServerUI(ROOT_PATH_UI, m_config.port_api, HTTP_API_PROCESS);

            #endregion

            File.WriteAllText(file_config, JsonConvert.SerializeObject(m_config, Formatting.Indented));

            #region [ READ LINE ]

            string line = Console.ReadLine();
            while (line != "exit")
            {
                //foreach (var socket in allSockets.ToList()) socket.Send(line);
                switch (line)
                {
                    case "cls":
                    case "clean":
                    case "clear":
                        Console.Clear();
                        break;
                }
                line = Console.ReadLine();
            }

            Console.WriteLine("Program is closing... ");
            app___free_resouce();

            //string file_node = Path.Combine(ROOT_PATH, "node.exe");
            //if (File.Exists(file_node))
            //{
            //    Process node = new Process();
            //    node.StartInfo.UseShellExecute = false;
            //    node.StartInfo.RedirectStandardOutput = true;
            //    node.StartInfo.RedirectStandardError = true;
            //    node.StartInfo.RedirectStandardInput = true;
            //    node.StartInfo.FileName = file_node;
            //    string argument = @" --max-old-space-size=4096 app.js";
            //    node.StartInfo.Arguments = argument;
            //    node.Start();
            //}

            #endregion
        }
    }
}
