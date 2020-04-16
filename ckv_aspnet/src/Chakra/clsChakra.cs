using ChakraHost.Hosting;
using Newtonsoft.Json;
using Sider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;

namespace ckv_aspnet.src.Chakra
{
    public class clsChakra
    {
        static JavaScriptSourceContext CURRENT_SOURCE_CONTEXT = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        static void _define(JavaScriptValue globalObject, string callbackName, JavaScriptNativeFunction callback)
        {
            JavaScriptPropertyId propertyId = JavaScriptPropertyId.FromString(callbackName);
            JavaScriptValue function = JavaScriptValue.CreateFunction(callback, IntPtr.Zero);
            globalObject.SetProperty(propertyId, function, true);
        }
        static JavaScriptContext _create_context(JavaScriptRuntime runtime)
        {
            JavaScriptContext context = runtime.CreateContext();
            using (new JavaScriptContext.Scope(context))
            {
                JavaScriptValue hostObject = JavaScriptValue.CreateObject();
                JavaScriptValue globalObject = JavaScriptValue.GlobalObject;
                JavaScriptPropertyId hostPropertyId = JavaScriptPropertyId.FromString("api");
                globalObject.SetProperty(hostPropertyId, hostObject, true);

                _api_register(hostObject);
            }
            return context;
        }

        #region [ api.test(...) ]

        private static readonly JavaScriptNativeFunction delegateTest = funcTest;
        static JavaScriptValue funcTest(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            try
            {
                string[] vals = new string[] { };
                if (argumentCount > 0)
                {
                    vals = new string[argumentCount - 1];
                    for (uint i = 1; i < argumentCount; i++)
                        vals[i - 1] = arguments[i].ConvertToString().ToString();
                }

                return JavaScriptValue.FromString("TEST: " + Guid.NewGuid().ToString() + " = " + JsonConvert.SerializeObject(vals));
            }
            catch
            {
                return JavaScriptValue.Invalid; // js return underfine
            }
        }

        #endregion

        #region [ api.log(...) ]

        private static readonly JavaScriptNativeFunction delegateLog = funcLog;
        static JavaScriptValue funcLog(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            try
            {
                if (argumentCount > 0)
                {
                    var vals = new string[argumentCount - 1];
                    for (uint i = 1; i < argumentCount; i++)
                        vals[i - 1] = arguments[i].ConvertToString().ToString();

                    string scope_name = "_", key = "", text = "";
                    switch (vals.Length)
                    {
                        case 1:
                            key = DateTime.Now.ToString("yyMMdd-HHmmss-fff");
                            text = vals[0];
                            break;
                        case 2:
                            key = DateTime.Now.ToString("yyMMdd-HHmmss-fff") + "." + vals[0];
                            text = vals[1];
                            break;
                        default:
                            scope_name = vals[0];
                            key = DateTime.Now.ToString("yyMMdd-HHmmss-fff") + "." + vals[1];
                            text = string.Join(Environment.NewLine + Environment.NewLine, vals.Where((x, i) => i > 1).ToArray());
                            break;
                    }
                    m_log.write(scope_name, key, text);

                    return JavaScriptValue.FromBoolean(true);
                }
            }
            catch { }
            return JavaScriptValue.FromBoolean(false);
        }

        #endregion

        static void _api_register(JavaScriptValue hostObject)
        {
            _define(hostObject, "test", delegateTest);
            _define(hostObject, "log", delegateLog);
        }

        static ILogJS m_log;
        static JavaScriptRuntime js_runtime;
        static JavaScriptContext js_context;
        public static void _init()
        {
            m_log = new clsLogJS();
            js_runtime = JavaScriptRuntime.Create();
            js_context = _create_context(js_runtime);
        }

        #region [ test_1; test_2 ]

        public static oResult test_1(Dictionary<string, object> request = null)
        {
            oResult rs = new oResult() { ok = false, request = request };

            using (JavaScriptRuntime runtime = JavaScriptRuntime.Create())
            {
                JavaScriptContext context = _create_context(runtime);

                using (new JavaScriptContext.Scope(context))
                {
                    string script = "(()=>{ var val = api.test('TEST_1: Hello world'); api.log('api-js.test-1', 'key-1', 'TEST_1: This log called by JS...'); return val; })()";

                    try
                    {
                        JavaScriptValue result = JavaScriptContext.RunScript(script, CURRENT_SOURCE_CONTEXT++, string.Empty);
                        rs.data = result.ConvertToString().ToString();
                        rs.ok = true;
                    }
                    catch (JavaScriptScriptException e)
                    {
                        var messageName = JavaScriptPropertyId.FromString("message");
                        rs.error = "ERROR_JS_EXCEPTION: " + e.Error.GetProperty(messageName).ConvertToString().ToString();
                    }
                    catch (Exception e)
                    {
                        rs.error = "ERROR_CHAKRA: failed to run script: " + e.Message;
                    }
                }
            }
            return rs;
        }

        public static oResult test_2(Dictionary<string, object> request = null)
        {
            oResult rs = new oResult() { ok = false, request = request };
            using (new JavaScriptContext.Scope(js_context))
            {
                string script = "(()=>{ var val = api.test('TEST_2: Hello world'); api.log('api-js.test-2', 'key-2', 'TEST_2: This log called by JS...'); return val; })()";

                try
                {
                    JavaScriptValue result = JavaScriptContext.RunScript(script, CURRENT_SOURCE_CONTEXT++, string.Empty);
                    rs.data = result.ConvertToString().ToString();
                    rs.ok = true;
                }
                catch (JavaScriptScriptException e)
                {
                    var messageName = JavaScriptPropertyId.FromString("message");
                    rs.error = "ERROR_JS_EXCEPTION: " + e.Error.GetProperty(messageName).ConvertToString().ToString();
                }
                catch (Exception e)
                {
                    rs.error = "ERROR_CHAKRA: failed to run script: " + e.Message;
                }
            }
            return rs;
        }

        #endregion

        public static oResult run_api(Dictionary<string, object> request = null)
        {
            oResult rs = new oResult() { ok = false, request = request };

            string scope = request.getValueByKey("___scope");
            if (string.IsNullOrEmpty(scope))
            {
                rs.error = "[___scope] is null or empty";
                return rs;
            }

            if (!clsApi.Exist(scope))
            {
                rs.error = "[___scope] = " + scope + " is not exist";
                return rs;
            }

            string sapi = request.getValueByKey("___api");
            if (string.IsNullOrEmpty(sapi))
            {
                rs.error = "[___api] is null or empty";
                return rs;
            }

            oApi o = clsApi.Get(scope);
            if (o.apis.Length == 0)
            {
                rs.error = "folder " + scope + " missing files api: " + sapi.Replace("|", ".js; ") + ".js";
                return rs;
            }

            sapi = sapi.ToLower().Trim();
            string[] a = sapi.Split('|');
            StringBuilder bi = new StringBuilder();

            bi.Append("(()=>{ ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___scope = 'API-JS." + scope + "." + sapi + "'; ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___log = function(key,text){ api.log(___scope, key, text); }; ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___para = ");
            bi.Append(JsonConvert.SerializeObject(request));
            bi.Append(Environment.NewLine);
            bi.Append(Environment.NewLine);

            for (var i = 0; i < a.Length; i++)
            {
                if (o.apis_data.ContainsKey(a[i]) == false)
                {
                    rs.error = "folder " + scope + " missing files api: " + a[i] + ".js";
                    return rs;
                }
                bi.Append(o.apis_data[a[i]]);
                bi.Append(Environment.NewLine);
                bi.Append(Environment.NewLine);
            }

            bi.Append(Environment.NewLine);
            bi.Append(" })()");

            string script = bi.ToString();

            using (new JavaScriptContext.Scope(js_context))
            {
                try
                {
                    JavaScriptValue result = JavaScriptContext.RunScript(script, CURRENT_SOURCE_CONTEXT++, string.Empty);
                    string v = result.ConvertToString().ToString();
                    if (v == "undefined") rs.data = null;
                    else rs.data = v;
                    rs.ok = true;
                }
                catch (JavaScriptScriptException e)
                {
                    var messageName = JavaScriptPropertyId.FromString("message");
                    rs.error = "ERROR_JS_EXCEPTION: " + e.Error.GetProperty(messageName).ConvertToString().ToString();
                }
                catch (Exception e)
                {
                    rs.error = "ERROR_CHAKRA: failed to run script: " + e.Message;
                }
            }
            return rs;
        }
    }


    public interface ILogJS
    {
        void write(string scope_name, string key, string text);
    }

    public class clsLogJS : ILogJS
    {
        static long ID_INCREMENT = 0;
        static RedisClient _redis;
        static bool _connected = false;

        public clsLogJS(int port = _CONFIG.LOG_PORT_REDIS) => _init(port);

        void _init(int port)
        {
            try
            {
                if (_connected == false)
                {
                    _redis = new RedisClient("localhost", port);
                    _connected = true;
                }
            }
            catch
            {
            }
        }

        public void write(string scope_name, string key, string text)
        {
            if (_connected == false) return;
            if (text == null) text = string.Empty;

            try
            {
                _redis.HSet(scope_name, key, text);
            }
            catch
            {
            }
        }

        void write1(string scope_name, string key, params object[] paras)
        {
            Interlocked.Increment(ref ID_INCREMENT);
            if (ID_INCREMENT == int.MaxValue) ID_INCREMENT = 0;

            string id = string.Format("{0}.{1}.{2}", DateTime.Now.ToString("yyMMddHHmm"), ID_INCREMENT, key);

            StringBuilder bi = new StringBuilder(DateTime.Now.ToString("yyMMdd.HHmmss.fff"));
            bi.Append(Environment.NewLine);
            try
            {
                for (var i = 0; i < paras.Length; i++)
                {
                    string type = paras[i].GetType().Name;
                    switch (type)
                    {
                        case "Boolean":
                        case "Byte":
                        case "SByte":
                        case "Char":
                        case "Decimal":
                        case "Double":
                        case "Single":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Int16":
                        case "UInt16":
                        case "String":
                            bi.Append(paras[i].ToString());
                            break;
                        default:
                            string text = JsonConvert.SerializeObject(paras[i], Formatting.Indented);
                            bi.Append(text);
                            bi.Append(Environment.NewLine);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string text = JsonConvert.SerializeObject(paras, Formatting.Indented);
                bi.Append(text);
                bi.Append(Environment.NewLine);
                bi.Append("ERROR_LOG: " + ex.Message);
            }

            try
            {
                _redis.HSet(scope_name, id, bi.ToString());
            }
            catch { }
        }
    }
}