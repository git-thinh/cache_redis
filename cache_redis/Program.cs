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

            string file_node = Path.Combine(path, "node.exe");
            if (File.Exists(file_node))
            {
                Process node = new Process();
                node.StartInfo.UseShellExecute = false;
                node.StartInfo.RedirectStandardOutput = true;
                node.StartInfo.RedirectStandardError = true;
                node.StartInfo.RedirectStandardInput = true;
                node.StartInfo.FileName = file_node;
                string argument = @" --max-old-space-size=4096 app.js";
                node.StartInfo.Arguments = argument;
                node.Start();
            }

            string file_redis = Path.Combine(path, "redis-server.exe");
            if (File.Exists(file_redis))
            {
                Process redis = new Process();
                redis.StartInfo.UseShellExecute = false;
                redis.StartInfo.RedirectStandardOutput = true;
                redis.StartInfo.RedirectStandardError = true;
                redis.StartInfo.RedirectStandardInput = true;
                redis.StartInfo.FileName = file_redis;
                string argument = @" redis.windows.conf --port 5500";
                redis.StartInfo.Arguments = argument;
                redis.Start();
            }

            Console.ReadLine();

        }
    }
}
