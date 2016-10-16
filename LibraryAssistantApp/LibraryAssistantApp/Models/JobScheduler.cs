using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace LibraryAssistantApp.Models
{
    public class JobScheduler
    {
        public static void Start()
        {
            IJobDetail emailJob = JobBuilder.Create<EmailJob>()
                .WithIdentity("job1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                (s =>
                    s.WithIntervalInSeconds(30)
                    .OnEveryDay()
                    )
                    .ForJob(emailJob)
                    .WithIdentity("trigger1")
                    .StartNow()
                    .WithCronSchedule("0 15 10 * * ?")
                    .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(emailJob, trigger);
            sc.Start();
        }
    }
}