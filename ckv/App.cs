using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace ckv
{
    class App
    {
        static readonly string ROOT_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static Process process_redis;
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

        static void Main(string[] args)
        {
            int port_redis_job = 0;
            string name = "JOB";

            #region [ JOB REDIS SETTING ]

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

            #endregion

            timer = new System.Threading.Timer(e => db_sync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(3));

            Console.WriteLine("[ Enter ] to exit ...");
            Console.ReadLine();
            timer.Dispose();

            process_redis.Kill();
            process_redis.Dispose();
        }
    }
}
