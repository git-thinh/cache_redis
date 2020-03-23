﻿using Fleck2;
using Fleck2.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
                api___response_stream("*.json", input, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.OutputStream.Close();
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

                    if (filename == "list.html" || filename == "list")
                    {
                        dirs = Directory.GetDirectories(ROOT_PATH_UI);
                        files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);

                        files = files.Select(x => string.Format(@"<a href=""{0}"">{0}</a></br>", Path.GetFileName(x))).ToArray();
                        dirs = dirs.Select(x => string.Format(@"<a href=""{0}"">{0}</a></br>", Path.GetFileName(x))).ToArray();

                        string s = string.Join(string.Empty, dirs) + string.Join(string.Empty, files);
                        input = api___stream_string(s);
                        filename = "list.html";
                    }
                    else
                    {
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

                                string s = string.Join(string.Empty, dirs) + string.Join(string.Empty, files);
                                input = api___stream_string(s);
                                filename = "*.html";
                            }
                            else
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                context.Response.OutputStream.Close();
                                return false;
                            }
                        }
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

        static void Main(string[] args)
        {
            #region [ READ CONFIG.JSON ]

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

            if (!Directory.Exists(ROOT_PATH_UI)) Directory.CreateDirectory(ROOT_PATH_UI);
            HTTPServerUI http_api = new HTTPServerUI(ROOT_PATH_UI, m_config.port_api, HTTP_API_PROCESS);



            Console.WriteLine("Enter to exit ...");
            Console.ReadKey();

            // Exit program
            http_api.Stop();










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

            //string file_redis = Path.Combine(ROOT_PATH, "redis-server.exe");
            //if (File.Exists(file_redis))
            //{
            //    Process redis = new Process();
            //    redis.StartInfo.UseShellExecute = false;
            //    redis.StartInfo.RedirectStandardOutput = true;
            //    redis.StartInfo.RedirectStandardError = true;
            //    redis.StartInfo.RedirectStandardInput = true;
            //    redis.StartInfo.FileName = file_redis;
            //    string argument = @" redis.windows.conf --port 5500";
            //    redis.StartInfo.Arguments = argument;
            //    redis.Start();
            //}

            //////Console.ReadLine();

            //////FleckLog.Level = LogLevel.Debug;
            //////var allSockets = new List<IWebSocketConnection>();
            //////var server = new WebSocketServer("ws://localhost:8181");
            //////server.Start(socket =>
            //////{
            //////    socket.OnOpen = () =>
            //////    {
            //////        Console.WriteLine("Open!");
            //////        allSockets.Add(socket);
            //////    };
            //////    socket.OnClose = () =>
            //////    {
            //////        Console.WriteLine("Close!");
            //////        allSockets.Remove(socket);
            //////    };
            //////    socket.OnMessage = message =>
            //////    {
            //////        Console.WriteLine(message);
            //////        allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
            //////    };
            //////});

            //////Process.Start("client.html");

            //////var input = Console.ReadLine();
            //////while (input != "exit")
            //////{
            //////    foreach (var socket in allSockets.ToList())
            //////    {
            //////        socket.Send(input);
            //////    }
            //////    input = Console.ReadLine();
            //////}
            ///

        }
    }
}
