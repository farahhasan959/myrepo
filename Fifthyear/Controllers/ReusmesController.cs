using DinkToPdf;
using DinkToPdf.Contracts;
using Fifthyear.Data;
using Fifthyear.Models;
using Fifthyear.Services;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fifthyear.Controllers
{
    public class ReusmesController : ControllerBase
    {
        private readonly Context ctx;
        private readonly IConverter converter;
        private readonly IHostingEnvironment _host;
       
        public ReusmesController(Context ctx, IConverter converter, IHostingEnvironment _host)
        {
            this.ctx = ctx;
            this.converter = converter;
            this._host = _host;
           
        }
        
        [HttpGet("api/reusmes")]
        public async Task<ActionResult<IEnumerable<ReusmeReturnedModel>>> GetReusmes()
        {
            List<ReusmeReturnedModel> vacancyReturnModels = new List<ReusmeReturnedModel>();
            var Items = await ctx.reusmes
                .Include(o => o.reusmeSkills)
                .Include(o => o.reusmeEducations)
                .Include(o => o.reusmeWorkExperiences)
                .Include(e => e.jobSeeker)
                .ThenInclude(p => p.JobSeekerProfile)
                .Include(e => e.jobSeeker)
                .ThenInclude(p => p.previousJobs)
                 .Include(e => e.jobSeeker)
                .ThenInclude(p => p.jobSeekerLanguages)
                .ToListAsync();
            foreach (var item in Items)
            {
                ReusmeReturnedModel vacancyReturnModel = new ReusmeReturnedModel();
                vacancyReturnModel.Id = item.Id;
                vacancyReturnModel.Title = item.Title;
                vacancyReturnModel.Name = item.jobSeeker.Name;
                vacancyReturnModel.spicialization = item.spicialization;
                vacancyReturnModel.JobSeekerID = item.JobSeekerID;
                vacancyReturnModel.ImageFile = item.jobSeeker.JobSeekerProfile.ImageFile;
                vacancyReturnModel.age = item.jobSeeker.age;
                vacancyReturnModel.Bio = item.jobSeeker.JobSeekerProfile.Bio;
                vacancyReturnModel.Email = item.jobSeeker.JobSeekerProfile.ProfileEmail;
                foreach (var i in item.reusmeWorkExperiences)
                {
                    VacancyWorkExperienceModel vacancyWorkExperienceModel1 = new VacancyWorkExperienceModel();
                    vacancyWorkExperienceModel1.Field = i.Field;
                    vacancyWorkExperienceModel1.years = i.years;
                    vacancyWorkExperienceModel1.Id = i.Id;
                    vacancyReturnModel.reusmeWorkExperiences.Add(vacancyWorkExperienceModel1);

                }
                foreach (var i in item.reusmeSkills)
                {
                    VacancySkillModel vacancyLanguageModel = new VacancySkillModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Name = i.Name;


                    vacancyReturnModel.reusmeSkills.Add(vacancyLanguageModel);
                }
                foreach (var i in item.reusmeEducations)
                {
                    ReusmeEducationModel vacancyLanguageModel = new ReusmeEducationModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Specialization = i.Specialization;
                    vacancyLanguageModel.University = i.University;

                    vacancyLanguageModel.Degree = i.Degree;

                    vacancyReturnModel.reusmeEducations.Add(vacancyLanguageModel);

                }
                foreach (var i in item.jobSeeker.previousJobs)
                {
                    PreviousJobModel vacancyLanguageModel = new PreviousJobModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Company = i.Company;
                    vacancyLanguageModel.Positin = i.Positin;
                    vacancyLanguageModel.Start = i.Start;

                    vacancyLanguageModel.End = i.End;
                    vacancyLanguageModel.Freelancer = i.Freelancer;
                    vacancyLanguageModel.StillWorking = i.StillWorking;


                    vacancyReturnModel.previousJobs.Add(vacancyLanguageModel);

                }
                foreach (var i in item.jobSeeker.jobSeekerLanguages)
                {
                    JobSeekerLanguageModel vacancyLanguageModel = new JobSeekerLanguageModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Name = i.Name;
                    vacancyLanguageModel.Level = i.Level;



                    vacancyReturnModel.jobSeekerLanguages.Add(vacancyLanguageModel);

                }


                vacancyReturnModels.Add(vacancyReturnModel);
            }
            return Ok(vacancyReturnModels);
        }
        //[HttpGet("api/reusmes/filterbyspicialization/{spicialization}")]
        public async Task<ActionResult<IEnumerable<Reusme>>> GetFilteredReusmes(string spicialization)
        {
            var Items = await ctx.reusmes
                .Where(p => p.spicialization == spicialization)
                .Include(e => e.jobSeeker)
                .ThenInclude(e => e.JobSeekerProfile)
                .Include(d => d.reusmeSkills)

                .ToListAsync();
            return Ok(Items);
        }
        //[HttpGet("api/onereusmes/{id}")]
        public async Task<ActionResult<Reusme>> GetReuById(int id)
        {
            var reu = await ctx.reusmes
                .Where(item => item.Id == id)

                .Include(P => P.reusmeWorkExperiences)
                .Include(p => p.reusmeSkills)
                .Include(p => p.reusmeEducations)
                .Include(p => p.jobSeeker)
                .ThenInclude(p => p.JobSeekerProfile)
                .Include(p => p.jobSeeker)
                .ThenInclude(p => p.jobSeekerLanguages)
                .Include(p => p.jobSeeker)
                .ThenInclude(p => p.previousJobs)


                .FirstOrDefaultAsync();
            if (reu != null)
            {
                return Ok(reu);
            }
            return NotFound("This Reusme is not found");
        }
        [HttpGet("api/reusmes/{id}")]
        public async Task<ActionResult<IEnumerable<ReusmetoseekerModel>>> GetReusmesToSeeker([FromRoute] int id)
        {
            var Items = await ctx.reusmes
                .Where(p => p.JobSeekerID == id)
                .Include(p => p.reusmeEducations)
                .Include(p => p.reusmeSkills)
                .Include(p => p.reusmeWorkExperiences)
                .ToListAsync();

            List<ReusmetoseekerModel> vacancyReturnModels = new List<ReusmetoseekerModel>();
            foreach (var item in Items)
            {
                ReusmetoseekerModel vacancyReturnModel = new ReusmetoseekerModel();
                vacancyReturnModel.Id = item.Id;
                vacancyReturnModel.Title = item.Title;
                vacancyReturnModel.spicialization = item.spicialization;
                foreach (var i in item.reusmeWorkExperiences)
                {
                    VacancyWorkExperienceModel vacancyWorkExperienceModel1 = new VacancyWorkExperienceModel();
                    vacancyWorkExperienceModel1.Field = i.Field;
                    vacancyWorkExperienceModel1.years = i.years;
                    vacancyWorkExperienceModel1.Id = i.Id;
                    vacancyReturnModel.reusmeWorkExperiences.Add(vacancyWorkExperienceModel1);

                }
                foreach (var i in item.reusmeSkills)
                {
                    VacancySkillModel vacancyLanguageModel = new VacancySkillModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Name = i.Name;


                    vacancyReturnModel.reusmeSkills.Add(vacancyLanguageModel);
                }
                foreach (var i in item.reusmeEducations)
                {
                    ReusmeEducationModel vacancyLanguageModel = new ReusmeEducationModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Specialization = i.Specialization;
                    vacancyLanguageModel.University = i.University;

                    vacancyLanguageModel.Degree = i.Degree;

                    vacancyReturnModel.reusmeEducations.Add(vacancyLanguageModel);

                }
                vacancyReturnModels.Add(vacancyReturnModel);
            }
            return Ok(vacancyReturnModels);
        }
      

            [HttpGet("api/reusmes/pdf/{id}")]
        public async Task<ActionResult> CreateReusmePDF(int id)
        {
            // try {
            var resume = ctx.reusmes.Where(p => p.Id == id).FirstOrDefault();
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = _host.WebRootPath + "/pdffiles/" + resume.Title + ".pdf"
            };
            try
            {
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = GetHTMLString(id),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = _host.ContentRootPath + "/assets/styles.css" },
                };
                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var pdfFile = converter.Convert(pdf);
        }

           
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "myfile.txt", true);
        writer.WriteLine(DateTime.Now.ToLongDateString() + ": " + ex.Message);
                writer.Close();
            }
            
            return new PhysicalFileResult(_host.WebRootPath + "/pdffiles/" + resume.Title + ".pdf", "application/pdf");
        }
        public string GetHTMLString(int id)
        {
            var resume = ctx.reusmes
                .Where(b => b.Id == id)
                .Include(p => p.reusmeSkills)
                .Include(e => e.reusmeEducations)
                .Include(f => f.reusmeWorkExperiences)
                .Include(o => o.jobSeeker)
                .ThenInclude(h => h.jobSeekerLanguages)
                .Include(d => d.jobSeeker)
                .ThenInclude(a => a.JobSeekerProfile)
                .Include(d => d.jobSeeker)
                .ThenInclude(a => a.previousJobs)
                .FirstOrDefault();
            var jobs = ctx.previousJobs
             .Where(j => j.JobSeekerID == resume.JobSeekerID)
             .OrderBy(news => news.Start)
             .ToList();
            var sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html>
                        <html lang='en'>
                            <head>
                            <meta charset='UTF-8'>
                            <title>Resume/CV Design</title>");


           // var str = "<link rel ='stylesheet' type='text/css' href ='styles.css'>";
           // sb.Append(str);
            //            sb.Append(@"
            //                     <style>

            sb.Append(@"</head>
                            <body><div class='resume'>");
            sb.AppendFormat(@"
                          
                              <div class='resume_left'>
                               <div class='resume_profile'>
                               <img src='{0}' alt='profile_pic'>
                               </div>
                               <div class='resume_content'>
                               <div class='resume_item_w resume_info'>
                               <div class='title'>
                              
                               <p class='regular'>{1}</p>
                               </div>
                               <ul>
                               <li>                              
                               <div styles='margin-bottom: 20px;' class='data'>Email: {2}</div>
                               </li></ul></div>
                               <div class='resume_item_w resume_skills'>
                               <div class='title'>
                               <p class='bold'>skill's</p>
                               </div><ul>", _host.WebRootPath + "/images/" + resume.jobSeeker.JobSeekerProfile.ImageFile
                                         // , resume.jobSeeker.Name
                                          , resume.Title
                                          , resume.jobSeeker.Email);

            foreach (var skill in resume.reusmeSkills)
            {
                sb.AppendFormat(@"<li><div class='skill_name'>{0}</div></li>", skill.Name);
            }
            sb.Append(@"</ul></div><div  class='resume_item_w resume_skills'>
                               <div class='title'>
                               <p class='bold'>Language's</p>
                               </div><ul>");
            foreach (var language in resume.jobSeeker.jobSeekerLanguages)
            {
                sb.AppendFormat(@"<li><div class='skill_name'>{0} - {1}</div>
                              </li>", language.Name, language.Level);
            }
            sb.Append(@" 
</ul></div></div></div>

                          
                        ");
            sb.AppendFormat(@" 
              <div class='resume_right'><div class='resume_item resume_about'>
             <div class='title'><p class='b'>{0}</p></div><p class='semi'>{1}</p></div>
              <div class='resume_item'><div class='title'>
             <p class='bold'>Work Experience's</p></div><ul>                          
                        ", resume.jobSeeker.Name,
                        resume.jobSeeker.JobSeekerProfile.Bio);


            foreach (var experience in resume.reusmeWorkExperiences)
            {
                sb.AppendFormat(@"
                <li>
                <div class='date'>{0} for {1} years.</div> 
                
                </li>", experience.Field, experience.years);
            }
            sb.Append(@" 
                </ul>
                </div>
               <div class='resume_item resume_about'>
                  <div class='title'>
                      <p class='bold'>Education's</p>
                  </div>
                  <ul>                          
                      ");

            foreach (var education in resume.reusmeEducations)
            {
                sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} Degree in {1}</div> 
                    </li>"
                         , education.Degree, education.Specialization);
            }
            sb.Append(@" 
                </ul>
                </div>
               <div class='resume_item resume_work'>
                  <div class='title'>
                      <p class='bold'>Job's</p>
                  </div>
                  <ul>
                
                      ");

            foreach (var job in jobs)
            {
                if (job.Freelancer == false & job.StillWorking == false)
                {
                    sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} - {1} </div>
                  <div class='info'>
                     <p class='semi-bold'>{2}</p> 
                  <p>Works as {3}</p>
                </div>


                "
                             , job.Start.Value.Year.ToString(), job.End.Value.Year.ToString(), job.Company, job.Positin);
                }

                else if (job.Freelancer == true)
                {
                    sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} - {1} </div>
                  <div class='info'>
                     <p class='semi-bold'>Freelancer work</p> 
                  <p class='semi'>Works as {2}</p>
                </div>

                    </li>"
                            , job.Start.Value.Year.ToString(), job.End.Value.Year.ToString(), job.Positin);

                }



                else
                {
                    sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} - present </div>
                  <div class='info'>
                     <p class='semi-bold'>{1}</p> 
                  <p class='data'>Works as {2}</p>
                </div>

                    </li>"
                             , job.Start.Value.Year.ToString(), job.Company, job.Positin);
                }
            }


            sb.Append(@" 
             
              </ul></div>
                 </div>
                      </div>


                       </body>
                        </html>");

            return sb.ToString();
        }
        public string GetHTMLString2(int id)
        {
            var reusme = ctx.reusmes
                .Where(b => b.Id == id)
                .Include(p => p.reusmeSkills)
                .Include(e => e.reusmeEducations)
                .Include(f => f.reusmeWorkExperiences)
                .Include(o => o.jobSeeker)
                .ThenInclude(h => h.jobSeekerLanguages)
                .Include(d => d.jobSeeker)
                .ThenInclude(a => a.JobSeekerProfile)
                .Include(d => d.jobSeeker)
                .ThenInclude(a => a.previousJobs)
                .FirstOrDefault();
            var jobs = ctx.previousJobs
             .Where(j => j.JobSeekerID == reusme.JobSeekerID)
             .OrderBy(news => news.Start)
             .ToList();
            var sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html>
                        <html lang='en'>
                            <head>
                            <meta charset='UTF-8'>
                            <title>Resume/CV Design</title>");



            sb.Append(@"</ head >
                            < body style='background: #fff;
    font-size: 14px;
    line-height: 22px;
    color: #555555;'>< div style='width: 800px;
    height: auto;
    display: -webkit-inline-box;
    margin: 50px auto;
    page-break-after: always;'>");
            sb.AppendFormat(@"
                          
                              <div style='width: 280px;
        background: #FF9C00;'>
                               <div class='resume_profile'>
                               <img src='{0}' alt='profile_pic'>
                               </div>
                               <div style='padding: 0 25px;'>
                               <div style=' padding: 25px 0;
        border-bottom: 2px solid white;margin-bottom: 20px;'>
                               <div style='margin-bottom: 20px;'>
                               <p style='color: #fff;'>{1}</p>
                               <p style='color: #b1eaff;'>{2}</p>
                               </div>
                               <ul>
                               <li>                              
                               <div style='color: #b1eaff;'>Email: {3}</div>
                               </li></ul></div>
                               <div style='padding: 25px 0;
        border-bottom: 2px solid white;'>
                               <div style='margin-bottom: 20px;'>
                               <p style=' color: #fff;'>skill's</p>
                               </div><ul>", _host.WebRootPath + "/images/" + reusme.jobSeeker.JobSeekerProfile.ImageFile
                                          , reusme.jobSeeker.Name
                                          , reusme.Title
                                          , reusme.jobSeeker.Email);

            foreach (var skill in reusme.reusmeSkills)
            {
                sb.AppendFormat(@"<li><div style='width: 90%;'>{0}</div></li>", skill.Name);
            }
            sb.Append(@"</ul></div><div style='padding: 25px 0;
        border-bottom: 2px solid white;'>
                               <div style='margin-bottom: 20px;'>
                               <p style=' color: #fff;'>Language's</p>
                               </div><ul>");
            foreach (var language in reusme.jobSeeker.jobSeekerLanguages)
            {
                sb.AppendFormat(@"<li><div style='width: 90%;'>{0}  with  {1}  level</div>
                              </li>", language.Name, language.Level);
            }
            sb.Append(@" 
</ul></div></div></div>

                          
                        ");
            sb.AppendFormat(@" 
              <div style='width: 520px;
        background: #fff;
        padding: 25px;' ><div style='padding: 25px 0;
        border-bottom: 2px solid #FF9C00;'>
             <div style='margin-bottom: 20px;'><p class='bold'>About us</p></div><p>{0}</p></div>
              <div class='resume_item'><div class='title'>
             <p class='bold'>Work Experience's</p></div><ul>                          
                        ", reusme.jobSeeker.JobSeekerProfile.Bio);


            foreach (var experience in reusme.reusmeWorkExperiences)
            {
                sb.AppendFormat(@"
                <li>
                <div class='date'>{0} for {1} years.</div> 
                
                </li>", experience.Field, experience.years);
            }
            sb.Append(@" 
                </ul>
                </div>
               <div class='resume_item resume_about'>
                  <div class='title'>
                      <p class='bold'>Education's</p>
                  </div>
                  <ul>                          
                      ");

            foreach (var education in reusme.reusmeEducations)
            {
                sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} Degree in {1} from {2}</div> 
                    </li>"
                         , education.Degree, education.Specialization, education.University);
            }
            sb.Append(@" 
                </ul>
                </div>
               <div class='resume_item resume_work'>
                  <div class='title'>
                      <p class='bold'>Job's</p>
                  </div>
                  <ul>
                
                      ");

            foreach (var job in jobs)
            {
                if (job.Freelancer == false & job.StillWorking == false)
                {
                    sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} - {1} </div>
                  <div class='info'>
                     <p class='semi-bold'>{2}</p> 
                  <p class='semi-bold'>Works as {3}</p>
                </div>

                 
                "
                             , job.Start, job.End, job.Company, job.Positin);
                }

                else if (job.Freelancer == true)
                {
                //    sb.AppendFormat(@"
                //          <li>
                //    <div class='date'>{0} - {1} </div>
                //  <div class='info'>
                //     <p class='semi-bold'>{2}/p> 
                  
                //</div>

                //    </li>"
                //            , job.Start.Value.Year.ToString(), job.End.Value.Year.ToString(), job.Positin);

                }



                else
                {
                    sb.AppendFormat(@"
                          <li>
                    <div class='date'>{0} - present </div>
                  <div class='info'>
                     <p class='semi-bold'>{1}</p> 
                  <p class='semi-bold'>Works as {2}</p>
                </div>

                    </li>"
                             , job.Start, job.Company, job.Positin);
                }
            }


            sb.Append(@" 
             
              </ul></div>
                 </div>
                      </div>


                       </body>
                        </html>");

            return sb.ToString();
        }
     


    }
}
