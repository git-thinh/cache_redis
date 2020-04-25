using System;
using System.Collections.Generic;
using Microsoft.ClearScript.V8;

namespace ConsoleApplication4
{
    public class clsJS
    {
        public static void Main()
        {
            var engine = new JavaScriptRunner();
            var executionParameters = new Dictionary<string, object>();
            executionParameters["context"] = new Context
            {
                Contact = new Contact { Name = "John Doe" }
            };

            var result = engine.Execute<Context>(@"
                (function() {
                    'use strict';

                    console.log('This is from JavaScript...');
                    console.log(context.Contact.Name);
                    context.Contact.Name = 'Crazy Joe';
                    return context;
                })();
                ", executionParameters);

            Console.WriteLine("Back in .NET...");
            Console.WriteLine(result.Contact.Name);

            Console.ReadKey();
        }
    }


    public class Context
    {
        public Contact Contact { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
    }

    public class JavaScriptRunner
    {
        private readonly JsConsole _console;

        public JavaScriptRunner()
        {
            _console = new JsConsole();
        }

        public T Execute<T>(string script, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var engine = new V8ScriptEngine();

            engine.AddHostType("Action", typeof(Action));
            engine.AddHostObject("console", _console);

            foreach (var kvp in parameters)
            {
                engine.AddHostObject(kvp.Key, kvp.Value);
            }

            var toReturn = engine.Evaluate(script);

            if (toReturn is T)
            {
                return (T)toReturn;
            }

            throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + toReturn.GetType().FullName);
        }
    }

    public class JsConsole
    {
        public void log(string s)
        {
            Console.WriteLine(s);
        }
    }
}
