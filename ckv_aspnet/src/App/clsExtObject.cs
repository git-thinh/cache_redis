using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ckv_aspnet
{
    public static class clsExtObject
    {
        public static void log(this IJobExecutionContext context, string key, params object[] paras)
        {
            try
            {
                if (context != null)
                {
                    if (context.Scheduler.Context.ContainsKey("ILOG___"))
                    {
                        string server_name = string.Empty,
                            date_time = string.Empty,
                            group_name = string.Empty,
                            job_name = string.Empty,
                            para_text = string.Empty;

                        int counter = 0;

                        ILogJob log = (ILogJob)context.Scheduler.Context.Get("ILOG___");
                        if(context.Scheduler.Context.ContainsKey("SERVER_NAME___"))
                            server_name = context.Scheduler.Context.Get("SERVER_NAME___").ToString();

                        JobDataMap dataMap = context.JobDetail.JobDataMap;

                        if (dataMap.ContainsKey("DATE_TIME___")) date_time = dataMap.Get("DATE_TIME___").ToString();
                        if (dataMap.ContainsKey("GROUP_NAME___")) group_name = dataMap.Get("GROUP_NAME___").ToString();
                        if (dataMap.ContainsKey("JOB_NAME___")) job_name = dataMap.Get("JOB_NAME___").ToString();

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

                        if (dataMap.ContainsKey("COUNTER___"))
                        {
                            IList<DateTimeOffset> state = (IList<DateTimeOffset>)dataMap["COUNTER___"];
                            counter = state.Count;
                            //state.Add(DateTimeOffset.UtcNow);
                        }
                         



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

    }
}