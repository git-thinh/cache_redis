using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace cache_redis
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string platform = "64";

            string file_node = Path.Combine(path, "x" + platform + @"\node.exe");

            if (File.Exists(file_node))
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.FileName = file_node;
                string argument = @" --max-old-space-size=4096 app.js";
                p.StartInfo.Arguments = argument;
                p.Start();
                //p.WaitForExit();
                Console.ReadLine();
            }



        }
    }
}
