using FlotaAppWorker.Data;
using FlotaAppWorker.Helpers;
using FlotaAppWorker.Interfaces;
using FlotaAppWorker.Jobs;
using FlotaAppWorker.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Net.Http;

namespace FlotaAppWorker
{
    public class Program
    {
       

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {

                        var cronSchedule = hostContext.Configuration["cronSchedule"];
                        q.UseMicrosoftDependencyInjectionJobFactory();
                        var jobKey = new JobKey("CheckingCars");

                        q.AddJob<CheckingCars>(opts => opts.WithIdentity(jobKey));
                        q.AddTrigger(opts => opts
                        .ForJob(jobKey)
                        .WithIdentity("CheckingCars-trigger")
                        .WithCronSchedule(cronSchedule, x=> x.InTimeZone(TimeZoneInfo.Local)));
                       
                    }
                        );
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                    
                    services.AddSingleton<ICarRepository, CarRepository>();
                    services.AddSingleton<IErrorEmailSender, ErrorEmailSender>();
                    services.AddSingleton<IEmailSender, EmailSender>();
                    services.AddSingleton<CheckingCars>();
                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<EmailSender>();
                    services.AddSingleton<DataContext>();
                });
    }
}
