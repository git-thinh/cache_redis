using ChakraHost.Hosting;
using Fleck2;
using Fleck2.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace cache_redis
{
    class Program
    {
        static readonly string ROOT_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static readonly string ROOT_PATH_UI = Path.Combine(ROOT_PATH, "ui");

        static oConfig m_config;
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(Program).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
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

        #region [ MEMORY CACHE ]

        static ConcurrentDictionary<string, string> m_01 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_02 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_03 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_04 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_05 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_06 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_07 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_08 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_09 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_10 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_11 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_12 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_13 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_14 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_15 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_16 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_17 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_18 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_19 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_20 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_21 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_22 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_23 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_24 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_25 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_26 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_27 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_28 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_29 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_30 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_31 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_32 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_33 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_34 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_35 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_36 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_37 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_38 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_39 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_40 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_41 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_42 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_43 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_44 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_45 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_46 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_47 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_48 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_49 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_50 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_51 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_52 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_53 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_54 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_55 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_56 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_57 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_58 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_59 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_60 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_61 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_62 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_63 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_64 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_65 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_66 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_67 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_68 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_69 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_70 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_71 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_72 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_73 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_74 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_75 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_76 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_77 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_78 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_79 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_80 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_81 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_82 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_83 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_84 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_85 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_86 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_87 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_88 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_89 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_90 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_91 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_92 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_93 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_94 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_95 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_96 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_97 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_98 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_99 = new ConcurrentDictionary<string, string>();

        static readonly string[] m_names = new string[99];
        static ConcurrentDictionary<string, string> m___get(string cache_name)
        {
            int index = -1;
            if (string.IsNullOrEmpty(cache_name)) return null;

            for (int i = 0; i < 100; i++) if (m_names[i] == cache_name) { index = i; break; }
            if (index == -1) return null;

            switch (index)
            {
                case 1: return m_01;
                case 2: return m_02;
                case 3: return m_03;
                case 4: return m_04;
                case 5: return m_05;
                case 6: return m_06;
                case 7: return m_07;
                case 8: return m_08;
                case 9: return m_09;
                case 10: return m_10;
                case 11: return m_11;
                case 12: return m_12;
                case 13: return m_13;
                case 14: return m_14;
                case 15: return m_15;
                case 16: return m_16;
                case 17: return m_17;
                case 18: return m_18;
                case 19: return m_19;
                case 20: return m_20;
                case 21: return m_21;
                case 22: return m_22;
                case 23: return m_23;
                case 24: return m_24;
                case 25: return m_25;
                case 26: return m_26;
                case 27: return m_27;
                case 28: return m_28;
                case 29: return m_29;
                case 30: return m_30;
                case 31: return m_31;
                case 32: return m_32;
                case 33: return m_33;
                case 34: return m_34;
                case 35: return m_35;
                case 36: return m_36;
                case 37: return m_37;
                case 38: return m_38;
                case 39: return m_39;
                case 40: return m_40;
                case 41: return m_41;
                case 42: return m_42;
                case 43: return m_43;
                case 44: return m_44;
                case 45: return m_45;
                case 46: return m_46;
                case 47: return m_47;
                case 48: return m_48;
                case 49: return m_49;
                case 50: return m_50;
                case 51: return m_51;
                case 52: return m_52;
                case 53: return m_53;
                case 54: return m_54;
                case 55: return m_55;
                case 56: return m_56;
                case 57: return m_57;
                case 58: return m_58;
                case 59: return m_59;
                case 60: return m_60;
                case 61: return m_61;
                case 62: return m_62;
                case 63: return m_63;
                case 64: return m_64;
                case 65: return m_65;
                case 66: return m_66;
                case 67: return m_67;
                case 68: return m_68;
                case 69: return m_69;
                case 70: return m_70;
                case 71: return m_71;
                case 72: return m_72;
                case 73: return m_73;
                case 74: return m_74;
                case 75: return m_75;
                case 76: return m_76;
                case 77: return m_77;
                case 78: return m_78;
                case 79: return m_79;
                case 80: return m_80;
                case 81: return m_81;
                case 82: return m_82;
                case 83: return m_83;
                case 84: return m_84;
                case 85: return m_85;
                case 86: return m_86;
                case 87: return m_87;
                case 88: return m_88;
                case 89: return m_89;
                case 90: return m_90;
                case 91: return m_91;
                case 92: return m_92;
                case 93: return m_93;
                case 94: return m_94;
                case 95: return m_95;
                case 96: return m_96;
                case 97: return m_97;
                case 98: return m_98;
                case 99: return m_99;
            }

            return null;
        }

        static void m___free_memory() {
            m_01.Clear();
            m_02.Clear();
            m_03.Clear();
            m_04.Clear();
            m_05.Clear();
            m_06.Clear();
            m_07.Clear();
            m_08.Clear();
            m_09.Clear();
            m_10.Clear();
            m_11.Clear();
            m_12.Clear();
            m_13.Clear();
            m_14.Clear();
            m_15.Clear();
            m_16.Clear();
            m_17.Clear();
            m_18.Clear();
            m_19.Clear();
            m_20.Clear();
            m_21.Clear();
            m_22.Clear();
            m_23.Clear();
            m_24.Clear();
            m_25.Clear();
            m_26.Clear();
            m_27.Clear();
            m_28.Clear();
            m_29.Clear();
            m_30.Clear();
            m_31.Clear();
            m_32.Clear();
            m_33.Clear();
            m_34.Clear();
            m_35.Clear();
            m_36.Clear();
            m_37.Clear();
            m_38.Clear();
            m_39.Clear();
            m_40.Clear();
            m_41.Clear();
            m_42.Clear();
            m_43.Clear();
            m_44.Clear();
            m_45.Clear();
            m_46.Clear();
            m_47.Clear();
            m_48.Clear();
            m_49.Clear();
            m_50.Clear();
            m_51.Clear();
            m_52.Clear();
            m_53.Clear();
            m_54.Clear();
            m_55.Clear();
            m_56.Clear();
            m_57.Clear();
            m_58.Clear();
            m_59.Clear();
            m_60.Clear();
            m_61.Clear();
            m_62.Clear();
            m_63.Clear();
            m_64.Clear();
            m_65.Clear();
            m_66.Clear();
            m_67.Clear();
            m_68.Clear();
            m_69.Clear();
            m_70.Clear();
            m_71.Clear();
            m_72.Clear();
            m_73.Clear();
            m_74.Clear();
            m_75.Clear();
            m_76.Clear();
            m_77.Clear();
            m_78.Clear();
            m_79.Clear();
            m_80.Clear();
            m_81.Clear();
            m_82.Clear();
            m_83.Clear();
            m_84.Clear();
            m_85.Clear();
            m_86.Clear();
            m_87.Clear();
            m_88.Clear();
            m_89.Clear();
            m_90.Clear();
            m_91.Clear();
            m_92.Clear();
            m_93.Clear();
            m_94.Clear();
            m_95.Clear();
            m_96.Clear();
            m_97.Clear();
            m_98.Clear();
            m_99.Clear();
        }

        #endregion

        #region [ ENGINE CHAKRA ]

        static JavaScriptRuntime runtime;
        static JavaScriptContext context;
        static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        static bool js___connected = false;
        static string js___libs_text = string.Empty;

        static void js___init()
        {
            js___connected = true;

            Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime);
            Native.JsCreateContext(runtime, out context);
            Native.JsSetCurrentContext(context);

            if (File.Exists("lib.js")) js___libs_text = File.ReadAllText("lib.js");
            if (js___libs_text.Length > 0)
                Native.JsRunScript(js___libs_text, currentSourceContext++, "", out JavaScriptValue r1);
        }

        static string js___index(string json)
        {
            if (!js___connected) js___init();

            using (new JavaScriptContext.Scope(context))
            {
                try
                {
                    JavaScriptValue result;
                    result = JavaScriptContext.RunScript("(()=>{ var o = " + json + "; \r\n return ___index(o); })()", currentSourceContext++, "");
                    string v = result.ConvertToString().ToString();
                    return v;
                }
                catch (Exception e)
                {
                }
            }
            return null;
        }

        static void js___search(string[] a)
        {
            List<long> ls = new List<long>() { };
            List<string> errs = new List<string>() { };
            try
            {
                if (!js___connected) js___init();

                if (a.Length > 0)
                {
                    string fn, filter;

                    using (new JavaScriptContext.Scope(context))
                    {
                        fn = "___" + Guid.NewGuid().ToString().Replace('-', '_');
                        //filter = " ___fn." + fn + " = function(o){ try { return o.id != null && o.id % 2 == 0; }catch(e){ return { ok: false, code: 1585035351111, id: o.id, message: e.message }; } }; ";
                        filter =
@" ___fn." + fn + @" = function(o){ 
    try { 
        return o.id != null && o.id % 2 == 0; 
    }catch(e){ 
        return { ok: false, code: 1585035351111, id: o.id, message: e.message }; 
    } 
};";
                        JavaScriptContext.RunScript(filter, currentSourceContext++, "");

                        for (var i = 0; i < a.Length; i++)
                        {
                            try
                            {
                                string js_exe =
@"(()=>{ 
    try { 
        var o = " + a[i] + @"; 
        var ok = ___fn." + fn + @"(o); 
        if(ok == true) 
            return o.id; 
        else if(ok == false) 
            return -1; 
        else 
            return JSON.stringify(ok); 
    } catch(e) { 
        return JSON.stringify({ ok: false, code: 1585035452039, id: o.id, message: e.message }); 
    } 
})()";
                                var result = JavaScriptContext.RunScript(js_exe, currentSourceContext++, "");
                                string v = result.ConvertToString().ToString();
                                if (v.Length > 20) errs.Add(v);
                                else
                                {
                                    long id = -1;
                                    long.TryParse(v, out id);
                                    if (id != -1) ls.Add(id);
                                }
                            }
                            catch (Exception e)
                            {
                                errs.Add(a[i]);
                            }
                        }

                        if (string.IsNullOrEmpty(fn) == false) JavaScriptContext.RunScript(" delete ___fn." + fn, currentSourceContext++, "");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        static void js___free_memory()
        {
            try
            {
                // Dispose runtime
                Native.JsSetCurrentContext(JavaScriptContext.Invalid);
                Native.JsDisposeRuntime(runtime);
            }
            catch { }
        }

        #endregion

        #region [ API BASE ]

        static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (Directory.Exists(path) == false) return new string[] { };

            if (string.IsNullOrEmpty(searchPattern) || searchPattern == "*.*")
                return Directory.GetFiles(path, "*.*", searchOption).Select(x => x.ToLower()).ToArray();

            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(Directory.GetFiles(path, sp, searchOption).Select(x => x.ToLower()));
            files.Sort();
            return files.ToArray();
        }

        static Stream api___stream_string(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void api___response_stream(string _extension, Stream input, HttpListenerContext context)
        {
            try
            {
                //Stream input = new FileStream(filename, FileMode.Open);
                //Stream input = api___stream_string(data);

                //Adding permanent http response headers
                string contentType = HTTPServerUI.GetContentType(_extension);
                context.Response.ContentType = contentType;
                context.Response.ContentLength64 = input.Length;
                //context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                //context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

                byte[] buffer = new byte[1024 * 16];
                int nbytes;
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
                input.Close();

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }

        static void api___response_json_error(string message, HttpListenerContext context)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new oResponseJson().Error(message));
                Stream input = api___stream_string(json);
                api___response_stream(".json", input, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.OutputStream.Close();
        }

        static void api___response_json_ok(object obj, HttpListenerContext context)
        {
            try
            {
                string json;

                if (obj == null)
                    json = JsonConvert.SerializeObject(new oResponseJson().Ok());
                else
                    json = JsonConvert.SerializeObject(new oResponseJson().Ok());

                Stream input = api___stream_string(json);
                api___response_stream(".json", input, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.OutputStream.Close();
        }

        #endregion

        #region [ API PROCESS ]

        static bool api___redis_check_ready(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api.Length == 0)
            {
                api___response_json_error("Uri must be / " + (a.Length > 0 ? a[1] : "[ACTION_NAME]") + "/[CACHE_NAME]", context);
                return false;
            }

            if (m_redis.ContainsKey(api) == false)
            {
                api___response_json_error("Cache engine " + api + " not exist", context);
                return false;
            }

            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            if (cf == null)
            {
                api___response_json_error("Config " + api + " not exist", context);
                return false;
            }

            if (m_config.db_connect == null || m_config.db_connect.Count == 0 || m_config.db_connect.ContainsKey(cf.scope) == false)
            {
                api___response_json_error("db_connect not contain connectString " + cf.scope, context);
                return false;
            }

            if (m___get(cf.name) == null)
            {
                api___response_json_error("m_names not contain name of Cache Engine " + cf.name, context);
                return false;
            }

            return true;
        }

        static bool api___redis_reset(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];


            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_search(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];

            string[] keys = redis.Keys;
            int max = keys.Length;
            string[] rs = new string[max];
            for (var i = 0; i < max; i++)
                rs[i] = ASCIIEncoding.UTF8.GetString(redis.Get(keys[i]));

            //js___index(rs[0]);
            js___search(rs);

            //string json = "[" + string.Join(",", rs) + "]";
            //Stream input = api___stream_string(json);
            //api___response_stream(".json", input, context);

            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_get_top(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            var m_cache = m___get(cf.name);

            string[] keys = redis.Keys;
            int max = keys.Length < 10 ? keys.Length : 10;
            string[] rs = new string[max];
            
            for (var i = 0; i < max; i++)
                //rs[i] = ASCIIEncoding.UTF8.GetString(redis.Get(keys[i]));
                if (m_cache.ContainsKey(keys[i])) rs[i] = m_cache[keys[i]];

            string json = "[" + string.Join(",", rs) + "]";

            Stream input = api___stream_string(json);
            api___response_stream(".json", input, context);
            return true;
        }

        static bool api___redis_bgsave(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.BackgroundSave();
            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_save(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.Save();
            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_clean_all(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.FlushDb();
            api___response_json_ok(null, context);
            return true;
        }

        static bool api___redis_reload_db(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];

            string file_sql = Path.Combine(ROOT_PATH, "config\\sql\\" + cf.name + ".sql");
            if (File.Exists(file_sql) == false)
            {
                api___response_json_error("File " + file_sql + " not exist", context);
                return false;
            }
            string sql_select = File.ReadAllText(file_sql);

            redis.FlushDb();

            string json;
            bool existID = false;
            
            Dictionary<string, string> rows = new Dictionary<string, string>() { };
            var m_cache = m___get(cf.name);

            using (var cn = new SqlConnection(m_config.db_connect[cf.scope]))
            {
                cn.Open();
                try
                {
                    var cm = cn.CreateCommand();
                    cm.CommandText = sql_select;

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var columns = new string[reader.FieldCount];
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                columns[i] = reader.GetName(i).ToLower();
                                if (columns[i] == "id") existID = true;
                            }

                            int k = 0;
                            long id = 0;
                            while (reader.Read())
                            {
                                var dic = new Dictionary<string, object>();
                                for (var i = 0; i < reader.FieldCount; i++)
                                    dic.Add(columns[i], reader.GetValue(i));

                                id = k;
                                if (existID) long.TryParse(dic["id"].ToString(), out id);
                                json = JsonConvert.SerializeObject(dic);
                                
                                string ix = js___index(json);
                                if (!string.IsNullOrEmpty(ix)) json = ix;

                                rows.Add(id.ToString(), json);
                                if (m_cache != null) m_cache.TryAdd(id.ToString(), json);

                                k++;
                            }
                        }
                    }
                }
                catch (Exception e111)
                {

                }
                cn.Close();
            }

            redis.Set(rows);
            rows.Clear();

            api___response_json_ok(null, context);
            return true;
        }

        static readonly Func<HttpListenerContext, oRedisCmd[], bool> HTTP_API_PROCESS_CMD = (context, cmds) =>
        {

            return false;
        };

        static readonly Func<HttpListenerContext, bool> HTTP_API_PROCESS = (context) =>
        {
            string method = context.Request.HttpMethod,
                path = context.Request.Url.AbsolutePath,
                text = string.Empty, filename = path.Substring(1);
            string[] files, dirs;
            Stream input;

            switch (method)
            {
                #region [ GET ]
                case "GET":
                    filename = path.Substring(1);
                    input = api___stream_string("Cannot found the file: " + path);

                    switch (filename)
                    {
                        case "list.html":
                        case "list":
                            #region
                            dirs = Directory.GetDirectories(ROOT_PATH_UI);
                            files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);

                            files = files.Select(x => string.Format(@"<a href=""{0}"" target=_blank>{0}</a></br>", Path.GetFileName(x))).ToArray();
                            dirs = dirs.Select(x => string.Format(@"<a href=""{0}"" target=_blank>{0}</a></br>", Path.GetFileName(x))).ToArray();

                            text = string.Join(string.Empty, dirs) + string.Join(string.Empty, files) +
                                //@"<a href=""/config"">config</a></br>" +
                                @"<a href=""/config"" target=_blank>config</a></br>";

                            input = api___stream_string(text);
                            filename = "list.html";
                            break;
                        #endregion
                        case "config":
                        case "config.html":
                            #region
                            text = JsonConvert.SerializeObject(m_config);
                            input = api___stream_string(text);
                            api___response_stream(".json", input, context);
                            break;
                        #endregion
                        default:
                            string[] a = context.Request.Url.Segments;
                            string cmd = a.Length > 0 ? a[1] : "";

                            switch (cmd)
                            {
                                case "reload_db/": return api___redis_reload_db(context);
                                case "reset/": return api___redis_reset(context);
                                case "bgsave/": return api___redis_bgsave(context);
                                case "save/": return api___redis_save(context);
                                case "clean_all/": return api___redis_clean_all(context);
                                case "top/": return api___redis_get_top(context);
                                case "search/": return api___redis_search(context);
                            }

                            #region

                            if (string.IsNullOrEmpty(filename))
                            {
                                files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);
                                string fileIndex = files.Where(x => x.EndsWith("index.htm") || x.EndsWith("index.html")).SingleOrDefault();
                                if (fileIndex != null) filename = Path.GetFileName(fileIndex);
                            }

                            filename = Path.Combine(ROOT_PATH_UI, filename);
                            if (File.Exists(filename))
                                input = new FileStream(filename, FileMode.Open);
                            else
                            {
                                if (Directory.Exists(filename))
                                {
                                    dirs = Directory.GetDirectories(filename);
                                    files = GetFiles(filename, "*.*", SearchOption.TopDirectoryOnly);

                                    files = files.Select(x => string.Format(@"<a href=""{0}/{1}"">{1}</a></br>", path, Path.GetFileName(x))).ToArray();
                                    dirs = dirs.Select(x => string.Format(@"<a href=""{0}/{1}"">{1}</a></br>", path, Path.GetFileName(x))).ToArray();

                                    text = string.Join(string.Empty, dirs) + string.Join(string.Empty, files);
                                    input = api___stream_string(text);
                                    filename = ".html";
                                }
                                else
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    context.Response.OutputStream.Close();
                                    return false;
                                }
                            }
                            #endregion
                            break;
                    }

                    //Console.WriteLine(path);
                    api___response_stream(Path.GetExtension(filename), input, context);

                    break;
                #endregion

                #region [ POST ]
                case "POST":
                    input = context.Request.InputStream;
                    if (input != null && input.Length > 0)
                    {
                        input.Position = 0;
                        using (StreamReader reader = new StreamReader(input, Encoding.UTF8))
                            text = reader.ReadToEnd();
                        oRedisCmd[] cmds = null;
                        try
                        {
                            cmds = JsonConvert.DeserializeObject<oRedisCmd[]>(text);
                            HTTP_API_PROCESS_CMD(context, cmds);
                        }
                        catch (Exception ex)
                        {
                            api___response_json_error("Error convert json input: " + ex.Message, context);
                            return false;
                        }
                    }
                    else
                    {
                        api___response_json_error("Body input is null", context);
                        return false;
                    }
                    break;
                    #endregion
            }
            return false;
        };

        #endregion

        #region [ FREE RESOURCE ]

        static int tcp___get_free_port()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        static HTTPServerUI http_api;
        static WebSocketServer server_ws;
        static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        static ConcurrentDictionary<string, Redis> m_redis
            = new ConcurrentDictionary<string, Redis>() { };
        static void app___free_resouce()
        {
            try
            {
                http_api.Stop();

                m_config.list_cache.ForEach(cf =>
                {
                    try { m_redis[cf.name].Dispose(); } catch (Exception e1) { }
                    try { cf.process.Kill(); } catch (Exception e2) { }
                });
                m_redis.Clear();

                allSockets.Clear();
                server_ws.Dispose();

                m___free_memory();
                js___free_memory();
            }
            catch (Exception err) { }
        }

        #endregion

        static void Main(string[] args)
        {
            #region [ CONFIG.JSON ]

            string file_config = Path.Combine(ROOT_PATH, "config.json");
            if (!File.Exists(file_config))
            {
                Console.WriteLine("Cannot find config.json");
                return;
            }

            try
            {
                string sconfig = File.ReadAllText(file_config);
                m_config = JsonConvert.DeserializeObject<oConfig>(sconfig);
            }
            catch (Exception e1)
            {
                Console.WriteLine("Error format JSON file config.json = ", e1.Message);
                return;
            }

            #endregion

            #region [ REDIS CACHE ]

            if (m_config != null && m_config.list_cache != null)
            {
                string file_conf_template = Path.Combine(ROOT_PATH, "redis.conf");
                if (File.Exists(file_conf_template) == false)
                {
                    Console.WriteLine("Cannot found the file: " + file_conf_template);
                    return;
                }
                string temp_conf = File.ReadAllText(file_conf_template);

                string path_conf = Path.Combine(ROOT_PATH, "config");
                if (Directory.Exists(path_conf) == false) Directory.CreateDirectory(path_conf);

                string PATH_DATA = Path.Combine(ROOT_PATH, "data");
                if (Directory.Exists(PATH_DATA) == false) Directory.CreateDirectory(PATH_DATA);

                m_config.busy = true;
                m_config.list_cache.ForEach(cf =>
                {
                    if (cf.id < 100 && cf.id > -1) m_names[cf.id] = cf.name;

                    if (cf.enable)
                    {
                        int port = tcp___get_free_port();
                        string file_redis = Path.Combine(ROOT_PATH, "redis-server.exe");
                        if (File.Exists(file_redis))
                        {
                            string file_conf = Path.Combine(path_conf, cf.name + ".conf");
                            if (File.Exists(file_conf)) File.Delete(file_conf);

                            string conf = temp_conf
                                .Replace("[IP]", "127.0.0.1")
                                .Replace("[PORT]", port.ToString())
                                .Replace("[DATA_FILE]", cf.name)
                                .Replace("[DATA_PATH]", PATH_DATA.Replace('\\', '/'));
                            File.WriteAllText(file_conf, conf);

                            Process p = new Process();
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.RedirectStandardInput = true;
                            p.StartInfo.FileName = file_redis;
                            //string argument = @" """ + file_conf + @""" --port " + port.ToString();
                            string argument = @" """ + file_conf + @"""";
                            p.StartInfo.Arguments = argument;
                            p.Start();

                            cf.process = p;
                            cf.port = port;
                            cf.ready = true;

                            //RedisDataAccessProvider redis = new RedisDataAccessProvider();
                            //redis.Configuration = new TeamDevRedis.LanguageItems.Configuration() { Host = "127.0.0.1", Port = port };
                            //redis.Connect();
                            var redis = new Redis("127.0.0.1", port);

                            if (m_redis.ContainsKey(cf.name))
                                m_redis.TryAdd(cf.name, redis);
                            else
                                m_redis[cf.name] = redis;

                            Console.WriteLine(" -> " + cf.name + " " + cf.port.ToString());
                        }
                    }
                });
                m_config.busy = false;
            }

            #endregion

            #region [ WEB_SOCKET ]

            int port_ws = tcp___get_free_port();
            FleckLog.Level = LogLevel.Error;
            server_ws = new WebSocketServer("ws://127.0.0.1:" + port_ws);
            server_ws.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
            m_config.port_ws = port_ws;

            #endregion

            #region [ TCP_PUSH ]


            #endregion

            #region [ HTTP_API ]

            if (!Directory.Exists(ROOT_PATH_UI)) Directory.CreateDirectory(ROOT_PATH_UI);
            http_api = new HTTPServerUI(ROOT_PATH_UI, m_config.port_api, HTTP_API_PROCESS);

            #endregion

            File.WriteAllText(file_config, JsonConvert.SerializeObject(m_config, Formatting.Indented));

            #region [ READ LINE ]

            string line = Console.ReadLine();
            while (line != "exit")
            {
                //foreach (var socket in allSockets.ToList()) socket.Send(line);
                switch (line)
                {
                    case "cls":
                    case "clean":
                    case "clear":
                        Console.Clear();
                        break;
                }
                line = Console.ReadLine();
            }

            Console.WriteLine("Program is closing... ");
            app___free_resouce();

            //string file_node = Path.Combine(ROOT_PATH, "node.exe");
            //if (File.Exists(file_node))
            //{
            //    Process node = new Process();
            //    node.StartInfo.UseShellExecute = false;
            //    node.StartInfo.RedirectStandardOutput = true;
            //    node.StartInfo.RedirectStandardError = true;
            //    node.StartInfo.RedirectStandardInput = true;
            //    node.StartInfo.FileName = file_node;
            //    string argument = @" --max-old-space-size=4096 app.js";
            //    node.StartInfo.Arguments = argument;
            //    node.Start();
            //}

            #endregion
        }
    }
}
