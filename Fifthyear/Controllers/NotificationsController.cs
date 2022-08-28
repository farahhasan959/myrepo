using CorePush.Google;
using Fifthyear.Data;
using Fifthyear.Models;
using Fifthyear.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Fifthyear.Controllers
{
    public class NotificationsController : ControllerBase
    {
        private readonly Context context;
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly INotificationService _notificationService;
        FirebaseApp defaultApp;
        private readonly FirebaseMessaging messaging;
        private IConfiguration _config;
        public NotificationsController(Context context, IConfiguration configuration)
        {
            this.context = context;
            _config = configuration;
            AppOptions appOp = new AppOptions() { Credential = GoogleCredential.FromFile(AppDomain.CurrentDomain.BaseDirectory + "hireme-c5c3a-firebase-adminsdk-raaqv-3a97010105.json") };
            FirebaseApp app;
            if (FirebaseApp.DefaultInstance == null)
                app = FirebaseApp.Create(appOp);
            else
                app = FirebaseApp.GetInstance(FirebaseApp.DefaultInstance.Name);
            messaging = FirebaseMessaging.GetMessaging(app);
        }
        public double sortOneReusmewithOneVacancy(int vacancyId, int reusmeId)
        {
            var vacancy = context.vacancies
                .Where(p => p.Id == vacancyId)
                .Include(s => s.vacancyWorkExperiences)
                .Include(s => s.VacancyLanguages)
                .Include(s => s.vacancySkills)
                .Include(s => s.vacancyEducations)
                .FirstOrDefault();
            var reusme = context.reusmes
                .Where(a => a.Id == reusmeId)
                .Include(s => s.reusmeEducations)
                .Include(s => s.reusmeSkills)
                .Include(s => s.reusmeWorkExperiences)
                .Include(s => s.jobSeeker)
                .ThenInclude(a => a.jobSeekerLanguages)
                .FirstOrDefault();
            double sum = 0.00;
            double i = 0.00;
            double ss = 0;
            double s = 0;
            foreach (var item in reusme.reusmeWorkExperiences)
            {
                foreach (var item1 in vacancy.vacancyWorkExperiences)
                {
                    if (item.Field.ToLower() == item1.Field.ToLower())
                    {
                        i = Convert.ToDouble(item1.years) / Convert.ToDouble(item.years);
                        if (i > 1) { i = 1; }
                        sum = sum + i;
                    }
                }
            }
            foreach (var item in reusme.reusmeSkills)
            {
                foreach (var item1 in vacancy.vacancySkills)
                {
                    if (item.Name.ToLower() == item1.Name.ToLower())
                    {
                        sum = sum + 1;
                    }

                }

            }

            foreach (var item in reusme.jobSeeker.jobSeekerLanguages)
            {
                foreach (var item1 in vacancy.VacancyLanguages)
                {
                    if (item.Name == item1.Name)
                    {
                        if (item.Level.ToLower() == "Advanced".ToLower())
                        {
                            if (item1.Level.ToLower() == "Advanced".ToLower())
                            {
                                ss = 1;
                                sum = sum + ss;
                            }
                            else if (item1.Level.ToLower() == "intermediate".ToLower())
                            {
                                ss = Convert.ToDouble(2) / Convert.ToDouble(3);
                                sum = sum + ss;
                            }
                            else
                            {
                                ss = Convert.ToDouble(1) / Convert.ToDouble(3);
                                sum = sum + ss;
                            }
                        }
                        else if (item.Level.ToLower() == "intermediate".ToLower())
                        {
                            if (item1.Level.ToLower() == "Beginner".ToLower())
                            {
                                ss = Convert.ToDouble(1) / Convert.ToDouble(2);
                                sum = sum + ss;
                            }
                            else
                            {
                                ss = 1;
                                sum = sum + ss;
                            }
                        }
                        else
                        {
                            ss = 1;
                            sum = sum + ss;
                        }
                    }


                }

            }

            foreach (var item in reusme.reusmeEducations)
            {
                foreach (var item1 in vacancy.vacancyEducations)
                {
                    if (item.Specialization == item1.Specialization)
                    {
                        if (item.Degree.ToLower() == "Doctorate".ToLower())
                        {
                            if (item1.Degree.ToLower() == "Doctorate".ToLower())
                            {
                                s = 1;
                                sum = sum + s;
                            }
                            else if (item1.Degree.ToLower() == "Master's degree".ToLower())
                            {
                                s = Convert.ToDouble(2) / Convert.ToDouble(3);
                                sum = sum + s;
                            }
                            else
                            {
                                s = Convert.ToDouble(1) / Convert.ToDouble(3);
                                sum = sum + s;
                            }

                        }
                        else if (item.Degree.ToLower() == "Master's degree".ToLower())
                        {
                            if (item1.Degree.ToLower() == "Bachelor's degree".ToLower())
                            {
                                s = Convert.ToDouble(1) / Convert.ToDouble(2);
                                sum = sum + s;
                            }
                            else
                            {
                                s = 1;
                                sum = sum + s;
                            }
                        }
                        else
                        {
                            s = 1;
                            sum = sum + s;

                        }

                    }


                }

            }
            if (vacancy.vacancyWorkExperiences.Count() == 0 && vacancy.vacancyEducations.Count() == 0
                && vacancy.VacancyLanguages.Count() == 0 && vacancy.vacancySkills.Count() == 0
                && reusme.reusmeWorkExperiences.Count() != 0 && reusme.reusmeEducations.Count() != 0
                && reusme.jobSeeker.jobSeekerLanguages.Count() != 0 && reusme.reusmeSkills.Count() != 0)
            {
                return 0;
            }
            else if (reusme.reusmeWorkExperiences.Count() == 0 && reusme.reusmeEducations.Count() == 0
                && reusme.jobSeeker.jobSeekerLanguages.Count() == 0 && reusme.reusmeSkills.Count() == 0
                )
            {
                return 100;

            }
            else
            {
                double avg = sum / Convert.ToDouble
                     (reusme.reusmeWorkExperiences.Count()
                    + reusme.reusmeEducations.Count()
                    + reusme.jobSeeker.jobSeekerLanguages.Count()
                    + reusme.reusmeSkills.Count());


                return avg * 100;
            }

        }
        public double sortOneVacancywithOneReusme(int vacancyId, int reusmeId)
        {
            var vacancy = context.vacancies
                .Where(p => p.Id == vacancyId)
                .Include(s => s.vacancyWorkExperiences)
                .Include(s => s.VacancyLanguages)
                .Include(s => s.vacancySkills)
                .Include(s => s.vacancyEducations)
                .FirstOrDefault();
            var reusme = context.reusmes
                .Where(a => a.Id == reusmeId)
                .Include(s => s.reusmeEducations)
                .Include(s => s.reusmeSkills)
                .Include(s => s.reusmeWorkExperiences)
                .Include(s => s.jobSeeker)
                .ThenInclude(a => a.jobSeekerLanguages)
                .FirstOrDefault();

            double sum = 0.00;
            double i = 0.00;
            double ss = 0;
            double s = 0;



            foreach (var item in vacancy.vacancyWorkExperiences)
            {
                foreach (var item1 in reusme.reusmeWorkExperiences)
                {
                    if (item.Field.ToLower() == item1.Field.ToLower())
                    {
                        if (item.years != 0)
                        {
                            i = Convert.ToDouble(item1.years) / Convert.ToDouble(item.years);
                            if (i > 1) { i = 1; }
                            sum = sum + i;
                        }
                        else
                        {
                            sum = sum + 1;
                        }
                    }

                }

            }


            foreach (var item in vacancy.vacancySkills)
            {
                foreach (var item1 in reusme.reusmeSkills)
                {
                    if (item.Name.ToLower() == item1.Name.ToLower())
                    {
                        sum = sum + 1;
                    }

                }

            }

            foreach (var item in vacancy.VacancyLanguages)
            {
                foreach (var item1 in reusme.jobSeeker.jobSeekerLanguages)
                {
                    if (item.Name.ToLower() == item1.Name.ToLower())
                    {
                        if (item.Level.ToLower() == "Advanced".ToLower())
                        {
                            if (item1.Level.ToLower() == "Advanced".ToLower())
                            {
                                ss = 1;
                                sum = sum + ss;
                            }
                            else if (item1.Level.ToLower() == "intermediate".ToLower())
                            {
                                ss = Convert.ToDouble(2) / Convert.ToDouble(3);
                                sum = sum + ss;
                            }
                            else
                            {
                                ss = Convert.ToDouble(1) / Convert.ToDouble(3);
                                sum = sum + ss;
                            }
                        }
                        else if (item.Level.ToLower() == "intermediate".ToLower())
                        {
                            if (item1.Level.ToLower() == "Beginner".ToLower())
                            {
                                ss = Convert.ToDouble(1) / Convert.ToDouble(2);
                                sum = sum + ss;
                            }
                            else
                            {
                                ss = 1;
                                sum = sum + ss;
                            }
                        }
                        else
                        {
                            ss = 1;
                            sum = sum + ss;
                        }
                    }


                }

            }

            foreach (var item in vacancy.vacancyEducations)
            {
                foreach (var item1 in reusme.reusmeEducations)
                {
                    if (item.Specialization == item1.Specialization)
                    {
                        if (item.Degree.ToLower() == "Doctorate".ToLower())
                        {
                            if (item1.Degree.ToLower() == "Doctorate".ToLower())
                            {
                                s = 1;
                                sum = sum + s;
                            }
                            else if (item1.Degree.ToLower() == "Master's degree".ToLower())
                            {
                                s = Convert.ToDouble(2) / Convert.ToDouble(3);
                                sum = sum + s;
                            }
                            else
                            {
                                s = Convert.ToDouble(1) / Convert.ToDouble(3);
                                sum = sum + s;
                            }

                        }
                        else if (item.Degree.ToLower() == "Master's degree".ToLower())
                        {
                            if (item1.Degree.ToLower() == "Bachelor's degree".ToLower())
                            {
                                s = Convert.ToDouble(1) / Convert.ToDouble(2);
                                sum = sum + s;
                            }
                            else
                            {
                                s = 1;
                                sum = sum + s;
                            }
                        }
                        else
                        {
                            s = 1;
                            sum = sum + s;

                        }

                    }


                }

            }
            if (vacancy.vacancyWorkExperiences.Count() != 0 && vacancy.vacancyEducations.Count() != 0
               && vacancy.VacancyLanguages.Count() != 0 && vacancy.vacancySkills.Count() != 0
               && reusme.reusmeWorkExperiences.Count() == 0 && reusme.reusmeEducations.Count() == 0
               && reusme.jobSeeker.jobSeekerLanguages.Count() == 0 && reusme.reusmeSkills.Count() == 0)
            {
                return 0;
            }
            else
            if (vacancy.vacancyWorkExperiences.Count() == 0 && vacancy.vacancyEducations.Count() == 0
                && vacancy.VacancyLanguages.Count() == 0 && vacancy.vacancySkills.Count() == 0
                )
            {
                return 100;

            }
            else
            {
                double avg = sum / Convert.ToDouble
                    (vacancy.vacancyWorkExperiences.Count()
                    + vacancy.vacancyEducations.Count()
                    + vacancy.VacancyLanguages.Count()
                    + vacancy.vacancySkills.Count());


                return avg * 100;
            }

        }
        [HttpPost("api/apply")]
        public async Task<ActionResult> CreateNotification

         ([FromBody] ApplyModel notification)
        {
            var not = await context.notifications.Where(p => p.companyId == notification.CompanyId && p.ReusmeID == notification.ReusmeID && p.VacancyId == notification.VacancyId).FirstOrDefaultAsync();
            if (not == null)
            {
                NotificationResponse notificationResponse = new NotificationResponse();
                notificationResponse.companyId = notification.CompanyId;
                notificationResponse.ReusmeID = notification.ReusmeID;
                notificationResponse.VacancyId = notification.VacancyId;
                double average = sortOneVacancywithOneReusme(notification.VacancyId, notification.ReusmeID);
                // double av = Convert.ToDouble(average);
                if (average > 60.00)
                {

                    await context.notifications.AddAsync(notificationResponse);
                    await context.SaveChangesAsync();


                    var company = await context.employeers.Where(p => p.Id == notification.CompanyId).FirstOrDefaultAsync();
                    if (company.FirebaseToken != null)
                    {
                        var message = new Message()
                        {

                            Notification = new FirebaseAdmin.Messaging.Notification
                            {
                                Title = "Someone Wants To Work For You",
                                Body = "There is someone applied to one of your vacancies"
                            },


                            Token = company.FirebaseToken,
                        };
                        string response = await messaging.SendAsync(message);
                    }
                    return Ok("your request has been sent, please wait");
                }
                return Ok("your resume doesn't match this vacancy");


            }

          
                return Ok("your request is already exist");
          

            
        }

        [HttpPost("api/suggest")]
        public async Task<ActionResult> CreateNotificationtoseeker

             ([FromBody] OfferModel notification)
        {
            var res = await context.reusmes
                    .Include(p => p.jobSeeker)
                    .Where(p => p.Id == notification.ReusmeID)
                    .FirstOrDefaultAsync();
            var seeker = await context.jobSeekers.Where(p => p.Id == res.JobSeekerID).FirstOrDefaultAsync();
            var not = await context.notifications.Where(p => p.seekerId == seeker.Id && p.ReusmeID == notification.ReusmeID && p.VacancyId == notification.VacancyId).FirstOrDefaultAsync();
            if (not == null)
            {
                NotificationResponse notificationResponse = new NotificationResponse();

            notificationResponse.seekerId = seeker.Id;
            notificationResponse.ReusmeID = notification.ReusmeID;
            notificationResponse.VacancyId = notification.VacancyId;
            double average = sortOneVacancywithOneReusme(notification.VacancyId, notification.ReusmeID);
            if (average > 60.00)
            {
                await context.notifications.AddAsync(notificationResponse);
                await context.SaveChangesAsync();
                if (seeker.FirebaseToken != null)
                {
                    var message = new Message()
                    {

                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = "A Company Wants You",
                            Body = "There is a company suggested a vacancy on you"
                        },


                        Token = seeker.FirebaseToken,
                    };
                    string response = await messaging.SendAsync(message);
                }
                return Ok("your request has been sent, please wait");
            }

            return Ok("your vacancy doesn't match this resume");
            }
            return Ok("your request is already exist");

        }
        [HttpGet("api/notttt")]
        public async Task<ActionResult> get() {
            
            var message = new Message()
            {

                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = "A Company Wants You",
                    Body = "There is a company suggested a vacancy on you"
                },


                Token = "eit36QYDofA3sfXIlvyj4v:APA91bG7tzRjyc2LcQqPyMiSLpNaOAFsXaBAO7Vmq0JlBGqtkRMhMGA5Xm42rhExhy6RFDiECN6KT_uQBGRFxBPwHR_GJxz3eG8FwnH-GptjcvhTxWGiCGAq8G3aCi1l83sMK0az-GHN",
            };
            string response = await messaging.SendAsync(message);
            return Ok();

        }
        [HttpGet("api/notificationsforcompany/{id}")]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetNotifications([FromRoute] int id)
        {
            var nots = await context.notifications.Where(p=>p.companyId==id).ToListAsync();
            return nots;
           

        }
        [HttpGet("api/notificationsforseeker/{id}")]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetNotificationss([FromRoute] int id)
        {
            var nots = await context.notifications.Where(p => p.seekerId == id).ToListAsync();
            return nots;


        }
        [HttpPut("api/notification/{id}")]
        public async Task<ActionResult> update([FromRoute] int id,[FromBody] SeenEdit isseen)
        {
            var not = await context.notifications.Where(p => p.Id == id).FirstOrDefaultAsync();
            not.IsSeen = isseen.IsSeen;
            await context.SaveChangesAsync();
            return Ok();


        }



    }

}
