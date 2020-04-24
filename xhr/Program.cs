using Microsoft.ClearScript.V8;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace xhr
{  

    class Program
    {
        static void Main(string[] args)
        {
            string s = string.Empty;
            try
            {
                string url = "https://zingnews.vn";
                if (args.Length > 0) url = args[0];

                //V8ScriptEngine engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging);
                V8ScriptEngine engine = new V8ScriptEngine(V8ScriptEngineFlags.DisableGlobalMembers);
                //engine.Execute(Script_Text);
                engine.AddCOMType("XMLHttpRequest", "MSXML2.XMLHTTP");
                //object returnedVal = _v8Engine.Script.Execute();
                //return returnedVal;
                //engine.AddCOMType("XMLHttpRequest", "MSXML2.XMLHTTP");
                engine.Execute(@"
    function get(url) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', url, false);
        xhr.send();
        if (xhr.status == 200)
            return xhr.responseText;
        return '';
    }
");

                s = engine.Script.get(url);
                engine.Dispose();
            }
            catch (Exception e)
            {
                s = "ERROR: " + e.Message;
            }

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine(s);

            //Force garbage collection.
            GC.Collect();
            // Wait for all finalizers to complete before continuing.
            GC.WaitForPendingFinalizers();
        }
    }
}