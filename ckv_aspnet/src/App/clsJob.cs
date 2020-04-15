using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
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


// CronTrigger Example 1 - an expression to create a trigger that simply fires every 5 minutes
"0 0/5 * * * ?"

// CronTrigger Example 2 - an expression to create a trigger that fires every 5 minutes, at 10 seconds after the minute (i.e. 10:00:10 am, 10:05:10 am, etc.).
"10 0/5 * * * ?"

// CronTrigger Example 3 - an expression to create a trigger that fires at 10:30, 11:30, 12:30, and 13:30, on every Wednesday and Friday.
"0 30 10-13 ? * WED,FRI"

// CronTrigger Example 4 - an expression to create a trigger that fires every half hour between the hours of 8 am and 10 am on the 5th and 20th of every month. Note that the trigger will NOT fire at 10:00 am, just at 8:00, 8:30, 9:00 and 9:30
"0 0/30 8-9 5,20 * ?"

//* Adding a JobListener that is interested in a particular job:
scheduler.ListenerManager.AddJobListener(myJobListener, KeyMatcher<JobKey>.KeyEquals(new JobKey("myJobName", "myJobGroup")));

//* Adding a JobListener that is interested in all jobs of a particular group:
scheduler.ListenerManager.AddJobListener(myJobListener, GroupMatcher<JobKey>.GroupEquals("myJobGroup"));

//* Adding a JobListener that is interested in all jobs of two particular groups:
scheduler.ListenerManager.AddJobListener(myJobListener, OrMatcher<JobKey>.Or(GroupMatcher<JobKey>.GroupEquals("myJobGroup"), GroupMatcher<JobKey>.GroupEquals("yourGroup")));

//* Adding a JobListener that is interested in all jobs:
scheduler.ListenerManager.AddJobListener(myJobListener, GroupMatcher<JobKey>.AnyGroup());

scheduler.ListenerManager.RemoveSchedulerListener(mySchedListener);

<schedule>

    <job>
        <name>JobOne</name>
        <group>JobOneGroup</group>
        <description>Sample job for Quartz Server</description>
        <job-type>SequentialQuartz_POC.JobOne, SequentialQuartz_POC</job-type>
        <durable>true</durable>
        <recover>false</recover>
    <job-data-map>
            <entry>
              <key>NextJobName</key>
              <value>JobTwo</value>
            </entry>
      </job-data-map>
    </job>

    <trigger>
      <simple>
        <name>sampleSimpleTrigger</name>
        <group>sampleSimpleGroup</group>
        <description>Simple trigger to simply fire sample job</description>
        <job-name>JobOne</job-name>
        <job-group>JobOneGroup</job-group>
        <misfire-instruction>SmartPolicy</misfire-instruction>
        <repeat-count>-1</repeat-count>
        <repeat-interval>100000</repeat-interval>
      </simple>
    </trigger>
  </schedule>

*/

namespace ckv_aspnet
{
    public class clsJob
    {
        static ISchedulerFactory factory;
        static IScheduler scheduler;

        public static void _init()
        {
            _init_test();
        }

        #region [ TEST ]

        public static void _init_test()
        {
            factory = new StdSchedulerFactory(_config_2());
            scheduler = factory.GetScheduler().GetAwaiter().GetResult();

            scheduler.ListenerManager.AddJobListener(new ExampleJobListener());
            scheduler.ListenerManager.AddJobListener(new JobListenerExample(), EverythingMatcher<JobKey>.AllJobs());

            scheduler.Start().Wait();

            scheduler.Context.Put("context_ioc", clsApi.api_list());
        }

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
            job.JobDataMap.Put("myStateData", new List<DateTimeOffset>());


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
            job.JobDataMap.Put("myStateData", new List<DateTimeOffset>());

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
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(9, 30)) // execute job daily at 9:30
                .Build();

            scheduler.ScheduleJob(job, trigger).Wait();
        }

        #endregion
    }

    #region [ TEST ]

    public class ExampleJobListener : IJobListener
    {
        public string Name
        {
            get { return "JobListenerName"; }
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Trace.WriteLine("Job Was Vetoed");

            return Task.FromResult(false);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Trace.WriteLine("Job to be Executed");

            return Task.FromResult(false);
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            Trace.WriteLine("Job was Executed");

            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// this listener class has methods which run before and after job execution
    /// </summary>
    public class JobListenerExample : IJobListener
    {
        /// <summary>
        /// to dismiss/ban/veto a job, we should return true from this method
        /// </summary>
        /// <param name="context"></param>
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            // this gets called before a job gets executed
            // by returning true from here we can basically prevent a job or all jobs from execution
            // Do nothing

            return Task.FromResult(false);
        }

        /// <summary>
        /// this gets called before a job is executed
        /// </summary>
        /// <param name="context"></param>
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Job {0} in group {1} is about to be executed", context.JobDetail.Key.Name, context.JobDetail.Key.Group);

            return Task.FromResult(false);
        }

        /// <summary>
        /// this gets called after a job is executed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobException"></param>
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Job {0} in group {1} was executed", context.JobDetail.Key.Name, context.JobDetail.Key.Group);

            // only run second job if first job was executed successfully
            if (jobException == null)
            {
                // fetching name of the job to be executed sequentially
                string nextJobName = Convert.ToString(context.MergedJobDataMap.GetString("NextJobName"));

                if (!string.IsNullOrEmpty(nextJobName))
                {
                    Console.WriteLine("Next job to be executed :" + nextJobName);
                    IJobDetail job = null;

                    // define a job and tie it to our JobTwo class
                    if (nextJobName == "JobTwo") // similarly we can write/handle cases for other jobs as well
                    {
                        job = JobBuilder.Create<JobTwo>()
                                .WithIdentity("JobTwo", "JobTwoGroup")
                                .Build();
                    }

                    // create a trigger to run the job now 
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("SimpleTrigger", "SimpleTriggerGroup")
                        .StartNow()
                        .Build();

                    // finally, schedule the job
                    if (job != null)
                        context.Scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    Console.WriteLine("No job to be executed sequentially");
                }
            }
            else
            {
                Console.WriteLine("An exception occured while executing job: {0} in group {1} with following details : {2}",
                    context.JobDetail.Key.Name, context.JobDetail.Key.Group, jobException.Message);
            }

            return Task.FromResult(false);
        }

        /// <summary>
        /// returns name of the listener
        /// </summary>
        public string Name
        {
            get { return "JobListenerExample"; }
        }
    }

    public class JobOne : IJob
    {
        /// <summary> 
        /// empty constructor for job initialization
        /// </summary>
        public JobOne()
        {
            // quartz requires a public empty constructor so that the
            // scheduler can instantiate the class whenever it needs.
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Job one started");
            // we can basically write code here for the stuff 
            // which we want our JobOne to do like inserting data into DB, etc.
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Job one finished");

            await Task.FromResult(false);
        }
    }

    public class JobTwo : IJob
    {
        /// <summary> 
        /// empty constructor for job initialization
        /// </summary>
        public JobTwo()
        {
            // quartz requires a public empty constructor so that the
            // scheduler can instantiate the class whenever it needs.
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Job two started");
            // we can basically write code here for the stuff 
            // which we want our JobOne to do like sending emails to users with reports as attachments, etc.
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Job two finished");

            await Task.FromResult(false);
        }
    }

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

            if (dataMap.ContainsKey("myStateData"))
            {
                IList<DateTimeOffset> state = (IList<DateTimeOffset>)dataMap["myStateData"];
                state.Add(DateTimeOffset.UtcNow);
            }

            await Task.FromResult(false);
        }
    }

    #endregion
}

