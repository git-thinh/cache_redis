using ChakraHost.Hosting;
using HtmlAgilityPack;
using Microsoft.ClearScript.V8;
//using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
        static JavaScriptValue fun_notify_user(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("notify_user", arguments);
        static JavaScriptValue fun_notify_broadcast(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("notify_broadcast", arguments);

        #endregion

        #region [ request_async | curl_call ( get|post|put... ) ]

        private static readonly JavaScriptNativeFunction delegate_request_async = fun_request_async;
        private static readonly JavaScriptNativeFunction delegate_curl_call = fun_curl_call;
        static JavaScriptValue fun_request_async(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("request_async", arguments);
        static JavaScriptValue fun_curl_call(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("curl_call", arguments);

        #endregion

        #region [ html_export_links | html_export_images | html_to_text_01 | html_clean_01 | html_remove_comment | html_remove_tag_simple | html_remove_tag_content | html_remove_tag_keep_content ]

        private static readonly JavaScriptNativeFunction delegate_html_export_links = fun_html_export_links;
        private static readonly JavaScriptNativeFunction delegate_html_export_images = fun_html_export_images;
        private static readonly JavaScriptNativeFunction delegate_html_to_text_01 = fun_html_to_text_01;
        private static readonly JavaScriptNativeFunction delegate_html_clean_01 = fun_html_clean_01;
        private static readonly JavaScriptNativeFunction delegate_html_remove_comment = fun_html_remove_comment;
        private static readonly JavaScriptNativeFunction delegate_html_remove_tag_simple = fun_html_remove_tag_simple;
        private static readonly JavaScriptNativeFunction delegate_html_remove_tag_content = fun_html_remove_tag_content;
        private static readonly JavaScriptNativeFunction delegate_html_remove_tag_keep_content = fun_html_remove_tag_keep_content;
        static JavaScriptValue fun_html_export_links(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_export_links", arguments);
        static JavaScriptValue fun_html_export_images(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_export_images", arguments);
        static JavaScriptValue fun_html_to_text_01(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_to_text_01", arguments);
        static JavaScriptValue fun_html_clean_01(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_clean_01", arguments);
        static JavaScriptValue fun_html_remove_comment(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_remove_comment", arguments);
        static JavaScriptValue fun_html_remove_tag_simple(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_remove_tag_simple", arguments);
        static JavaScriptValue fun_html_remove_tag_content(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_remove_tag_content", arguments);
        static JavaScriptValue fun_html_remove_tag_keep_content(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("html_remove_tag_keep_content", arguments);

        #endregion

        #region [ api_list | api_get | api_reload | api_reload_all | api_exist ]

        private static readonly JavaScriptNativeFunction delegate_api_list = fun_api_list;
        private static readonly JavaScriptNativeFunction delegate_api_get = fun_api_get;
        private static readonly JavaScriptNativeFunction delegate_api_reload = fun_api_reload;
        private static readonly JavaScriptNativeFunction delegate_api_reload_all = fun_api_reload_all;
        private static readonly JavaScriptNativeFunction delegate_api_exist = fun_api_exist;
        static JavaScriptValue fun_api_list(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("api_list", arguments);
        static JavaScriptValue fun_api_get(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("api_get", arguments);
        static JavaScriptValue fun_api_reload(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("api_reload", arguments);
        static JavaScriptValue fun_api_reload_all(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("api_reload_all", arguments);
        static JavaScriptValue fun_api_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("api_exist", arguments);

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

        static JavaScriptValue fun_file_read_text(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("file_read_text", arguments);
        static JavaScriptValue fun_file_write_text(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("file_write_text", arguments);
        static JavaScriptValue fun_file_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("file_append_text", arguments);
        static JavaScriptValue fun_file_delete(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("file_exist", arguments);
        static JavaScriptValue fun_file_append_text(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("file_delete", arguments);
        static JavaScriptValue fun_dir_get_files(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("dir_get_files", arguments);
        static JavaScriptValue fun_dir_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("dir_exist", arguments);
        static JavaScriptValue fun_dir_create(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("dir_create", arguments);
        static JavaScriptValue fun_dir_delete(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("dir_delete", arguments);

        #endregion

        #region [ cache_addnew | cache_update | cache_remove | cache_clear_all ]

        private static readonly JavaScriptNativeFunction delegate_cache_addnew = fun_cache_addnew;
        private static readonly JavaScriptNativeFunction delegate_cache_update = fun_cache_update;
        private static readonly JavaScriptNativeFunction delegate_cache_remove = fun_cache_remove;
        private static readonly JavaScriptNativeFunction delegate_cache_clear_all = fun_cache_clear_all;

        static JavaScriptValue fun_cache_addnew(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_addnew", arguments);

        static JavaScriptValue fun_cache_update(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_update", arguments);

        static JavaScriptValue fun_cache_remove(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_remove", arguments);

        static JavaScriptValue fun_cache_clear_all(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_clear_all", arguments);

        #endregion

        #region [ cache_runtime_exist |cache_runtime_set | cache_runtime_get | cache_runtime_remove ]

        private static readonly JavaScriptNativeFunction delegate_cache_runtime_exist = fun_cache_runtime_exist;
        private static readonly JavaScriptNativeFunction delegate_cache_runtime_set = fun_cache_runtime_set;
        private static readonly JavaScriptNativeFunction delegate_cache_runtime_get = fun_cache_runtime_get;
        private static readonly JavaScriptNativeFunction delegate_cache_runtime_remove = fun_cache_runtime_remove;
        static JavaScriptValue fun_cache_runtime_exist(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_runtime_exist", arguments);
        static JavaScriptValue fun_cache_runtime_set(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_runtime_set", arguments);
        static JavaScriptValue fun_cache_runtime_get(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_runtime_get", arguments);
        static JavaScriptValue fun_cache_runtime_remove(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_runtime_remove", arguments);

        #endregion

        #region [ cache_search | cache_get_item_by_id | cache_get_items_by_ids ]

        private static readonly JavaScriptNativeFunction delegate_cache_search = fun_cache_search;
        private static readonly JavaScriptNativeFunction delegate_cache_get_item_by_id = fun_cache_get_item_by_id;
        private static readonly JavaScriptNativeFunction delegate_cache_get_items_by_ids = fun_cache_get_items_by_ids;
        static JavaScriptValue fun_cache_search(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_search", arguments);
        static JavaScriptValue fun_cache_get_item_by_id(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_get_item_by_id", arguments);
        static JavaScriptValue fun_cache_get_items_by_ids(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("cache_get_items_by_ids", arguments);

        #endregion

        #region [ db_execute ]

        private static readonly JavaScriptNativeFunction delegate_db_execute = fun_db_execute;
        static JavaScriptValue fun_db_execute(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("db_execute", arguments);

        #endregion

        #region [ job_list | job_create | job_stop | job_start | job_remove ]

        private static readonly JavaScriptNativeFunction delegate_job_list = fun_job_list;
        private static readonly JavaScriptNativeFunction delegate_job_create = fun_job_create;
        private static readonly JavaScriptNativeFunction delegate_job_stop = fun_job_stop;
        private static readonly JavaScriptNativeFunction delegate_job_start = fun_job_start;
        private static readonly JavaScriptNativeFunction delegate_job_remove = fun_job_remove;
        static JavaScriptValue fun_job_list(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("job_list", arguments);
        static JavaScriptValue fun_job_create(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("job_create", arguments);
        static JavaScriptValue fun_job_stop(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("job_stop", arguments);
        static JavaScriptValue fun_job_start(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("job_start", arguments);
        static JavaScriptValue fun_job_remove(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
            => _call_function("job_remove", arguments);

        #endregion

        static void _register(JavaScriptValue hostObject)
        {
            _define(hostObject, "test", delegate_test);
            _define(hostObject, "log", delegate_log);

            _define(hostObject, "curl_call", delegate_curl_call);
            _define(hostObject, "request_async", delegate_request_async);

            _define(hostObject, "html_export_links", delegate_html_export_links);
            _define(hostObject, "html_export_images", delegate_html_export_images);
            _define(hostObject, "html_to_text_01", delegate_html_to_text_01);
            _define(hostObject, "html_clean_01", delegate_html_clean_01);
            _define(hostObject, "html_remove_comment", delegate_html_remove_comment);
            _define(hostObject, "html_remove_tag_simple", delegate_html_remove_tag_simple);
            _define(hostObject, "html_remove_tag_content", delegate_html_remove_tag_content);
            _define(hostObject, "html_remove_tag_keep_content", delegate_html_remove_tag_keep_content);

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

            _define(hostObject, "job_list", delegate_job_list);
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
            catch (Exception ex)
            {
                ERROR_MESSAGE = "ERROR_INIT: " + ex.Message;
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

        static JavaScriptValue _call_function(string function, JavaScriptValue[] arguments)
        {
            var para = arguments.getDictionary();
            oResult result = new oResult() { ok = false, request = para };

            if (m_ready == false)
            {
                result.error = ERROR_MESSAGE;
                return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
            }

            if (para.Count == 0)
            {
                result.error = "Arguments is null or empty. Not If Arguments must be string text json, may be JSON.stringify({...})";
                return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
            }

            switch (function)
            {
                case "":
                    #region [  ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("headers") == false)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [headers] not exist";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }
                    }

                    #endregion
                    break;
                case "request_async":
                    #region [ request_async ]

                    if (para.Count > 0 && para.ContainsKey("headers"))
                    {
                        #region

                        Dictionary<string, object> headers = null;

                        if (para.ContainsKey("headers") == false)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [headers] not exist";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        if (para.ContainsKey("headers"))
                        {
                            try
                            {
                                headers = ((JObject)para["headers"]).ToObject<Dictionary<string, object>>();
                            }
                            catch (Exception e)
                            {
                                result.error = "ERROR_" + function.ToUpper() + ": The paramenter [headers] must be format JSON. " + e.Message;
                                return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                            }
                        }

                        if (headers.Count == 0
                            || headers.ContainsKey("url") == false || headers["url"] == null || string.IsNullOrEmpty(headers["url"].ToString())
                            || headers.ContainsKey("method") == false || headers["method"] == null || string.IsNullOrEmpty(headers["method"].ToString()))
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [headers] must be { url:..., method:... } and Value is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        #endregion

                        try
                        {
                            string httpMethod = "GET";
                            string type = "v8";
                            string url = string.Empty;
                            string htm = string.Empty;

                            foreach (var header in headers)
                            {
                                if (header.Key.StartsWith("Content") || header.Key == "data") continue;

                                if (header.Key == "url")
                                {
                                    url = header.Value.ToString();
                                    continue;
                                }

                                if (header.Key == "method")
                                {
                                    httpMethod = header.Value.ToString();
                                    continue;
                                }

                                if (header.Key == "type")
                                {
                                    type = header.Value.ToString();
                                    continue;
                                }
                            }
                            switch (type)
                            {
                                case "curl":
                                    htm = clsCURL.___https(url);
                                    result.ok = true;
                                    result.data = htm;
                                    break;
                                case "v8":
                                    try
                                    {
                                        V8ScriptEngine engine = new V8ScriptEngine(V8ScriptEngineFlags.DisableGlobalMembers);
                                        engine.AddCOMType("XMLHttpRequest", "MSXML2.XMLHTTP");
                                        engine.Execute(@" function get(url) { var xhr = new XMLHttpRequest(); xhr.open('GET', url, false); xhr.send(); if (xhr.status == 200) return xhr.responseText; else return ''; }");
                                        htm = engine.Script.get(url);
                                        engine.Dispose();

                                        result.ok = true;
                                        result.data = htm;
                                    }
                                    catch (Exception e1)
                                    {
                                        //htm = e1.Message;
                                        result.error = "ERROR_XHR_" + function.ToUpper() + ": " + e1.Message;
                                        return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                                    }
                                    break;
                                default:
                                    using (var httpClient = new HttpClient())
                                    {
                                        foreach (var header in headers)
                                        {
                                            if (header.Key.StartsWith("Content") || header.Key == "type" || header.Key == "data" || header.Key == "url" || header.Key == "method") continue;
                                            if (header.Value != null) httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                                        }

                                        //============================================================
                                        //ServicePointManager.CertificatePolicy = new MyPolicy();
                                        //ServicePointManager.Expect100Continue = true;
                                        //ServicePointManager.DefaultConnectionLimit = 9999;
                                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;

                                        //============================================================
                                        HttpResponseMessage responseMessage = null;
                                        switch (httpMethod)
                                        {
                                            case "DELETE":
                                                responseMessage = httpClient.DeleteAsync(url).Result;
                                                break;
                                            case "PATCH":
                                            case "POST":
                                                string data = string.Empty;
                                                if (para.ContainsKey("data") && para[data] != null) data = para["data"].ToString();
                                                responseMessage = httpClient.PostAsync(url, new StringContent(data)).Result;
                                                break;
                                            case "GET":
                                                responseMessage = httpClient.GetAsync(url).Result;
                                                break;
                                        }

                                        if (responseMessage != null)
                                            using (responseMessage)
                                            using (var content = responseMessage.Content)
                                                htm = content.ReadAsStringAsync().Result.Trim();
                                        if (htm.Length > 0)
                                        {
                                            result.ok = true;
                                            result.data = htm;
                                        }
                                    }
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            result.error = "ERROR_THROW_" + function.ToUpper() + ": " + e.Message;
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }
                    }

                    #endregion
                    break;
                case "curl_call":
                    #region [ curl_call ]

                    #endregion
                    break;
                case "html_export_links":
                    #region [ html_export_links ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        if (s.Length > 0)
                        {
                            List<oLinkItem> list = new List<oLinkItem>();

                            // 1. Find all matches in file.
                            MatchCollection m1 = Regex.Matches(s, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

                            // 2. Loop over each match.
                            foreach (Match m in m1)
                            {
                                string value = m.Groups[1].Value;
                                oLinkItem i = new oLinkItem() { html = m.ToString() };

                                // 3. Get href attribute.
                                Match m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);
                                if (m2.Success)
                                    i.href = m2.Groups[1].Value;

                                // 4. Remove inner tags from text.
                                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                                i.text = t;

                                list.Add(i);
                            }

                            result.data = list;
                            result.ok = true;
                        }
                    }

                    #endregion
                    break;
                case "html_export_images":
                    #region [ html_export_images ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        if (s.Length > 0)
                        {
                            List<oImageItem> list = new List<oImageItem>();
                            try
                            {
                                HtmlDocument doc = new HtmlDocument();
                                doc.LoadHtml(s); // or doc.Load(htmlFileStream)
                                                 //var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");
                                var nodes = doc.DocumentNode.SelectNodes(@"//img");
                                if (nodes != null)
                                {
                                    foreach (var img in nodes)
                                    {
                                        var dic = new Dictionary<string, string>();
                                        foreach (var attr in img.Attributes)
                                            dic.Add(attr.Name, attr.Value);
                                        list.Add(new oImageItem() { html = img.OuterHtml, attrs = dic });
                                    }
                                }
                                result.data = list;
                                result.ok = true;
                            }
                            catch (Exception e)
                            {
                                result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                                return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                            }
                        }
                    }

                    #endregion
                    break;
                case "html_to_text_01":
                    #region [ html_to_text_01 ]

                    /*
                     * POST /api/js/test
                        {
	                        "___scope": "test",
	                        "___api": "crawler",
	                        "headers": {
		                        "url": "https://vnexpress.net/chu-tich-vcci-noi-long-phong-toa-la-goi-kich-thich-kinh-te-lon-nhat-4087759.html",
		                        "method": "GET"
	                        },
                            "remove_first": "<h1 class=\"title-detail\">|</h1>",
                            "remove_end": "</article",
	                        "data": ""
                        }
                     */

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        if (s.Length > 0)
                        {
                            var mtit = Regex.Match(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            string title = string.Empty;
                            if (mtit.Success)
                            {
                                title = "[TITLE] " + mtit.Groups["Title"].Value.Trim() + Environment.NewLine;
                                s = Regex.Replace(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", string.Empty, RegexOptions.Singleline);
                            }

                            List<oLinkItem> list = new List<oLinkItem>();
                            #region [ LINK ]

                            // 1. Find all matches in file.
                            MatchCollection m1 = Regex.Matches(s, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

                            // 2. Loop over each match.
                            foreach (Match m in m1)
                            {
                                string value = m.Groups[1].Value;
                                oLinkItem i = new oLinkItem() { html = m.ToString() };

                                // 3. Get href attribute.
                                Match m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);
                                if (m2.Success)
                                    i.href = m2.Groups[1].Value;

                                // 4. Remove inner tags from text.
                                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                                i.text = t;

                                list.Add(i);
                                s = s.Replace(m.ToString(), " " + i.id + " ");
                            }

                            #endregion

                            #region [ IMG ]

                            //List<oImageItem> list_img = new List<oImageItem>();
                            //HtmlDocument doc = new HtmlDocument();
                            //string htm = s;
                            //if (htm.Contains("<html") == false) htm = "<html>" + htm;
                            //doc.LoadHtml(htm); // or doc.Load(htmlFileStream)
                            ////var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");
                            //var nodes = doc.DocumentNode.SelectNodes(@"//img");
                            //if (nodes != null)
                            //{
                            //    foreach (var img in nodes)
                            //    {
                            //        var dic = new Dictionary<string, string>();
                            //        foreach (var attr in img.Attributes)
                            //            dic.Add(attr.Name, attr.Value);
                            //        var i = new oImageItem() { html = img.OuterHtml, attrs = dic };
                            //        list_img.Add(i);
                            //        s = s.Replace(i.html, " " + i.id + " ");
                            //    }
                            //}

                            #endregion

                            s = s
                                //.Replace("<a", " [A] <a").Replace("</a", " [/A]</a")
                                //.Replace("<img", " [IMG] <img")

                                .Replace("<article", "^[ARTICLE] <article").Replace("</article", "^[/ARTICLE]</article")

                                .Replace("<div", "^<div")
                                .Replace("<p", "^<p")

                                .Replace("<table", "^[TABLE] <table").Replace("</table", "^[/TABLE]</table")
                                .Replace("<tr", "^[TR] <tr").Replace("</tr", "^[/TR]</tr")
                                //.Replace("<td", " || <td")

                                .Replace("<ul", "^[UL] <ul").Replace("</ul", "^[/UL]</ul")
                                .Replace("<li", "^[LI] <li")
                                .Replace("<h1", "^[H1] <h1")
                                .Replace("<h2", "^[H2] <h2")
                                .Replace("<h3", "^[H3] <h3")
                                .Replace("<h4", "^[H4] <h4")
                                .Replace("<h5", "^[H5] <h5")
                                .Replace("<h6", "^[H6] <h6")

                                .Replace("<br", "^<br");

                            foreach (var a in list) s = s.Replace(a.id, a.ToString());

                            //### Remove any tags but not there content "<p>bob<span> johnson</span></p>" -> "bob johnson"
                            // Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
                            s = Regex.Replace(s, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", string.Empty, RegexOptions.Singleline);

                            s = s.Replace("^", Environment.NewLine);
                            s = Regex.Replace(s, @"[\r\n]{2,}", "\r\n");
                            s = string.Join(Environment.NewLine + Environment.NewLine,
                                s.Split(new char[] { '\r', '\n' }).Select(x => x.Trim()).Where(x => x.Length > 0));

                            if (para.ContainsKey("remove_first") && para["remove_first"] != null)
                            {
                                string remove_first = para.getValueByKey("remove_first").Trim();
                                if (!string.IsNullOrWhiteSpace(remove_first))
                                {
                                    //var mtit = Regex.Match(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    //string title = string.Empty;
                                    //if (mtit.Success) title = "[TITLE] " + mtit.Groups["Title"].Value.Trim() + Environment.NewLine;

                                    int pos = -1;
                                    string split_text;
                                    string[] a = remove_first.Split('|');
                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        split_text = a[i];
                                        pos = s.IndexOf(split_text);
                                        if (pos != -1) s = s.Substring(pos + split_text.Length, s.Length - pos - split_text.Length).Trim();
                                    }

                                    //s = title + s;
                                }
                            }

                            if (para.ContainsKey("remove_end") && para["remove_end"] != null)
                            {
                                string remove_end = para.getValueByKey("remove_end").Trim();
                                if (!string.IsNullOrWhiteSpace(remove_end))
                                {
                                    int pos = -1;
                                    string split_text;
                                    string[] a = remove_end.Split('|');
                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        split_text = a[i];
                                        pos = s.IndexOf(split_text);
                                        if (pos != -1) s = s.Substring(0, pos).Trim();
                                    }
                                }
                            }

                            s = title + s.Trim();

                            //if (list_img.Count > 0) s = Environment.NewLine + "<==IMG==>" + Environment.NewLine + JsonConvert.SerializeObject(list_img);
                        }
                        result.data = s;
                        result.ok = true;
                    }

                    #endregion
                    break;
                case "html_clean_01":
                    #region [ html_clean_01 ]

                    /*
                     * POST /api/js/test
                        {
	                        "___scope": "test",
	                        "___api": "crawler",
	                        "headers": {
		                        "url": "https://vnexpress.net/chu-tich-vcci-noi-long-phong-toa-la-goi-kich-thich-kinh-te-lon-nhat-4087759.html",
		                        "method": "GET"
	                        },
                            "remove_first": "<h1 class=\"title-detail\">|</h1>",
                            "remove_end": "</article",
	                        "data": ""
                        }
                     */

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        if (s.Length > 0)
                        {
                            s = new Regex(@"<script[^>]*>[\s\S]*?</script>").Replace(s, string.Empty);
                            s = new Regex(@"<style[^>]*>[\s\S]*?</style>").Replace(s, string.Empty);
                            s = new Regex(@"<noscript[^>]*>[\s\S]*?</noscript>").Replace(s, string.Empty);
                            s = Regex.Replace(s, @"<meta(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            s = Regex.Replace(s, @"<link(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            s = Regex.Replace(s, @"<use(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            s = Regex.Replace(s, @"<figure(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            s = Regex.Replace(s, @"<!DOCTYPE(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            s = Regex.Replace(s, @"<!--(.|\n)*?-->", string.Empty, RegexOptions.Singleline);

                            s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");

                            if (para.ContainsKey("remove_first") && para["remove_first"] != null)
                            {
                                string remove_first = para.getValueByKey("remove_first").Trim();
                                if (!string.IsNullOrWhiteSpace(remove_first))
                                {
                                    var mtit = Regex.Match(s, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    string title = string.Empty;
                                    if (mtit.Success) title = "[TITLE] " + mtit.Groups["Title"].Value.Trim() + Environment.NewLine;

                                    int pos = -1;
                                    string split_text;
                                    string[] a = remove_first.Split('|');
                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        split_text = a[i];
                                        pos = s.IndexOf(split_text);
                                        if (pos != -1) s = s.Substring(pos + split_text.Length, s.Length - pos - split_text.Length).Trim();
                                    }

                                    s = title + s;
                                }
                            }

                            if (para.ContainsKey("remove_end") && para["remove_end"] != null)
                            {
                                string remove_end = para.getValueByKey("remove_end").Trim();
                                if (!string.IsNullOrWhiteSpace(remove_end))
                                {
                                    int pos = -1;
                                    string split_text;
                                    string[] a = remove_end.Split('|');
                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        split_text = a[i];
                                        pos = s.IndexOf(split_text);
                                        if (pos != -1) s = s.Substring(0, pos).Trim();
                                    }
                                }
                            }
                        }
                        result.data = s;
                        result.ok = true;
                    }

                    #endregion
                    break;
                case "html_remove_comment":
                    #region [ html_remove_comment ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("headers") == false)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [headers] not exist";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }
                    }

                    #endregion
                    break;
                case "html_remove_tag_simple":
                    #region [ html_remove_tag_simple ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }
                        if (para.ContainsKey("tags") == false || para["tags"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [tags] is not null or empty and it has format { tags: '!DOCTYPE,use,figure,meta,link,...' }";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        string[] tags = para.getValueByKey("tags").Trim().Split(',').Select(x => x.Trim().ToLower()).ToArray();
                        if (s.Length > 0 && tags.Length > 0)
                        {
                            for (int i = 0; i < tags.Length; i++)
                                s = new Regex(@"<" + tags[i] + @"(.|\n)*?>").Replace(s, string.Empty);
                            s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");

                            //s = Regex.Replace(s, @"<meta(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            //s = Regex.Replace(s, @"<link(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            //s = Regex.Replace(s, @"<use(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            //s = Regex.Replace(s, @"<figure(.|\n)*?>", string.Empty, RegexOptions.Singleline);
                            //s = Regex.Replace(s, @"<!DOCTYPE(.|\n)*?>", string.Empty, RegexOptions.Singleline);

                        }
                        result.data = s;
                        result.ok = true;
                    }

                    #endregion
                    break;
                case "html_remove_tag_content":
                    #region [ html_remove_tag_content ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }
                        if (para.ContainsKey("tags") == false || para["tags"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [tags] is not null or empty and it has format { tags: 'script,style,h1,a,p,...' }";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        string[] tags = para.getValueByKey("tags").Trim().Split(',').Select(x => x.Trim().ToLower()).ToArray();
                        if (s.Length > 0 && tags.Length > 0)
                        {
                            for (int i = 0; i < tags.Length; i++)
                                s = new Regex(@"<" + tags[i] + @"[^>]*>[\s\S]*?</" + tags[i] + @">").Replace(s, string.Empty);

                            //s = new Regex(@"<script[^>]*>[\s\S]*?</script>").Replace(s, string.Empty);
                            //s = new Regex(@"<style[^>]*>[\s\S]*?</style>").Replace(s, string.Empty);
                            //s = new Regex(@"<noscript[^>]*>[\s\S]*?</noscript>").Replace(s, string.Empty);

                            s = Regex.Replace(s, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "\r\n");
                        }
                        result.data = s;
                        result.ok = true;
                    }

                    #endregion
                    break;
                case "html_remove_tag_keep_content":
                    #region [ html_remove_tag_keep_content ]

                    if (para.Count > 0)
                    {
                        if (para.ContainsKey("html") == false || para["html"] == null)
                        {
                            result.error = "ERROR_" + function.ToUpper() + ": The paramenter [html] is not null or empty";
                            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
                        }

                        string s = para.getValueByKey("html").Trim();
                        if (s.Length > 0)
                        {
                            //### Remove any tags but not there content "<p>bob<span> johnson</span></p>" -> "bob johnson"
                            // Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
                            s = Regex.Replace(s, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", string.Empty, RegexOptions.Singleline);
                        }
                        result.data = s;
                        result.ok = true;
                    }

                    #endregion
                    break;
                case "notify_user":
                    #region [ notify_user ]

                    #endregion
                    break;
                case "notify_broadcast":
                    #region [ notify_broadcast ]

                    #endregion
                    break;
                case "api_list":
                    #region [ api_list ]

                    #endregion
                    break;
                case "api_get":
                    #region [ api_get ]

                    #endregion
                    break;
                case "api_reload":
                    #region [ api_reload ]

                    #endregion
                    break;
                case "api_reload_all":
                    #region [ api_reload_all ]

                    #endregion
                    break;
                case "api_exist":
                    #region [ api_exist ]

                    #endregion
                    break;
                case "file_read_text":
                    #region [ file_read_text ]

                    #endregion
                    break;
                case "file_write_text":
                    #region [ file_write_text ]

                    #endregion
                    break;
                case "file_append_text":
                    #region [ file_append_text ]

                    #endregion
                    break;
                case "file_exist":
                    #region [ file_exist ]

                    #endregion
                    break;
                case "file_delete":
                    #region [ file_delete ]

                    #endregion
                    break;
                case "dir_get_files":
                    #region [ dir_get_files ]

                    #endregion
                    break;
                case "dir_exist":
                    #region [ dir_exist ]

                    #endregion
                    break;
                case "dir_create":
                    #region [ dir_create ]

                    #endregion
                    break;
                case "dir_delete":
                    #region [ dir_delete ]

                    #endregion
                    break;
                case "cache_addnew":
                    #region [ cache_addnew ]

                    #endregion
                    break;
                case "cache_update":
                    #region [ cache_update ]

                    #endregion
                    break;
                case "cache_remove":
                    #region [ cache_remove ]

                    #endregion
                    break;
                case "cache_clear_all":
                    #region [ cache_clear_all ]

                    #endregion
                    break;
                case "cache_runtime_exist":
                    #region [ cache_runtime_exist ]

                    #endregion
                    break;
                case "cache_runtime_set":
                    #region [ cache_runtime_set ]

                    #endregion
                    break;
                case "cache_runtime_get":
                    #region [ cache_runtime_get ]

                    #endregion
                    break;
                case "cache_runtime_remove":
                    #region [ cache_runtime_remove ]

                    #endregion
                    break;
                case "cache_search":
                    #region [ cache_search ]

                    #endregion
                    break;
                case "cache_get_item_by_id":
                    #region [ cache_get_item_by_id ]

                    #endregion
                    break;
                case "cache_get_items_by_ids":
                    #region [ cache_get_items_by_ids ]

                    #endregion
                    break;
                case "db_execute":
                    #region [ db_execute ]

                    #endregion
                    break;
                case "job_list":
                    #region [ job_list ]

                    #endregion
                    break;
                case "job_create":
                    #region [ job_create ]

                    #endregion
                    break;
                case "job_stop":
                    #region [ job_stop ]

                    #endregion
                    break;
                case "job_start":
                    #region [ job_start ]

                    #endregion
                    break;
                case "job_remove":
                    #region [ job_remove ]

                    #endregion
                    break;
            }

            //return JavaScriptValue.Invalid;
            return JavaScriptValue.FromString(JsonConvert.SerializeObject(result));
        }

        public static oResult run_api(Dictionary<string, object> request = null)
        {
            oResult rs = new oResult() { ok = false, request = request };
            if (request == null || request.Count == 0)
            {
                rs.error = "Cannot found data of request";
                return rs;
            }

            string scope = string.Empty;

            if (string.IsNullOrEmpty(request.getValueByKey("___api"))
                && string.IsNullOrEmpty(request.getValueByKey("___fun")))
            {
                rs.error = "[___api] or [___fun] is null or empty";
                return rs;
            }

            string js = string.Empty,
                api_name = string.Empty;

            #region [ JS: api or function ]

            if (string.IsNullOrEmpty(request.getValueByKey("___api")) == false)
            {
                scope = request.getValueByKey("___scope");
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
                oApi o = clsApi.Get(scope);
                if (o.apis.Length == 0)
                {
                    rs.error = "folder " + scope + " missing files api: " + sapi.Replace("|", ".js; ") + ".js";
                    return rs;
                }
                sapi = sapi.ToLower().Trim();
                string[] a = sapi.Split('|');
                for (var i = 0; i < a.Length; i++)
                {
                    if (o.apis_data.ContainsKey(a[i]) == false)
                    {
                        rs.error = "folder " + scope + " missing files api: " + a[i] + ".js";
                        return rs;
                    }
                    js += o.apis_data[a[i]] + Environment.NewLine + Environment.NewLine;
                }
                api_name = string.Join(",", a);
            }
            else if (string.IsNullOrEmpty(request.getValueByKey("___fun")) == false)
            {
                api_name = request.getValueByKey("___fun");
                js = "var rs_ = ___api." + api_name + "(JSON.stringify(___para));" + Environment.NewLine;
                js += "return rs_;" + Environment.NewLine + Environment.NewLine;
            }

            #endregion

            StringBuilder bi = new StringBuilder();

            bi.Append("(()=>{ ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___scope = 'API-JS." + scope + "." + api_name + "'; ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___config = " + JsonConvert.SerializeObject(new
            {
                PATH_ROOT = _CONFIG.PATH_ROOT,
                LOG_PORT_REDIS = _CONFIG.LOG_PORT_REDIS,
                PATH_DATA_FILE = _CONFIG.PATH_DATA_FILE
            }) + ";");
            bi.Append(Environment.NewLine);
            bi.Append("var ___log = function(key,text){ ___api.log(___scope, key, text); }; ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___api_call = function(func, obj){ var p = JSON.stringify(obj); var v = ___api[func](p); var rs = JSON.parse(v); return rs; }; ");
            bi.Append(Environment.NewLine);
            bi.Append("var ___para = " + JsonConvert.SerializeObject(request));
            bi.Append(Environment.NewLine);
            bi.Append(Environment.NewLine);

            bi.Append(js);

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

                    rs.type = DATA_TYPE.JSON_RESPONSE;
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
    public class MyPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint,
          X509Certificate certificate, WebRequest request,
          int certificateProblem)
        {
            //Return True to force the certificate to be accepted.
            return true;
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