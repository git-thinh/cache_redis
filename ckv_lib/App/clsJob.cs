using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ckv_lib
{
    public class clsJob
    {
        static ISchedulerFactory factory;
        static IScheduler scheduler;
        static ConcurrentDictionary<string, IJobDetail> m_job = new ConcurrentDictionary<string, IJobDetail>();
        static ConcurrentDictionary<string, ITrigger> m_trigger = new ConcurrentDictionary<string, ITrigger>();

        public static void _init()
        {
            factory = new StdSchedulerFactory(_config());
            scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            scheduler.ListenerManager.AddJobListener(new clsJobListener());
            scheduler.Start().Wait();
            scheduler.Context.Put("ILOG___", new clsLogJob());
            scheduler.Context.Put("SCOPE_NAME___", "CKV_JOB");
        }

        static NameValueCollection _config()
        {
            NameValueCollection configuration = new NameValueCollection
            {
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" },
                { "quartz.serializer.type", "binary" }
            };

            return configuration;
        }

        public static void create_schedule(string group_name) => create_schedule(group_name, "0/5 * * * * ?", null);

        public static void create_schedule(string group_name, string schedule = "0/15 * * * * ?", Dictionary<string, object> para = null)
        {
            if (para == null) para = new Dictionary<string, object>() { };
            group_name = group_name.ToLower();

            string name = group_name + "." + DateTime.Now.ToString("yyMMdd-HHmmss-fff");

            JobDataMap m = new JobDataMap();
            m.Put("ID___", name);
            m.Put("SCHEDULE___", schedule);
            m.Put("CURRENT_ID___", 0);
            m.Put("COUNTER___", new ConcurrentDictionary<long, bool>());
            m.Put("PARA___", para);

            IJobDetail j = JobBuilder.Create<clsJobItem>().WithIdentity(name, group_name)
                .UsingJobData(m)
                .Build();
            ITrigger t = TriggerBuilder.Create().WithIdentity(name, group_name)
                .WithSchedule(CronScheduleBuilder.CronSchedule(schedule))
                .StartNow()
                .Build();

            m_job.TryAdd(name, j);
            m_trigger.TryAdd(name, t);

            scheduler.ScheduleJob(j, t).Wait();
        }
    }

    public class clsJobItem : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var para = context.getParaInput();

            //var api = para.getValue<string>("api___");
            //if (string.IsNullOrWhiteSpace(api)) { 
            
            //}

            context.log("key_name_1", "This is content of JOB. It executing...");
            await Task.FromResult(false);
        }
    }

    public class clsJobListener : IJobListener
    {
        public string Name { get { return "clsJobListener"; } }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            if (dataMap.ContainsKey("CURRENT_ID___"))
            {
                long CURRENT_ID___ = long.Parse(DateTime.Now.ToString("yyMMddHHmmss"));
                dataMap.Put("CURRENT_ID___", CURRENT_ID___);

                if (dataMap.ContainsKey("COUNTER___"))
                {
                    var state = (ConcurrentDictionary<long, bool>)dataMap["COUNTER___"];
                    state.TryAdd(CURRENT_ID___, false);
                }
            }
            return Task.FromResult(false);
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            if (dataMap.ContainsKey("CURRENT_ID___"))
            {
                long CURRENT_ID___ = dataMap.GetLong("CURRENT_ID___");
                if (dataMap.ContainsKey("COUNTER___"))
                {
                    var state = (ConcurrentDictionary<long, bool>)dataMap["COUNTER___"];
                    state[CURRENT_ID___] = true;
                }
            }
            return Task.FromResult(false);
        }
    }
}

