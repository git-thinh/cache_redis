using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;

/*

0 0 12 * * ?		Fire at 12pm (noon) every day
0 15 10 ? * *		Fire at 10:15am every day
0 15 10 * * ?		Fire at 10:15am every day
0 15 10 * * ? *		Fire at 10:15am every day
0 15 10 * * ?		2005 Fire at 10:15am every day during the year 2005
0 * 14 * * ?		Fire every minute starting at 2pm and ending at 2:59pm, every day
0 0/5 14 * * ?		Fire every 5 minutes starting at 2pm and ending at 2:55pm, every day
0 0/5 14,18 * * ?	Fire every 5 minutes starting at 2pm and ending at 2:55pm, AND fire every 5 minutes starting at 6pm and ending at 6:55pm, every day
0 0-5 14 * * ?		Fire every minute starting at 2pm and ending at 2:05pm, every day
0 10,44 14 ? 3 WED	Fire at 2:10pm and at 2:44pm every Wednesday in the month of March.
0 15 10 ? * MON-FRI	Fire at 10:15am every Monday, Tuesday, Wednesday, Thursday and Friday
0 15 10 15 * ?		Fire at 10:15am on the 15th day of every month
0 15 10 L * ?		Fire at 10:15am on the last day of every month
0 15 10 L-2 * ?		Fire at 10:15am on the 2nd-to-last last day of every month
0 15 10 ? * 6L		Fire at 10:15am on the last Friday of every month
0 15 10 ? * 6L		Fire at 10:15am on the last Friday of every month
0 15 10 ? * 6L 2002-2005	Fire at 10:15am on every last friday of every month during the years 2002, 2003, 2004 and 2005
0 15 10 ? * 6#3		Fire at 10:15am on the third Friday of every month
0 0 12 1/5 * ?		Fire at 12pm (noon) every 5 days every month, starting on the first day of the month.
0 11 11 11 11 ?		Fire every November 11th at 11:11am.

*/

namespace ckv_aspnet
{
    public class clsJob
    {
        static ISchedulerFactory factory;
        static IScheduler scheduler;

        public static void _init()
        {
            factory = new StdSchedulerFactory(_config_2());
            scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().Wait();

            scheduler.Context.Put("context_ioc", clsApi.api_list());
        }
        
        #region [ TEST ]

        static NameValueCollection _config_1()
        {
            NameValueCollection configuration = new NameValueCollection
            {
                 { "quartz.scheduler.instanceName", "RemoteServer" },
                 { "quartz.scheduler.instanceId", "RemoteServer" },
                 { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                 { "quartz.jobStore.useProperties", "true" },
                 { "quartz.jobStore.dataSource", "default" },
                 { "quartz.jobStore.tablePrefix", "QRTZ_" },
                 { "quartz.dataSource.default.connectionString", "Server=(servername);Database=(datbasename);Trusted_Connection=true;" },
                 { "quartz.dataSource.default.provider", "SqlServer" },
                 { "quartz.threadPool.threadCount", "1" },
                 { "quartz.serializer.type", "binary" },
            };

            return configuration;
        }
        static NameValueCollection _config_2()
        {
            NameValueCollection configuration = new NameValueCollection
            {
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" },
                { "quartz.serializer.type", "binary" }
            };

            return configuration;
        }

        public static void test_create_job_1()
        {
            //StdSchedulerFactory factory = new StdSchedulerFactory(configuration);
            //IScheduler sched = await factory.GetScheduler();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<TestJob>()
                .WithIdentity($"HelloJob-{DateTime.Now}", "Group1")
                .UsingJobData("ServerName", "server-1")
                .UsingJobData("DateTime", DateTime.Now.ToString())
                .Build();

            job.JobDataMap.Put("para", clsApi.api_names());

            // Trigger the job to run now, and then only one time
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity($"Trigger at {DateTime.Now}", "group1")
              .StartNow()
              .Build();

            scheduler.ScheduleJob(job, trigger).Wait();
        }

        public static void test_create_job_2()
        {
            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<TestJob>()
                .WithIdentity("job1", "group1")
                .UsingJobData("ServerName", "server-1")
                .UsingJobData("DateTime", DateTime.Now.ToString())
                .Build();
            job.JobDataMap.Put("para", clsApi.api_names());

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger).Wait();

            //////// some sleep to show what's happening
            //////await Task.Delay(TimeSpan.FromSeconds(60));
            //////// and last shut down the scheduler when you are ready to close your program
            //////await scheduler.Shutdown();
        }

        public static void test_create_job_3()
        {
            IJobDetail inviteRequestProcessor = new JobDetailImpl("ProcessInviteRequest", null, typeof(InviteRequestJob));
            IDailyTimeIntervalTrigger trigger = new DailyTimeIntervalTriggerImpl(
                "Invite Request Trigger",
                TimeOfDay.HourMinuteAndSecondOfDay(0, 0, 0),
                TimeOfDay.HourMinuteAndSecondOfDay(23, 23, 59),
                IntervalUnit.Second,
                3);
            scheduler.ScheduleJob(inviteRequestProcessor, trigger);
        }

        public static void test_create_job_4()
        {
            JobDataMap m = new JobDataMap();
            m.Put("para", clsApi.api_names());

            IJobDetail job = JobBuilder.Create<TestJob>()
                .WithIdentity($"HelloJob-{DateTime.Now}", "Group1")
                .UsingJobData("ServerName", "server-1")
                .UsingJobData("DateTime", DateTime.Now.ToString())
                .UsingJobData(m)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("cronTrigger2", "group2")
                //https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontrigger.html
                .WithSchedule(CronScheduleBuilder.CronSchedule("0/3 * * * * ?"))
                .Build();

            scheduler.ScheduleJob(job, trigger).Wait();
        }

        #endregion
    }

    #region [ TEST ]

    public class InviteRequestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //RequestInvite.ProcessInviteRequests();
            await Task.FromResult(false);
        }
    }

    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var schedulerContext = context.Scheduler.Context;

            if (schedulerContext.ContainsKey("context_ioc"))
            {
                var context_ioc = (oApi[])schedulerContext.Get("context_ioc");
            }

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string serverName = dataMap.GetString("ServerName");
            string dateTime = dataMap.GetString("DateTime");
            string log = string.Format("Job started on {0} server at {1}", serverName, dateTime);

            if (dataMap.ContainsKey("para"))
            {
                var para = (string[])dataMap["para"];
            }

            await Task.FromResult(false);
        }
    }

    #endregion
}

