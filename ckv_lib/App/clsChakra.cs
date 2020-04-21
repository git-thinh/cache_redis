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

namespace ckv_lib
{
    public class clsChakra
    {
        public static bool m_ready = false;
        public static string ERROR_MESSAGE = string.Empty;

        #region [ BASE ]

        static List<string> m_apis = new List<string>();
        public static string[] js_apis() => m_apis.ToArray();
        static JavaScriptSourceContext CURRENT_SOURCE_CONTEXT = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        static void _define(JavaScriptValue globalObject, string callbackName, JavaScriptNativeFunction callback)
        {
            JavaScriptPropertyId propertyId = JavaScriptPropertyId.FromString(callbackName);
            JavaScriptValue function = JavaScriptValue.CreateFunction(callback, IntPtr.Zero);
            globalObject.SetProperty(propertyId, function, true);
            m_apis.Add(callbackName);
        }
        static JavaScriptContext _create_context(JavaScriptRuntime runtime)
        {
            JavaScriptContext context = runtime.CreateContext();
            using (new JavaScriptContext.Scope(context))
            {
                JavaScriptValue hostObject_API = JavaScriptValue.CreateObject();
                JavaScriptPropertyId hostPropertyId_API = JavaScriptPropertyId.FromString("___api");
                JavaScriptValue.GlobalObject.SetProperty(hostPropertyId_API, hostObject_API, true);
                _register(hostObject_API);
            }
            return context;
        }

        #endregion

        #region [ test | log ]

        private static readonly JavaScriptNativeFunction delegate_test = fun_test;
        static JavaScriptValue fun_test(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
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

        private static readonly JavaScriptNativeFunction delegate_log = fun_log;
        static JavaScriptValue fun_log(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
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

        #region [ notify_user | notify_broadcast ]

        private static readonly JavaScriptNativeFunction delegate_notify_user = fun_notify_user;
        private static readonly JavaScriptNativeFunction delegate_notify_broadcast = fun_notify_broadcast;
        static JavaScriptValue fun_notify_user(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_notify_broadcast(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        #region [ ajax_call | curl_call ( get|post|put... ) ]

        private static readonly JavaScriptNativeFunction delegate_ajax_call = fun_ajax_call;
        private static readonly JavaScriptNativeFunction delegate_curl_call = fun_curl_call;
        static JavaScriptValue fun_ajax_call(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_curl_call(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        #region [ api_list | api_get | api_reload | api_reload_all | api_exist ]

        private static readonly JavaScriptNativeFunction delegate_api_list = fun_api_list;
        private static readonly JavaScriptNativeFunction delegate_api_get = fun_api_get;
        private static readonly JavaScriptNativeFunction delegate_api_reload = fun_api_reload;
        private static readonly JavaScriptNativeFunction delegate_api_reload_all = fun_api_reload_all;
        private static readonly JavaScriptNativeFunction delegate_api_exist = fun_api_exist;
        static JavaScriptValue fun_api_list(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_api_get(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_api_reload(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_api_reload_all(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_api_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        #region [ file_read_text | file_write_text | file_append_text | file_exist | file_delete | dir_get_files | dir_exist | dir_create | dir_delete ]

        private static readonly JavaScriptNativeFunction delegate_file_read_text = fun_file_read_text;
        private static readonly JavaScriptNativeFunction delegate_file_write_text = fun_file_write_text;
        private static readonly JavaScriptNativeFunction delegate_file_append_text = fun_file_append_text;
        private static readonly JavaScriptNativeFunction delegate_file_exist = fun_file_exist;
        private static readonly JavaScriptNativeFunction delegate_file_delete = fun_file_delete;
        private static readonly JavaScriptNativeFunction delegate_dir_get_files = fun_dir_get_files;
        private static readonly JavaScriptNativeFunction delegate_dir_exist = fun_dir_exist;
        private static readonly JavaScriptNativeFunction delegate_dir_create = fun_dir_create;
        private static readonly JavaScriptNativeFunction delegate_dir_delete = fun_dir_delete;

        static JavaScriptValue fun_file_read_text(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_file_write_text(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_file_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_file_delete(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_file_append_text(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_dir_get_files(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_dir_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_dir_create(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_dir_delete(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        #region [ cache_addnew | cache_update | cache_remove | cache_clear_all ]

        private static readonly JavaScriptNativeFunction delegate_cache_addnew = fun_cache_addnew;
        private static readonly JavaScriptNativeFunction delegate_cache_update = fun_cache_update;
        private static readonly JavaScriptNativeFunction delegate_cache_remove = fun_cache_remove;
        private static readonly JavaScriptNativeFunction delegate_cache_clear_all = fun_cache_clear_all;

        static JavaScriptValue fun_cache_addnew(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            return JavaScriptValue.Invalid;
        }

        static JavaScriptValue fun_cache_update(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            return JavaScriptValue.Invalid;
        }

        static JavaScriptValue fun_cache_remove(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            return JavaScriptValue.Invalid;
        }

        static JavaScriptValue fun_cache_clear_all(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            return JavaScriptValue.Invalid;
        }

        #endregion

        #region [ cache_runtime_exist |cache_runtime_set | cache_runtime_get | cache_runtime_remove ]

        private static readonly JavaScriptNativeFunction delegate_cache_runtime_exist = fun_cache_runtime_exist;
        private static readonly JavaScriptNativeFunction delegate_cache_runtime_set = fun_cache_runtime_set;
        private static readonly JavaScriptNativeFunction delegate_cache_runtime_get = fun_cache_runtime_get;
        private static readonly JavaScriptNativeFunction delegate_cache_runtime_remove = fun_cache_runtime_remove;
        static JavaScriptValue fun_cache_runtime_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_cache_runtime_set(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_cache_runtime_get(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_cache_runtime_remove(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        #region [ cache_search | cache_get_item_by_id | cache_get_items_by_ids ]

        private static readonly JavaScriptNativeFunction delegate_cache_search = fun_cache_search;
        private static readonly JavaScriptNativeFunction delegate_cache_get_item_by_id = fun_cache_get_item_by_id;
        private static readonly JavaScriptNativeFunction delegate_cache_get_items_by_ids = fun_cache_get_items_by_ids;
        static JavaScriptValue fun_cache_search(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_cache_get_item_by_id(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_cache_get_items_by_ids(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        #region [ db_execute ]

        private static readonly JavaScriptNativeFunction delegate_db_execute = fun_db_execute;
        static JavaScriptValue fun_db_execute(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            return JavaScriptValue.Invalid;
        }

        #endregion

        #region [ job_list | job_create | job_stop | job_start | job_remove ]

        private static readonly JavaScriptNativeFunction delegate_job_list = fun_job_list;
        private static readonly JavaScriptNativeFunction delegate_job_create = fun_job_create;
        private static readonly JavaScriptNativeFunction delegate_job_stop = fun_job_stop;
        private static readonly JavaScriptNativeFunction delegate_job_start = fun_job_start;
        private static readonly JavaScriptNativeFunction delegate_job_remove = fun_job_remove;
        static JavaScriptValue fun_job_list(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_job_create(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_job_stop(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_job_start(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;
        static JavaScriptValue fun_job_remove(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData) => JavaScriptValue.Invalid;

        #endregion

        static void _register(JavaScriptValue hostObject)
        {
            _define(hostObject, "test", delegate_test);
            _define(hostObject, "log", delegate_log);

            _define(hostObject, "curl_call", delegate_curl_call);
            _define(hostObject, "ajax_call", delegate_ajax_call);

            _define(hostObject, "cache_update", delegate_cache_update);
            _define(hostObject, "cache_addnew", delegate_cache_addnew);
            _define(hostObject, "cache_remove", delegate_cache_remove);
            _define(hostObject, "cache_clear_all", delegate_cache_clear_all);

            _define(hostObject, "cache_runtime_set", delegate_cache_runtime_set);
            _define(hostObject, "cache_runtime_get", delegate_cache_runtime_get);
            _define(hostObject, "cache_runtime_remove", delegate_cache_runtime_remove);

            _define(hostObject, "cache_search", delegate_cache_search);
            _define(hostObject, "cache_get_item_by_id", delegate_cache_get_item_by_id);
            _define(hostObject, "cache_get_items_by_ids", delegate_cache_get_items_by_ids);

            _define(hostObject, "db_execute", delegate_db_execute);

            _define(hostObject, "file_read_text", delegate_file_read_text);
            _define(hostObject, "file_write_text", delegate_file_write_text);
            _define(hostObject, "file_append_text", delegate_file_append_text);
            _define(hostObject, "file_exist", delegate_file_exist);
            _define(hostObject, "file_delete", delegate_file_delete);

            _define(hostObject, "dir_get_files", delegate_dir_get_files);
            _define(hostObject, "dir_exist", delegate_dir_exist);
            _define(hostObject, "dir_create", delegate_dir_create);
            _define(hostObject, "dir_delete", delegate_dir_delete);

            _define(hostObject, "job_list", fun_job_list);
            _define(hostObject, "job_create", delegate_job_create);
            _define(hostObject, "job_stop", delegate_job_stop);
            _define(hostObject, "job_start", delegate_job_start);
            _define(hostObject, "job_remove", delegate_job_remove);

            _define(hostObject, "notify_user", delegate_notify_user);
            _define(hostObject, "notify_broadcast", delegate_notify_broadcast);

            _define(hostObject, "api_list", delegate_api_list);
            _define(hostObject, "api_get", delegate_api_get);
            _define(hostObject, "api_reload", delegate_api_reload);
            _define(hostObject, "api_reload_all", delegate_api_reload_all);
            _define(hostObject, "api_exist", delegate_api_exist);
        }

        static ILogJS m_log;
        static JavaScriptRuntime js_runtime;
        static JavaScriptContext js_context;
        public static void _init()
        {
            try
            {
                m_log = new clsLogJS();
                js_runtime = JavaScriptRuntime.Create();
                js_context = _create_context(js_runtime);
                m_ready = true;
            }
            catch (Exception ex) {
                ERROR_MESSAGE = ex.Message;
            }
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
                    string script = "(()=>{ var val = ___api.test('TEST_1: Hello world'); ___api.log('api-js.test-1', 'key-1', 'TEST_1: This log called by JS...'); return val; })()";

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
                string script = "(()=>{ var val = ___api.test('TEST_2: Hello world'); ___api.log('api-js.test-2', 'key-2', 'TEST_2: This log called by JS...'); return val; })()";

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
            bi.Append("var ___log = function(key,text){ ___api.log(___scope, key, text); }; ");
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

        public clsLogJS(int port = 0) => _init(port == 0 ? _CONFIG.LOG_PORT_REDIS : port);

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