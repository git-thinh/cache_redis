using ChakraHost.Hosting;
using Fleck;
using Jose;
using log4net;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ckv
{
    public interface IApp
    {

    }

    public class App : IApp
    {
        static readonly string ROOT_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static IApp app = new App();

        #region [ IAPP ]

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
            ILog logger = LogManager.GetLogger(typeof(FleckLog));
            FleckLog.LogAction = (level, message, ex) =>
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        logger.Debug(message, ex);
                        break;
                    case LogLevel.Error:
                        logger.Error(message, ex);
                        break;
                    case LogLevel.Warn:
                        logger.Warn(message, ex);
                        break;
                    default:
                        logger.Info(message, ex);
                        break;
                }
            };

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

        #region [ JS ENGINE ]

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
            var result = JavaScriptContext.RunScript(script, js_currentSourceContext++, string.Empty);
            string v = result.ConvertToString().ToString();
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

        #region [ OWIN HTTP ]

        static IDisposable http_server;
        static void http_init()
        {
            StartOptions startOptions = new StartOptions();
            startOptions.Urls.Add("http://*:12345/");
            Action<IAppBuilder> startup = http_router;
            http_server = WebApp.Start(startOptions, startup);
        }

        static void http_router(IAppBuilder app)
        {
            //app.Map("/", (iab) =>
            //{
            //    iab.Run(context =>
            //    {
            //        context.Response.ContentType = "text/plain";
            //        return context.Response.WriteAsync("OK: " + DateTime.Now.ToString());
            //    });
            //});

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

            app.Map("/token", (iab) =>
            {
                iab.Run(context =>
                {
                    context.Response.ContentType = "text/plain";
                    var payload = new Dictionary<string, object>()
                    {
                        { "sub", "mr.thinh@iot.vn" },
                        { "exp", 1300819380 }
                    };
                    var secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };
                    string token = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
                    return context.Response.WriteAsync(token);
                });
            });
            app.Map("/admin", (iab) =>
            {
                iab.Run(context =>
                {
                    context.Response.ContentType = "text/plain";
                    return context.Response.WriteAsync("This is admin page");
                });
            });

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

        #region [ CACHE MEMORY ]

        const int MEM_SIZE_INDEX = 3000000;
        static ConcurrentDictionary<string, string> mem_store = new ConcurrentDictionary<string, string>();
        static string[] mem_ascii = new string[MEM_SIZE_INDEX];
        static string[] mem_utf8 = new string[MEM_SIZE_INDEX];

        static string mem_cache_db()
        {
            string[] fs = Directory.GetFiles(ROOT_PATH, "*.sql");
            if (fs.Length == 0) return "ERR: Cannot find *.sql";
            
            string[] a = File.ReadAllLines(fs[0]);
            if (a.Length < 2) return "ERR: Cannot find connect string and SQL command at file " + fs[0];

            string con_str = a[0].Trim(),
                query = string.Join(Environment.NewLine, a.Where((x, i) => i > 0).ToArray());

            if (con_str.StartsWith("--")) con_str = con_str.Substring(2);

            try
            {
                using (SqlConnection conn = new SqlConnection(con_str))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(query, conn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var columns = new List<string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                            columns.Add(reader.GetName(i));

                        while (reader.Read())
                        {
                            var dic = new Dictionary<string, object>();

                            for (var i = 0; i < reader.FieldCount; i++)
                                dic.Add(columns[i], reader.GetValue(i));

                            string json = JsonConvert.SerializeObject(dic, Formatting.Indented);
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
                        }



                        //while (reader.Read())
                        //{ 
                        //    string json = subfix + JsonConvert.SerializeObject(reader.GetValue(0).ToString(), Formatting.Indented);
                        //    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
                        //    stream.Write(buffer, 0, buffer.Length); //sends bytes to server
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Format("#ERR: {0}", ex.Message);
            }

            return "OK";
        }

        #endregion

        #region [ STATIC MAIN ]

        static System.Threading.Timer timer;

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

        static void db_sync()
        {
            Console.WriteLine(DateTime.Now.ToString());
        }

        static void start()
        {
            //redis_init();

            ws_init();
            js_init();
            http_init();

            timer = new System.Threading.Timer(e => db_sync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
        }

        static void stop()
        {
            http_stop();
            js_stop();
            ws_stop();
            timer.Dispose();
            process_redis.Kill();
            process_redis.Dispose();
        }

        static void Main(string[] args)
        {
            start();

            var input = Console.ReadLine();
            while (input != "exit")
            {
                ws_broadcast(input);
                input = Console.ReadLine();
            }

            //Console.WriteLine("[ Enter ] to exit ...");
            //Console.ReadLine();

            stop();
        }

        #endregion
    }
}
