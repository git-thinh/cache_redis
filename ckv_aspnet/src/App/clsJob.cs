using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace ckv_aspnet
{
    public class clsJob
    {
        static ISchedulerFactory factory;
        static IScheduler scheduler;

        public static void _init()
        {
            factory = new StdSchedulerFactory(_config());
            scheduler = factory.GetScheduler().GetAwaiter().GetResult();

            //scheduler.ListenerManager.AddJobListener(new ExampleJobListener());
            //scheduler.ListenerManager.AddJobListener(new JobListenerExample(), EverythingMatcher<JobKey>.AllJobs());

            scheduler.Start().Wait();

            //scheduler.Context.Put("context_ioc", clsApi.api_list());
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
    }
}

