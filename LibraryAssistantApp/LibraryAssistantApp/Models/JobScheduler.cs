using Quartz;
using Quartz.Impl;
using System.Xml.Linq;
using System.IO;
using LibraryAssistantApp.Models;
using System.Linq;

namespace LibraryAssistantApp.Models
{
    public class JobScheduler
    {
        public static void Start()
        {
            //read XML
            XElement settings = XElement.Load(serverpath.path);
            //get time
            XElement scheduler = (from el in settings.Elements("scheduler")
                                  select el).FirstOrDefault();
            string time = scheduler.Element("time").Value;

            var hour = time.Substring(0, 2);
            var minutes = time.Substring(3, 2);


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
                    .WithCronSchedule("0 " + minutes + " "  + hour +" * * ?")
                    .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(emailJob, trigger);
            sc.Start();
        }
    }
}