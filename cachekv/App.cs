using Microsoft.Owin.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace cachekv
{
    class App
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

        public static void Main(string[] args)
        {
            string baseAddress = "http://localhost:12345/";
            baseAddress = "http://*:12345/";
            using (WebApp.Start<Startup>(baseAddress))
            {
                var res = new HttpClient().GetAsync("http://localhost:12345/admin").Result;
                Console.WriteLine(res);
                Console.WriteLine(res.Content.ReadAsStringAsync().Result);

                Console.WriteLine("Server Started...Press any key to exit");
                Console.ReadLine();
            }

        }
    }
}
