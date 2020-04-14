using ChakraHost.Hosting;
using Fleck;
using Microsoft.ClearScript.V8;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using SeasideResearch.LibCurlNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ckv
{
    public interface IApp
    {

    }

    public class App : IApp
    {
        static App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(App).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
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

        static oConfig CF;
        static string MSG_ERR = string.Empty;
        static readonly string ROOT_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static IApp app = new App();
        static ConcurrentDictionary<string, object> FN = new ConcurrentDictionary<string, object>();
        public static string getJson(object o) => JsonConvert.SerializeObject(o, Formatting.Indented);
        static void fn_init()
        {
            FN.TryAdd("config", new Func<object, string>((o) => { return JsonConvert.SerializeObject(CF); }));
            FN.TryAdd("pipe", new Func<object, string>((o) =>
            {
                var client = new NamedPipeClientStream("ckv." + CF.name);
                client.Connect();
                string p = new StreamReader(client).ReadToEnd().Trim();
                return JsonConvert.SerializeObject(new { port = p });
            }));

            FN.TryAdd("file_reset", new Func<object, string>((o) => { file_load(); return JsonConvert.SerializeObject(file_data.Keys); }));

            FN.TryAdd("clear", new Func<object, string>((o) => { mem_cache_clear(); return "OK"; }));
            FN.TryAdd("load_db", new Func<object, string>(mem_cache_db));

            FN.TryAdd("keys", new Func<object, string>((o) => { return JsonConvert.SerializeObject(mem_store.Keys); }));
            FN.TryAdd("item", new Func<object, string>(mem_cache_item));
            FN.TryAdd("all", new Func<object, string>(mem_cache_all));

            FN.TryAdd("remove", new Func<object, string>(mem_cache_remove));
            FN.TryAdd("update", new Func<object, string>(mem_cache_update));
            FN.TryAdd("addnew", new Func<object, string>(mem_cache_addnew));

            FN.TryAdd("url_get_raw", new Func<object, string>(curl_get_raw));
            FN.TryAdd("url_get_raw_by_js", new Func<object, string>(curl_get_raw_by_js));
            FN.TryAdd("url_get_text", new Func<object, string>(curl_get_text));

            FN.TryAdd("v8_get_raw", new Func<object, string>(v8_get_raw));


            FN.TryAdd("api", new Func<object, string>((o) => JsonConvert.SerializeObject(FN.Keys)));
        }


        #region [ IAPP ]

        #endregion

        #region [ CURL ]

        static string curl_get_text(object p)
        {
            string html = curl_get_raw(p);
            string s = html;

            //s = new Regex(@"<script[^>]*>[\s\S]*?</script>").Replace(s, string.Empty);
            //s = new Regex(@"<style[^>]*>[\s\S]*?</style>").Replace(s, string.Empty);
            //s = new Regex(@"<noscript[^>]*>[\s\S]*?</noscript>").Replace(s, string.Empty);
            //s = Regex.Replace(s, @"<meta(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<link(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<use(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<figure(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<!DOCTYPE(.|\n)*?>", string.Empty, RegexOptions.Singleline);
            //s = Regex.Replace(s, @"<!--(.|\n)*?-->", string.Empty, RegexOptions.Singleline);

            string title = Regex.Match(html, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value.Trim();
            s = Regex.Replace(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", "[TITLE] " + title, RegexOptions.Singleline);

            s = Regex.Replace(s, @"<a(.|\n)*?>", string.Empty, RegexOptions.Singleline);

            //### Remove any tags but not there content "<p>bob<span> johnson</span></p>" -> "bob johnson"
            // Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
            s = Regex.Replace(s, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", string.Empty, RegexOptions.Singleline);

            //### Remove multi break lines
            //s = Regex.Replace(s, @"[\r\n]+", "<br />");
            //s = Regex.Replace(s, @"[\r\n]{2,}", "<br />");
            s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");

            return s.Trim();
        }

        static bool curl_inited = false;
        static string curl_get_raw(object p)
        {
            if (p == null) return string.Empty;

            string url = p.ToString();
            try
            {
                if (curl_inited == false)
                {
                    Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);
                    curl_inited = true;
                }

                Easy easy = new Easy();

                StringBuilder bi = new StringBuilder();
                Easy.WriteFunction wf = new Easy.WriteFunction((buf, size, nmemb, extraData) =>
                {
                    string si = Encoding.UTF8.GetString(buf);
                    bi.Append(si);
                    return size * nmemb;
                });
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                Easy.SSLContextFunction sf = new Easy.SSLContextFunction((ctx, extraData) => CURLcode.CURLE_OK);
                easy.SetOpt(CURLoption.CURLOPT_SSL_CTX_FUNCTION, sf);

                easy.SetOpt(CURLoption.CURLOPT_URL, url);
                easy.SetOpt(CURLoption.CURLOPT_CAINFO, "ca-bundle.crt");

                easy.Perform();
                easy.Dispose();

                //Curl.GlobalCleanup();

                string s = bi.ToString();

                //string title = Regex.Match(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                s = new Regex(@"<script[^>]*>[\s\S]*?</script>").Replace(s, string.Empty);
                s = new Regex(@"<style[^>]*>[\s\S]*?</style>").Replace(s, string.Empty);
                s = new Regex(@"<noscript[^>]*>[\s\S]*?</noscript>").Replace(s, string.Empty);
                s = Regex.Replace(s, @"<meta(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<link(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<use(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<figure(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<!DOCTYPE(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                s = Regex.Replace(s, @"<!--(.|\n)*?-->", string.Empty, RegexOptions.Singleline);

                s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");

                return s;
            }
            catch (Exception ex)
            {
                return "ERR: " + ex.Message;
            }
        }

        static string curl_get_raw_by_js(object p)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.ToString())) return string.Empty;
            string url = p.ToString();

            string js = @"
var request = new XMLHttpRequest();
request.open('GET', '" + url + @"', false);
request.send(null);

if (request.status === 200) {
    //console.log(request.responseText);
    return request.responseText;
}else return '#ERR';

";
            string s = js_run(js);
            return s;
        }

        static void curl_stop()
        {
            Curl.GlobalCleanup();
        }

        #endregion

        #region [ WS ]

        static List<IWebSocketConnection> ws_sockets;
        static WebSocketServer ws_server;

        static void ws_broadcast(string input)
        {
            foreach (var socket in ws_sockets.ToList())
                socket.Send(input);
        }

        static void ws_stop()
        {
            ws_sockets.Clear();
            ws_server.Dispose();
        }

        static void ws_init()
        {
            ws_sockets = new List<IWebSocketConnection>();

            //var ws_server = new WebSocketServer("wss://0.0.0.0:8431");
            //ws_server.Certificate = new X509Certificate2("MyCert.pfx");
            ws_server = new WebSocketServer("ws://0.0.0.0:8181");
            ws_server.RestartAfterListenError = true; // Auto Restart After Listen Error
            ws_server.ListenerSocket.NoDelay = true; // Disable Nagle's Algorithm
            ws_server.SupportedSubProtocols = new[] { "superchat", "chat" };

            FleckLog.Level = LogLevel.Debug; 
            ws_server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open...");
                    ws_sockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close...");
                    ws_sockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    ws_sockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
        }

        #endregion

        #region [ JS engine: V8 ]

        static string v8_get_raw(object p)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.ToString())) return string.Empty;
            string url = p.ToString();

            V8ScriptEngine engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, 9393);

            //engine.Execute(Script_Text);
            //engine.AddCOMType("XMLHttpRequest", "MSXML2.XMLHTTP");
            //object returnedVal = _v8Engine.Script.Execute();

            engine.AddCOMType("XMLHttpRequest", "MSXML2.XMLHTTP");
            engine.Execute(@"
    function get(url) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', url, false);
        xhr.send();

        if (xhr.status == 200) return xhr.responseText;
        else return 'ERR: Request failed: ' + xhr.status;
    }
");

            string v = engine.Script.get(url);
            return v;
        }

        #endregion

        #region [ JS engine: Chakra ]

        static JavaScriptRuntime js_runtime;
        static JavaScriptContext js_context;
        static JavaScriptSourceContext js_currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        static void js_init()
        {
            Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out js_runtime);
            Native.JsCreateContext(js_runtime, out js_context);
            Native.JsSetCurrentContext(js_context);
        }
        static void js_stop()
        {
            Native.JsSetCurrentContext(JavaScriptContext.Invalid);
            Native.JsDisposeRuntime(js_runtime);
        }
        static string js_run(string body_function)
        {
            //string script = "(()=>{return \'Hello world!\';})()";
            string script = "(()=>{ try{ " + body_function + " }catch(e){ return 'ERR:'+e.message; } })()";
            //var result = JavaScriptContext.RunScript(script, js_currentSourceContext++, string.Empty);
            //string v = result.ConvertToString().ToString();

            JavaScriptRuntime runtime;
            JavaScriptContext context;
            JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
            JavaScriptValue result;

            // Create a runtime. 
            Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime);

            // Create an execution context. 
            Native.JsCreateContext(runtime, out context);

            // Now set the execution context as being the current one on this thread.
            Native.JsSetCurrentContext(context);

            //Native.js ("ChakraBridge");

            // Run the script.
            Native.JsRunScript(script, currentSourceContext++, "", out result);

            // Convert your script result to String in JavaScript; redundant if your script returns a String
            JavaScriptValue resultJSString;
            Native.JsConvertValueToString(result, out resultJSString);

            // Project script result in JS back to C#.
            IntPtr resultPtr;
            UIntPtr stringLength;
            Native.JsStringToPointer(resultJSString, out resultPtr, out stringLength);

            string v = Marshal.PtrToStringUni(resultPtr);

            // Dispose runtime
            Native.JsSetCurrentContext(JavaScriptContext.Invalid);
            Native.JsDisposeRuntime(runtime);

            //JavaScriptValue result;
            //Native.JsRunScript(script, js_currentSourceContext++, "", out result);
            //string v = result.ConvertToString().ToString();
            return v;
        }

        #endregion

        #region [ REDIS ]

        static Process process_redis;

        static void redis_init()
        {
            int port_redis_job = 0;
            string name = "JOB";

            string file_redis = Path.Combine(ROOT_PATH, "redis-server.exe");
            if (File.Exists(file_redis) == false)
            {
                Console.WriteLine("Cannot found the file: redis-server.exe");
                Console.ReadLine();
                return;
            }

            string file_conf_template = Path.Combine(ROOT_PATH, "redis.conf");
            if (File.Exists(file_conf_template) == false)
            {
                Console.WriteLine("Cannot found the file: " + file_conf_template);
                Console.ReadLine();
                return;
            }
            string temp_conf = File.ReadAllText(file_conf_template);

            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            port_redis_job = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            string file_conf = Path.Combine(ROOT_PATH, name + ".conf");
            if (File.Exists(file_conf)) File.Delete(file_conf);

            string conf = temp_conf
                .Replace("[IP]", "127.0.0.1")
                .Replace("[PORT]", port_redis_job.ToString())
                .Replace("[DATA_FILE]", name)
                .Replace("[DATA_PATH]", "./");
            File.WriteAllText(file_conf, conf);

            process_redis = new Process();
            process_redis.StartInfo.UseShellExecute = false;
            process_redis.StartInfo.RedirectStandardOutput = true;
            process_redis.StartInfo.RedirectStandardError = true;
            process_redis.StartInfo.RedirectStandardInput = true;
            process_redis.StartInfo.FileName = file_redis;
            //string argument = @" """ + file_conf + @""" --port " + port.ToString();
            string argument = @" """ + file_conf + @"""";
            process_redis.StartInfo.Arguments = argument;
            process_redis.Start();
        }

        #endregion

        #region [ OWIN ]

        static IDisposable http_server;
        static void http_init()
        {
            StartOptions startOptions = new StartOptions();
            startOptions.Urls.Add("http://*:" + CF.port.ToString() + "/");
            Action<IAppBuilder> startup = http_router;
            http_server = WebApp.Start(startOptions, startup);
        }

        static void http_router(IAppBuilder app)
        {
            foreach (string router in FN.Keys)
                app.Map("/" + router, (iab) =>
                 {
                     iab.Run(context =>
                     {
                         string json = string.Empty;
                         try
                         {
                             bool ok = true;
                             object pr = null;
                             if (context.Request.Method == "POST")
                             {
                                 string sbody = new StreamReader(context.Request.Body).ReadToEnd();
                                 if (string.IsNullOrWhiteSpace(sbody))
                                 {
                                     json = JsonConvert.SerializeObject(new { ok = false, message = "Body of POST is not null or emtpy" });
                                     ok = false;
                                 }
                                 else pr = sbody;
                             }
                             if (ok)
                             {
                                 var fn = (Func<object, string>)FN[router];
                                 string url = context.Request.Uri.ToString();
                                 int pos = url.IndexOf('?');

                                 if (context.Request.Method == "GET" && pos != -1)
                                     pr = Uri.UnescapeDataString(url.Substring(pos + 1));

                                 json = fn(pr);
                             }
                         }
                         catch (Exception e)
                         {
                             json = JsonConvert.SerializeObject(new { ok = false, message = e.Message });
                         }
                         context.Response.ContentType = "application/json";
                         return context.Response.WriteAsync(json);
                     });
                 });

            #region [ UI ]

            //app.Map("/index.html", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/w2ui.min.css", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/w2ui.min.js", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/style.css", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            //app.Map("/index.js", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            #endregion

            #region [ /token, /admin ]

            //app.Map("/token", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        var payload = new Dictionary<string, object>()
            //        {
            //            { "sub", "mr.thinh@iot.vn" },
            //            { "exp", 1300819380 }
            //        };
            //        var secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };
            //        string token = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
            //        return context.Response.WriteAsync(token);
            //    });
            //});
            //app.Map("/admin", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("This is admin page");
            //    });
            //});

            #endregion

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("OK: " + DateTime.Now.ToString());
            });

            //app.UseFileServer(true);
            //var options = new FileServerOptions
            //{
            //    EnableDirectoryBrowsing = true,
            //    EnableDefaultFiles = true,
            //    DefaultFilesOptions = { DefaultFileNames = { "index.html" } },
            //    FileSystem = new PhysicalFileSystem("ui"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //};
            //app.UseFileServer(options);
            //app.UseFileServer(new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //    RequestPath = new PathString("/valid_add"),
            //    FileSystem = new PhysicalFileSystem("valid_add"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //});
            //app.UseFileServer(new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //    RequestPath = new PathString("/schema"),
            //    FileSystem = new PhysicalFileSystem("schema"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //});
            //app.UseFileServer(new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //    RequestPath = new PathString("/sql"),
            //    FileSystem = new PhysicalFileSystem(@"config\sql"),
            //    StaticFileOptions = { ContentTypeProvider = new CustomContentTypeProvider() }
            //});
        }

        static void http_stop()
        {
            http_server.Dispose();
        }

        #endregion

        #region [ UTF8 -> ASCCI ]

        static string[] arr1 = new string[] {
            "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ"
        };
        static string[] arr2 = new string[] {
            "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y"
        };

        static string convert_unicode_2_ascii(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            for (int i = 0; i < arr1.Length; i++)
                str = str.Replace(arr1[i], arr2[i]);

            str = str.Replace("ẻ", "e").Replace("ó", "o").Replace("à", "o");

            return str;
        }

        #endregion

        #region [ FILE: JSON, SQL, JS, VALID ]

        static ConcurrentDictionary<string, string> file_data = new ConcurrentDictionary<string, string>();
        static string file_load()
        {
            var fs = Directory.GetFiles(ROOT_PATH, "*.json").Select(x => Path.GetFileName(x)).ToList();
            fs.AddRange(Directory.GetFiles(ROOT_PATH, "*.sql").Select(x => Path.GetFileName(x)));
            fs.AddRange(Directory.GetFiles(ROOT_PATH, "*.js").Select(x => Path.GetFileName(x)));
            fs.AddRange(Directory.GetFiles(ROOT_PATH, "*.valid").Select(x => Path.GetFileName(x)));

            file_data.Clear();
            foreach (var f in fs)
                file_data.TryAdd(f.ToLower(), File.ReadAllText(f));

            return JsonConvert.SerializeObject(new { ok = true, files = file_data.Keys });
        }

        #endregion

        #region [ CACHE MEMORY ]

        const int MEM_SIZE_INDEX = 3000000;
        static ConcurrentDictionary<long, string> mem_store = new ConcurrentDictionary<long, string>();
        static string[] mem_ascii = new string[MEM_SIZE_INDEX];
        static string[] mem_utf8 = new string[MEM_SIZE_INDEX];
        static long ID_INCREMENT = 0;

        static void mem_cache_clear(object pr = null)
        {
            mem_store.Clear();
            mem_ascii = new string[MEM_SIZE_INDEX];
            mem_utf8 = new string[MEM_SIZE_INDEX];
        }

        static string mem_cache_db(object pr = null)
        {
            string file = "_" + CF.name + ".select.sql";
            if (string.IsNullOrEmpty(CF.name) || File.Exists(file) == false)
                return JsonConvert.SerializeObject(new { ok = true, message = "Cannot found file: _" + CF.name + ".select.sql" });

            string[] a = File.ReadAllLines(file);
            if (a.Length < 2) return JsonConvert.SerializeObject(new { ok = true, message = "ERR: Cannot find connect string and SQL command at file " + file });

            string con_str = a[0].Trim(),
                query = string.Join(Environment.NewLine, a.Where((x, i) => i > 0).ToArray());

            if (con_str.StartsWith("--")) con_str = con_str.Substring(2);

            using (SqlConnection conn = new SqlConnection(con_str))
            {
                var cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var columns = new List<string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                            columns.Add(reader.GetName(i));

                        mem_cache_clear();

                        while (reader.Read())
                        {
                            var dic = new Dictionary<string, object>();

                            for (var i = 0; i < reader.FieldCount; i++)
                                dic.Add(columns[i], reader.GetValue(i));

                            string json = JsonConvert.SerializeObject(dic, Formatting.Indented);
                            if (dic.ContainsKey("id"))
                            {
                                long id;
                                if (long.TryParse(dic["id"].ToString(), out id))
                                    mem_store.TryAdd(id, json);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { ok = false, message = string.Format("#ERR: {0}", ex.Message) });
                }
            }

            return JsonConvert.SerializeObject(new { ok = true, total = mem_store.Count });
        }

        static string mem_cache_all(object pr = null)
        {
            StringBuilder bi = new StringBuilder("[");
            long[] ids = mem_store.Keys.ToArray();
            for (int i = 0; i < ids.Length; i++)
            {
                if (mem_store.ContainsKey(ids[i]))
                {
                    bi.Append(i == 0 ? string.Empty : ",");
                    bi.Append(mem_store[ids[i]]);
                }
            }
            bi.Append("]");
            return bi.ToString();
        }

        static string mem_cache_item(object pr = null)
        {
            if (pr == null) return JsonConvert.SerializeObject(new { ok = false, message = "Method of request is POST and Body is not null or empty" });

            long id;
            if (long.TryParse(pr.ToString(), out id) == false)
                return JsonConvert.SerializeObject(new { ok = false, message = "Body must be number" });

            if (mem_store.ContainsKey(id) == false)
                return JsonConvert.SerializeObject(new { ok = false, message = "Cannot found item has ID = " + id.ToString() });

            return mem_store[id];
        }

        static string mem_cache_remove(object pr = null)
        {
            if (pr == null)
                return JsonConvert.SerializeObject(new { ok = false, message = "Method of request is POST and Body is not null or empty" });

            Dictionary<string, object> dic = null;
            try
            {
                dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(pr.ToString());
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { ok = false, message = e.Message });
            }

            if (dic.ContainsKey("id"))
                return JsonConvert.SerializeObject(new { ok = false, message = "Body must be number" });

            long id;
            if (long.TryParse(pr.ToString(), out id) == false)
                return JsonConvert.SerializeObject(new { ok = false, message = "Body must be number" });

            string key = id.ToString() + " ";
            for (int i = 0; i < mem_store.Count; i++)
            {
                if (!string.IsNullOrEmpty(mem_ascii[i])
                    && mem_ascii[i].StartsWith(key))
                {
                    mem_ascii[i] = null;
                    mem_utf8[i] = null;
                    break;
                }
            }

            mem_store.TryRemove(id, out string v);

            return JsonConvert.SerializeObject(new { ok = true });
        }

        static string mem_cache_update(object pr = null)
        {
            bool update = false;
            long id;

            #region [ VALID ]

            if (pr == null)
                return JsonConvert.SerializeObject(new { ok = false, message = "Method of request is POST and Body is not null or empty" });

            Dictionary<string, object> dic = null;
            try
            {
                dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(pr.ToString());
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { ok = false, message = e.Message });
            }

            if (dic.ContainsKey("id"))
            {
                if (long.TryParse(dic["id"].ToString(), out id) == false)
                    return JsonConvert.SerializeObject(new { ok = false, message = "Body must be number" });

                if (mem_store.ContainsKey(id) == false)
                    return JsonConvert.SerializeObject(new { ok = false, message = "Cannot found item has ID " + id.ToString() + " to update it." });
                update = true;
            }
            else
            {
                Interlocked.Increment(ref ID_INCREMENT);
                id = long.Parse(DateTime.Now.ToString("yyMMddHHmmss")) + ID_INCREMENT;
            }

            #endregion

            if (update)
            {
                //UPDATE
                string key = id.ToString() + " ";
                for (int i = 0; i < mem_store.Count; i++)
                {
                    if (!string.IsNullOrEmpty(mem_ascii[i])
                        && mem_ascii[i].StartsWith(key))
                    {
                        mem_ascii[i] = null;
                        mem_utf8[i] = null;
                        break;
                    }
                }
            }
            else
            {
                //ADD_NEW

            }

            return JsonConvert.SerializeObject(new { ok = true, id = id, action = update ? "UPDATE" : "ADDNEW" });
        }

        static object mem_cache___bind_value(object v)
        {
            switch (v)
            {
                case "yyMMdd":
                case "yyyyMMdd":
                case "HHmmss":
                case "yyMMddhhmmss":
                    v = int.Parse(DateTime.Now.ToString(v.ToString()));
                    break;
                case "yyyyMMddhhmmss":
                    v = long.Parse(DateTime.Now.ToString(v.ToString()));
                    break;
                case "-1|yyMMdd":
                case "-1|yyyyMMdd":
                case "-1|HHmmss":
                case "-1|yyyyMMddhhmmss":
                case "-1|yyMMddhhmmss":
                    v = -1;
                    break;
                case "UUID":
                    v = Guid.NewGuid().ToString();
                    break;
                case "UUID_WORD":
                    v = Guid.NewGuid().ToString().Replace('-', ' ');
                    break;
                case "UUID_URL":
                    v = "http://" + Guid.NewGuid().ToString();
                    break;
            }

            return v;
        }

        static string mem_cache_addnew(object pr = null)
        {
            #region [ PARA ]

            if (pr == null)
                return JsonConvert.SerializeObject(new { ok = false, message = "Method of request is POST and Body is not null or empty" });

            string file = CF.name + ".addnew.json";
            if (!file_data.ContainsKey(file))
                return JsonConvert.SerializeObject(new { ok = false, message = "Cannot found file: " + file });

            Dictionary<string, object> shema;
            try
            {
                shema = JsonConvert.DeserializeObject<Dictionary<string, object>>(file_data[file]);
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { ok = false, message = "Convert json file " + file + " . " + e.Message });
            }

            Dictionary<string, object> dic = null;
            try
            {
                dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(pr.ToString());
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { ok = false, message = "Convert json paramenter: " + e.Message });
            }
            Interlocked.Increment(ref ID_INCREMENT);
            long id = long.Parse(DateTime.Now.ToString("yyMMddHHmmssfff")) + ID_INCREMENT;
            if (dic.ContainsKey("id")) dic["id"] = id; else dic.Add("id", id);

            string[] keys = shema.Keys.ToArray();
            foreach (var key in keys)
            {
                shema[key] = mem_cache___bind_value(shema[key]);
                try
                {
                    if (dic.ContainsKey(key))
                        shema[key] = dic[key];
                }
                catch (Exception e)
                {
                    return JsonConvert.SerializeObject(new { ok = false, message = "Set value from para into data shema: " + file + " . " + e.Message });
                }
            }

            List<string> ascii = new List<string>() { id.ToString() };
            List<string> utf8 = new List<string>();
            foreach (var key in keys)
            {
                object v = shema[key];
                if (v != null && key != "id")
                {
                    if (v.GetType().Name.Contains("String"))
                    {
                        if (!string.IsNullOrWhiteSpace(v.ToString()))
                        {
                            if (new Regex(@"^-?[0-9][0-9,\.]+$").IsMatch(v.ToString()))
                                ascii.Add(v.ToString());
                            else utf8.Add(v.ToString());
                        }
                    }
                    else ascii.Add(v.ToString());
                }
            }

            string s1 = string.Join(" ", ascii),
                s2 = string.Join(" ", utf8).ToLower();
            s1 = s1 + " " + convert_unicode_2_ascii(s2);

            string j = JsonConvert.SerializeObject(shema);
            var para = JsonConvert.DeserializeObject<Dictionary<string, object>>(j);
            string[] key2 = dic.Keys.Where(x => keys.FirstOrDefault(o => o == x) == null).ToArray();
            foreach (string key in key2) para.Add(key, dic[key]);

            #endregion

            string json = "";

            #region [ SQL EXECUTE ]

            string file_sql = CF.name + ".addnew.sql";
            if (!file_data.ContainsKey(file_sql))
                return JsonConvert.SerializeObject(new { ok = false, message = "Cannot found file: " + file_sql });

            string[] a = file_data[file_sql].Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string con_str = a[0].Trim(),
                query = string.Join(Environment.NewLine, a.Where((x, i) => i > 0).ToArray());

            if (con_str.StartsWith("--")) con_str = con_str.Substring(2);

            using (SqlConnection conn = new SqlConnection(con_str))
            {
                try
                {
                    conn.Open();

                    var cmd = new SqlCommand(query, conn);
                    foreach (var kv in para)
                        cmd.Parameters.AddWithValue("@" + kv.Key, kv.Value);

                    var reader = cmd.ExecuteReader();
                    var dic_ckv = new Dictionary<string, List<long>>();
                    long id_;
                    string api_;

                    while (reader.Read())
                    {
                        id_ = reader.GetInt64(0);
                        api_ = reader.GetString(1).ToLower().Trim();
                        if (dic_ckv.ContainsKey(api_)) dic_ckv[api_].Add(id_);
                        else dic_ckv.Add(api_, new List<long>() { id_ });
                    }
                    reader.Close();

                    if (dic_ckv.Count > 0)
                    {
                        foreach (var kv_ in dic_ckv)
                        {
                            api_ = kv_.Key;
                            long[] ids_ = kv_.Value.ToArray();
                            bool it_self = api_ == CF.name;
                            string file_ = (it_self ? "_" : "../" + api_ + "/_") + api_ + ".select.sql";

                            if (it_self) query = file_data[file_];
                            else
                            {
                                if (File.Exists(file_) == false)
                                    return JsonConvert.SerializeObject(new { ok = false, message = "#ERR_SET_CACHE: Cannot found file: " + file_ });
                                query = File.ReadAllText(file_);
                            }

                            query = string.Join(Environment.NewLine,
                                query.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                                .Where((x, i) => i > 0));

                            if (ids_.Length == 1)
                                query = query.Replace("--<1>", string.Empty).Replace("<ID>", id.ToString());
                            else if (ids_.Length > 1)
                            {
                                string strw = " ( m___.id = " + string.Join(" or m___.id = ", ids_) + " ) ";
                                query = query.Replace("--<2>", string.Empty).Replace("<IDS>", strw);
                            }

                            cmd = new SqlCommand(query, conn);
                            reader = cmd.ExecuteReader();

                            var columns = new List<string>();
                            for (var i = 0; i < reader.FieldCount; i++)
                                columns.Add(reader.GetName(i));

                            var ls_jsons = new List<string>();
                            while (reader.Read())
                            {
                                var dt_ = new Dictionary<string, object>();

                                for (var i = 0; i < reader.FieldCount; i++)
                                    dt_.Add(columns[i], reader.GetValue(i));

                                string it = JsonConvert.SerializeObject(dt_, Formatting.Indented);
                                if (dt_.ContainsKey("id"))
                                {
                                    long idc;
                                    if (long.TryParse(dt_["id"].ToString(), out idc))
                                    {
                                        mem_store.TryAdd(idc, it);
                                        ls_jsons.Add(it);
                                    }
                                }
                            }
                            reader.Close();

                            if (it_self)
                                json = @"{""ok"": true, ""data"": [" + string.Join(",", ls_jsons.ToArray()) + "]}";
                            else
                            {
                                // POST URL to set cache
                            }
                        } //end foreach set caches
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { ok = false, message = string.Format("#ERR_EXE: {0}", ex.Message) });
                }
            }

            #endregion

            //shema.Add("#ascii", s1);
            //shema.Add("#utf8", s2);
            //return JsonConvert.SerializeObject(new { ok = true, id = id, item = shema, para = para });
            return json;
        }

        #endregion

        #region [ STATIC MAIN ]

        static System.Threading.Timer timer;

        static void cf_load()
        {
            oConfig o = null;

            try
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string cf = Path.Combine(dir, "config.json");
                bool ok = true;
                if (File.Exists(cf))
                {
                    try
                    {
                        o = JsonConvert.DeserializeObject<oConfig>(File.ReadAllText(cf));
                        if (!string.IsNullOrEmpty(o.name)) o.name = o.name.ToLower();
                        if (o.port == 0)
                        {
                            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
                            l.Start();
                            o.port = ((IPEndPoint)l.LocalEndpoint).Port;
                            l.Stop();
                        }
                    }
                    catch { ok = false; }
                }

                if (!ok)
                {
                    o = new oConfig();

                    string name = Path.GetFileName(dir);
                    string file = Path.Combine(dir, "_" + name + ".select.sql");
                    if (File.Exists(file) == false)
                    {
                        name = Directory.GetFiles(dir, "*.sql").Select(x => Path.GetFileName(x)).Where(x => x[0] == '_').Take(1).SingleOrDefault();
                        if (!string.IsNullOrEmpty(name)) name = name.Substring(1).Split('.')[0];
                        name = name.ToLower();
                    }
                    o.name = name;
                    TcpListener l = new TcpListener(IPAddress.Loopback, 0);
                    l.Start();
                    o.port = ((IPEndPoint)l.LocalEndpoint).Port;
                    l.Stop();
                    File.WriteAllText(cf, JsonConvert.SerializeObject(o, Formatting.Indented));
                }
            }
            catch { }

            CF = o;
        }

        static void db_sync()
        {
            Console.WriteLine(DateTime.Now.ToString());
        }

        static void start()
        {
            fn_init();
            //redis_init();

            ws_init();
            js_init();
            http_init();

            timer = new System.Threading.Timer(e => db_sync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
        }

        static void stop()
        {
            curl_stop();
            http_stop();
            js_stop();
            ws_stop();

            if (timer != null) timer.Dispose();
            if (process_redis != null)
            {
                process_redis.Kill();
                process_redis.Dispose();
            }
        }

        static NamedPipeServerStream pipe_server;
        static Thread pipe_thread;
        static bool pipe_stop = false;
        static void Main(string[] args)
        {
            cf_load();
            //if (string.IsNullOrEmpty(CF.name) || File.Exists("_" + CF.name + ".select.sql") == false)
            //{
            //    MSG_ERR = "Cannot found file: _" + CF.name + ".select.sql";
            //    Console.WriteLine(MSG_ERR);
            //    Console.WriteLine("[ Enter ] to exit ...");
            //    Console.ReadLine();
            //    return;
            //}
            //file_load();

            pipe_thread = new Thread(new ParameterizedThreadStart((p_) =>
            {
                while (pipe_stop)
                {
                    pipe_server = new NamedPipeServerStream("ckv." + CF.name);
                    pipe_server.WaitForConnection();
                    var writer = new StreamWriter(pipe_server);
                    writer.WriteLine(p_.ToString());
                    writer.Flush();
                    pipe_server.Close();
                    pipe_server.Dispose();
                    Thread.Sleep(100);
                }
            }));
            pipe_thread.Start(CF.port);

            //var reader = new StreamReader(server);
            //var writer = new StreamWriter(server);

            //var received = reader.ReadLine();
            //Console.WriteLine("Received from client: " + received);

            //var toSend = "Hello, client.";
            //writer.WriteLine(toSend);
            //writer.Flush();


            mem_cache_db(null);

            start();

            var input = Console.ReadLine();
            while (input != "exit")
            {
                ws_broadcast(input);
                input = Console.ReadLine();
            }

            //Console.WriteLine("[ Enter ] to exit ...");
            //Console.ReadLine();

            Console.WriteLine("Program to closing ...");
            pipe_stop = true;
            if (pipe_server != null) pipe_server.Close();
            if (pipe_thread != null) pipe_thread.Abort();

            stop();
        }

        #endregion
    }
}
