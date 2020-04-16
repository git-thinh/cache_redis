using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ckv_aspnet
{
    public static class clsExtObject
    {
        public static T getValue<T>(this Dictionary<string, object> dic, string key)
        {
            try
            {
                if (dic != null && dic.ContainsKey(key))
                {
                    var p = (T)dic[key];
                    return p;
                }
            }
            catch { }

            T v = default(T);
            return v;
        }

        public static Dictionary<string, object> getParaInput(this IJobExecutionContext context)
        {
            try
            {
                if (context != null)
                {
                    JobDataMap dataMap = context.JobDetail.JobDataMap;
                    if (dataMap.ContainsKey("PARA___"))
                    {
                        var p = (Dictionary<string, object>)dataMap["PARA___"];
                        return p;
                    }
                }
            }
            catch { }

            return null;
        }

        public static void log(this IJobExecutionContext context, string key, params object[] paras)
        {
            try
            {
                if (context != null)
                {
                    if (context.Scheduler.Context.ContainsKey("ILOG___"))
                    {
                        string server_name = string.Empty,
                            id = string.Empty,
                            schedule = string.Empty,
                            scope_name = string.Empty,
                            current_id = string.Empty,
                            para_text = string.Empty;

                        int counter = 0;

                        ILogJob log = (ILogJob)context.Scheduler.Context.Get("ILOG___");

                        if (context.Scheduler.Context.ContainsKey("SCOPE_NAME___"))
                            scope_name = context.Scheduler.Context.GetString("SCOPE_NAME___");

                        JobDataMap dataMap = context.JobDetail.JobDataMap;

                        // id = [group_name].[date_time_create = yyMMddHHmmss].[guid_id...]
                        if (dataMap.ContainsKey("ID___")) id = dataMap.Get("ID___").ToString();
                        if (dataMap.ContainsKey("CURRENT_ID___")) current_id = dataMap.Get("CURRENT_ID___").ToString();
                        if (dataMap.ContainsKey("SCHEDULE___")) schedule = dataMap.Get("SCHEDULE___").ToString();

                        if (dataMap.ContainsKey("PARA___"))
                        {
                            var p = dataMap["PARA___"];
                            if (p == null) para_text = "NULL";
                            else
                            {
                                switch (p.GetType().Name)
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
                                        para_text = p.ToString();
                                        break;
                                    default:
                                        para_text = JsonConvert.SerializeObject(p, Formatting.Indented);
                                        break;
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(para_text))
                            para_text += Environment.NewLine + "----------------------------" + Environment.NewLine;

                        if (dataMap.ContainsKey("COUNTER___"))
                        {
                            var state = (ConcurrentDictionary<long, bool>)dataMap["COUNTER___"];
                            counter = state.Count;
                        }

                        StringBuilder bi = new StringBuilder();
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
                                        string s = JsonConvert.SerializeObject(paras[i], Formatting.Indented);
                                        bi.Append(s);
                                        bi.Append(Environment.NewLine);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string s = JsonConvert.SerializeObject(paras, Formatting.Indented);
                            bi.Append(s);
                            bi.Append(Environment.NewLine);
                            bi.Append("ERROR_LOG: " + ex.Message);
                        }

                        string 
                            l_scope = scope_name + "." + id,
                            l_key = DateTime.Now.ToString("yyMMdd-HHmmss-fff") + "." + key;

                        string text =
                            "LOG_ID: " + l_key + Environment.NewLine +
                            "JOB_ID: " + id + Environment.NewLine +
                            "CURRENT_ID: " + current_id + Environment.NewLine +
                            "SCHEDULE: " + schedule + Environment.NewLine +
                            "COUNT: " + counter.ToString() + Environment.NewLine +
                            "----------------------------" + Environment.NewLine +
                            para_text + bi.ToString();

                        log.write(l_scope, l_key, text);
                    }
                }
            }
            catch { }
        }

        public static T getValueByKey<T>(this ConcurrentDictionary<string, object> dic, string key)
        {
            if (dic != null && key != null && dic.ContainsKey(key))
            {
                try
                {
                    object val = dic[key];
                    if (val != null) return (T)val;
                }
                catch { }
            }
            var v = default(T);
            return v;
        }

        public static T getValueByKey<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic != null && key != null && dic.ContainsKey(key))
            {
                try
                {
                    object val = dic[key];
                    if (val != null) return (T)val;
                }
                catch { }
            }
            var v = default(T);
            return v;
        }

        public static string getValueByKey(this Dictionary<string, object> dic, string key)
        {
            if (dic != null && key != null && dic.ContainsKey(key))
            {
                try
                {
                    object val = dic[key];
                    if (val != null) return val.ToString();
                }
                catch { }
            }

            return null;
        }
    }
}