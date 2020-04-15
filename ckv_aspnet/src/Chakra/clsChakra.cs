using ChakraHost.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace ckv_aspnet.src.Chakra
{
    public class clsChakra
    {
        #region [ js_chakra ]

        public static string js_chakra_run(object p = null)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.ToString())) return string.Empty;

            string body_function = p.ToString();

            string script = "(()=>{return \'Hello world!\';})()";
            //string script = "(()=>{ try{ " + body_function + " }catch(e){ return 'ERR:'+e.message; } })()";
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

        public static string js_chakra_run_1(object p = null)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.ToString())) return string.Empty;
            string body_function = p.ToString();

            string script = "(()=>{return \'Hello world!\';})()";
            //string script = "(()=>{ try{ " + body_function + " }catch(e){ return 'ERR:'+e.message; } })()";
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

        #region [ js_chakra 2]

        private struct CommandLineArguments
        {
            public int ArgumentsStart;
        };

        private static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);

        // We have to hold on to the delegates on the managed side of things so that the
        // delegates aren't collected while the script is running.
        private static readonly JavaScriptNativeFunction echoDelegate = Echo;
        private static readonly JavaScriptNativeFunction runScriptDelegate = RunScript;

        private static void ThrowException(string errorString)
        {
            // We ignore error since we're already in an error state.
            JavaScriptValue errorValue = JavaScriptValue.FromString(errorString);
            JavaScriptValue errorObject = JavaScriptValue.CreateError(errorValue);
            JavaScriptContext.SetException(errorObject);
        }

        private static JavaScriptValue Echo(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            string val = "";
            for (uint index = 1; index < argumentCount; index++)
            {
                if (index > 1)
                {
                    Console.Write(" ");
                }

                val = arguments[index].ConvertToString().ToString();

                //Console.Write(arguments[index].ConvertToString().ToString());
            }

            //Console.WriteLine();
            //return JavaScriptValue.Invalid;
            return JavaScriptValue.FromString(Guid.NewGuid().ToString() + " = " + val);
        }

        private static JavaScriptValue RunScript(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            if (argumentCount < 2)
            {
                ThrowException("not enough arguments");
                return JavaScriptValue.Invalid;
            }

            //
            // Convert filename.
            //

            string filename = arguments[1].ToString();

            //
            // Load the script from the disk.
            //

            string script = File.ReadAllText(filename);
            if (string.IsNullOrEmpty(script))
            {
                ThrowException("invalid script");
                return JavaScriptValue.Invalid;
            }

            //
            // Run the script.
            //

            return JavaScriptContext.RunScript(script, currentSourceContext++, filename);
        }

        private static void DefineHostCallback(JavaScriptValue globalObject, string callbackName, JavaScriptNativeFunction callback, IntPtr callbackData)
        {
            //
            // Get property ID.
            //

            JavaScriptPropertyId propertyId = JavaScriptPropertyId.FromString(callbackName);

            //
            // Create a function
            //

            JavaScriptValue function = JavaScriptValue.CreateFunction(callback, callbackData);

            //
            // Set the property
            //

            globalObject.SetProperty(propertyId, function, true);
        }

        private static JavaScriptContext CreateHostContext(JavaScriptRuntime runtime, string[] arguments, int argumentsStart)
        {
            //
            // Create the context. Note that if we had wanted to start debugging from the very
            // beginning, we would have called JsStartDebugging right after context is created.
            //

            JavaScriptContext context = runtime.CreateContext();

            //
            // Now set the execution context as being the current one on this thread.
            //

            using (new JavaScriptContext.Scope(context))
            {
                //
                // Create the host object the script will use.
                //

                JavaScriptValue hostObject = JavaScriptValue.CreateObject();

                //
                // Get the global object
                //

                JavaScriptValue globalObject = JavaScriptValue.GlobalObject;

                //
                // Get the name of the property ("host") that we're going to set on the global object.
                //

                JavaScriptPropertyId hostPropertyId = JavaScriptPropertyId.FromString("host");

                //
                // Set the property.
                //

                globalObject.SetProperty(hostPropertyId, hostObject, true);

                //
                // Now create the host callbacks that we're going to expose to the script.
                //

                DefineHostCallback(hostObject, "echo", echoDelegate, IntPtr.Zero);
                //////////DefineHostCallback(hostObject, "runScript", runScriptDelegate, IntPtr.Zero);

                //////////
                ////////// Create an array for arguments.
                //////////

                ////////JavaScriptValue hostArguments = JavaScriptValue.CreateArray((uint)(arguments.Length - argumentsStart));

                ////////for (int index = argumentsStart; index < arguments.Length; index++)
                ////////{
                ////////    //
                ////////    // Create the argument value.
                ////////    //

                ////////    JavaScriptValue argument = JavaScriptValue.FromString(arguments[index]);

                ////////    //
                ////////    // Create the index.
                ////////    //

                ////////    JavaScriptValue indexValue = JavaScriptValue.FromInt32(index - argumentsStart);

                ////////    //
                ////////    // Set the value.
                ////////    //

                ////////    hostArguments.SetIndexedProperty(indexValue, argument);
                ////////}

                //////////
                ////////// Get the name of the property that we're going to set on the host object.
                //////////

                ////////JavaScriptPropertyId argumentsPropertyId = JavaScriptPropertyId.FromString("arguments");

                //////////
                ////////// Set the arguments property.
                //////////

                ////////hostObject.SetProperty(argumentsPropertyId, hostArguments, true);
            }

            return context;
        }

        private static void PrintScriptException(JavaScriptValue exception)
        {
            //
            // Get message.
            //

            JavaScriptPropertyId messageName = JavaScriptPropertyId.FromString("message");
            JavaScriptValue messageValue = exception.GetProperty(messageName);
            string message = messageValue.ToString();

            Console.Error.WriteLine("chakrahost: exception: {0}", message);
        }

        public static string js_chakra_run_2(object p = null)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.ToString())) return string.Empty;

            string body_function = p.ToString();

            string v = "";
            //string script = "(()=>{ try{ " + body_function + " }catch(e){ return 'ERR:'+e.message; } })()";
            //var result = JavaScriptContext.RunScript(script, js_currentSourceContext++, string.Empty);
            //string v = result.ConvertToString().ToString();


            //int returnValue = 1;
            CommandLineArguments commandLineArguments;
            commandLineArguments.ArgumentsStart = 0;
            string[] arguments = new string[] { "chakrahost", "test.js", "12345" };

            using (JavaScriptRuntime runtime = JavaScriptRuntime.Create())
            {
                //
                // Similarly, create a single execution context. Note that we're putting it on the stack here,
                // so it will stay alive through the entire run.
                //

                JavaScriptContext context = CreateHostContext(runtime, arguments, commandLineArguments.ArgumentsStart);

                //
                // Now set the execution context as being the current one on this thread.
                //

                using (new JavaScriptContext.Scope(context))
                {
                    //
                    // Load the script from the disk.
                    //

                    //string script = File.ReadAllText(arguments[commandLineArguments.ArgumentsStart]);
                    string script = "(()=>{ var val = host.echo('Hello world'); return val; })()";

                    //
                    // Run the script.
                    //

                    JavaScriptValue result = new JavaScriptValue();
                    try
                    {
                        result = JavaScriptContext.RunScript(script, currentSourceContext++, arguments[commandLineArguments.ArgumentsStart]);
                    }
                    catch (JavaScriptScriptException e)
                    {
                        PrintScriptException(e.Error);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("chakrahost: failed to run script: {0}", e.Message);
                    }

                    //
                    // Convert the return value.
                    //

                    //JavaScriptValue numberResult = result.ConvertToNumber();
                    //double doubleResult = numberResult.ToDouble();
                    //returnValue = (int)doubleResult;
                    //v = returnValue.ToString();

                    v = result.ConvertToString().ToString();
                }
            }

            return v;
        }

        #endregion

    }
}