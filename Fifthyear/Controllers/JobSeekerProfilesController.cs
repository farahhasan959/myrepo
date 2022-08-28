using Fifthyear.Data;
using Fifthyear.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Fifthyear.Controllers
{
    public class JobSeekerProfilesController : ControllerBase
    {
        private readonly Context ctx;
        private readonly IHostingEnvironment _host;
        public JobSeekerProfilesController(Context ctx,IHostingEnvironment host)
        {
            this.ctx = ctx;
            _host=host;
            //_environment = hostingEnvironment;
        }
       // [HttpGet("api/jobseekerprofiles")]
        public async Task<ActionResult<IEnumerable<JobSeekerProfile>>> GetJobSeekerProfiles()
        {
            var Items = await ctx.JobSeekerProfiles
               .Include(t => t.JobSeeker)
               .ThenInclude(t => t.jobSeekerEducations)
               .Include(e => e.JobSeeker)
                .ThenInclude(p => p.jobSeekerSkills)
                .Include(e => e.JobSeeker)
                .ThenInclude(p => p.previousJobs)
                .Include(e => e.JobSeeker)
                .ThenInclude(p => p.jobSeekerLanguages)
                 .Include(e => e.JobSeeker)
                .ThenInclude(p => p.jobSeekerWorkExperiences)
                .ToListAsync();
            return Ok(Items);
        }
        [HttpGet("api/jobseekerprofiles/{id}")]
        public async Task<ActionResult<SeekerProfileReturnedModel>> GetProfileById(int id)
        {
            var item = await ctx.JobSeekerProfiles
               .Where(item => item.JobSeekerID == id)
               .Include(t => t.JobSeeker)
               .ThenInclude(t => t.jobSeekerEducations)
               .Include(e => e.JobSeeker)
                .ThenInclude(p => p.jobSeekerSkills)
                .Include(e => e.JobSeeker)
                .ThenInclude(p => p.previousJobs)
                .Include(e => e.JobSeeker)
                .ThenInclude(p => p.jobSeekerLanguages)
                 .Include(e => e.JobSeeker)
                .ThenInclude(p => p.jobSeekerWorkExperiences)
               .FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound("This seeker is not found");
            }
            SeekerProfileReturnedModel vacancyReturnModel = new SeekerProfileReturnedModel();


            vacancyReturnModel.Name = item.JobSeeker.Name;
            vacancyReturnModel.Gender = item.JobSeeker.Gender;
            vacancyReturnModel.age = item.JobSeeker.age;
            vacancyReturnModel.ImageFile = item.ImageFile;

            vacancyReturnModel.Bio = item.Bio;
            vacancyReturnModel.Email = item.ProfileEmail;
            foreach (var i in item.JobSeeker.jobSeekerWorkExperiences)
            {
                VacancyWorkExperienceModel vacancyWorkExperienceModel1 = new VacancyWorkExperienceModel();
                vacancyWorkExperienceModel1.Field = i.Field;
                vacancyWorkExperienceModel1.years = i.years;
                vacancyWorkExperienceModel1.Id = i.Id;
                vacancyReturnModel.jobSeekerWorkExperiences.Add(vacancyWorkExperienceModel1);

            }
            foreach (var i in item.JobSeeker.jobSeekerSkills)
            {
                VacancySkillModel vacancyLanguageModel = new VacancySkillModel();
                vacancyLanguageModel.Id = i.Id;
                vacancyLanguageModel.Name = i.Name;


                vacancyReturnModel.jobSeekerSkills.Add(vacancyLanguageModel);
            }
            foreach (var i in item.JobSeeker.jobSeekerEducations)
            {
                ReusmeEducationModel vacancyLanguageModel = new ReusmeEducationModel();
                vacancyLanguageModel.Id = i.Id;
                vacancyLanguageModel.Specialization = i.Specialization;
                vacancyLanguageModel.University = i.University;

                vacancyLanguageModel.Degree = i.Degree;

                vacancyReturnModel.jobSeekerEducations.Add(vacancyLanguageModel);

            }
            foreach (var i in item.JobSeeker.previousJobs)
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
            foreach (var i in item.JobSeeker.jobSeekerLanguages)
            {
                JobSeekerLanguageModel vacancyLanguageModel = new JobSeekerLanguageModel();
                vacancyLanguageModel.Id = i.Id;
                vacancyLanguageModel.Name = i.Name;
                vacancyLanguageModel.Level = i.Level;
                vacancyReturnModel.jobSeekerLanguages.Add(vacancyLanguageModel);


            }
            return Ok(vacancyReturnModel);
        }

       
        [HttpPut("api/jobseekerprofiles/{id}")]
        public async Task<ActionResult<SeekerProfileReturnedModel>> UpdateSeekerProfile

            ([FromBody] SeekerProfileReturnedModel jobSeekerProfile,[FromRoute] int id)
        {
            var item = await ctx.JobSeekerProfiles
                 .Where(item => item.JobSeekerID == id)
                 .Include(t => t.JobSeeker)
                 .ThenInclude(t => t.jobSeekerEducations)
                 .Include(e => e.JobSeeker)
                  .ThenInclude(p => p.jobSeekerSkills)
                  .Include(e => e.JobSeeker)
                  .ThenInclude(p => p.previousJobs)
                  .Include(e => e.JobSeeker)
                  .ThenInclude(p => p.jobSeekerLanguages)
                   .Include(e => e.JobSeeker)
                  .ThenInclude(p => p.jobSeekerWorkExperiences)
                 .FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }
           
                item.Bio = jobSeekerProfile.Bio;
            
          
                item.ProfileEmail = jobSeekerProfile.Email;
            await ctx.SaveChangesAsync();
            foreach (var i in jobSeekerProfile.jobSeekerEducations)
            {

                JobSeekerEducation jobSeekerEducation = new JobSeekerEducation();
                jobSeekerEducation.JobSeekerID = id;
                jobSeekerEducation.Specialization = i.Specialization;
                jobSeekerEducation.University = i.University;
                jobSeekerEducation.Degree = i.Degree;
                await ctx.jobSeekerEducations.AddAsync(jobSeekerEducation);
                await ctx.SaveChangesAsync();


            }
            foreach (var i in jobSeekerProfile.jobSeekerLanguages)
            {

                JobSeekerLanguage jobSeekerEducation = new JobSeekerLanguage();
                jobSeekerEducation.JobSeekerID = id;
                jobSeekerEducation.Name = i.Name;
                jobSeekerEducation.Level = i.Level;
                await ctx.jobSeekerLanguages.AddAsync(jobSeekerEducation);
                await ctx.SaveChangesAsync();


            }
            foreach (var i in jobSeekerProfile.jobSeekerSkills)
            {

                JobSeekerSkill jobSeekerEducation = new JobSeekerSkill();
                jobSeekerEducation.JobSeekerID = id;
                jobSeekerEducation.Name = i.Name;
                
                await ctx.jobSeekerSkills.AddAsync(jobSeekerEducation);
                await ctx.SaveChangesAsync();


            }
            foreach (var i in jobSeekerProfile.jobSeekerWorkExperiences)
            {

                JobSeekerWorkExperience jobSeekerEducation = new JobSeekerWorkExperience();
                jobSeekerEducation.JobSeekerID = id;
                jobSeekerEducation.Field = i.Field;
                jobSeekerEducation.years = i.years;

                await ctx.jobSeekerWorkExperiences.AddAsync(jobSeekerEducation);
                await ctx.SaveChangesAsync();


            }
            foreach (var i in jobSeekerProfile.previousJobs)
            {

                PreviousJob jobSeekerEducation = new PreviousJob();
                jobSeekerEducation.JobSeekerID = id;
                jobSeekerEducation.Company = i.Company;
                jobSeekerEducation.Start = i.Start;
                jobSeekerEducation.End = i.End;
                jobSeekerEducation.Positin = i.Positin;
                jobSeekerEducation.Freelancer = i.Freelancer;
                jobSeekerEducation.StillWorking = i.StillWorking;

                await ctx.previousJobs.AddAsync(jobSeekerEducation);
                await ctx.SaveChangesAsync();


            }



            return NoContent();

        }
        [HttpPut("api/jobseekerprofiles/image/{id}")]
        public async Task<ActionResult<string>> UpdateImageProfile

          ([FromBody] ImageEditModel img, [FromRoute] int id)
        {
            var profile = await ctx.JobSeekerProfiles
                .Where(item => item.JobSeekerID == id)
                .FirstOrDefaultAsync();


            if (profile == null)
            {
                return NotFound("This profile is not found");
            }
           
            //var mss = new MemoryStream();
            //img.ImageFile.CopyTo(mss);
            //var filebutes = img.ImageFile.ToArray();
            //string s = Convert.ToBase64String(img.ImageFile);
            //byte[] imageBytes = Convert.FromBase64String(s);
            MemoryStream ms = new MemoryStream(img.ImageFile, 0, img.ImageFile.Length);
            ms.Write(img.ImageFile, 0, img.ImageFile.Length);
            IFormFile formFile = new FormFile(ms, 0, ms.Length, profile.Bio, ".jpg");
           // Image image = Image.FromStream(ms, true);
            string newFileName = string.Empty;
            string fn = formFile.FileName;
            string extension = Path.GetExtension(fn);
            newFileName = Guid.NewGuid().ToString() + extension;
            string fileName = Path.Combine(_host.WebRootPath + "/images", newFileName);
            await formFile.CopyToAsync(new FileStream(fileName, FileMode.Create));

            profile.ImageFile = newFileName;
            await ctx.SaveChangesAsync();


            return newFileName;
        }
    
        [HttpDelete("api/profiles/education/{id}")]
        public async Task<IActionResult> Delete([FromBody] IEnumerable<VacancyIdModel> vacids, int id)
        {
            var item = await ctx.JobSeekerProfiles
                  .Where(item => item.JobSeekerID == id)
                  .Include(t => t.JobSeeker)
                  .ThenInclude(t => t.jobSeekerEducations)
                  .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerSkills)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.previousJobs)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerLanguages)
                    .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerWorkExperiences)
                  .FirstOrDefaultAsync();
            var branches = await ctx.jobSeekerEducations.Where(p => p.JobSeekerID == id).ToListAsync();
            foreach (var i in branches)
            {

                foreach (var item1 in vacids)
                {
                    if (i.Id == item1.Id)
                    {
                        var reusmes = await ctx.reusmes.Where(p => p.JobSeekerID == id)
                          .Include(e => e.jobSeeker)
                          .ThenInclude(e => e.jobSeekerEducations)
                          .ToListAsync();

                        foreach (var r in reusmes)
                        {
                            var works = await ctx.reusmeEducations.Where(t => t.ReusmeID == r.Id).ToListAsync();
                            foreach (var work in works)
                            {
                                if (work.Specialization == i.Specialization)
                                {
                                    ctx.reusmeEducations.Remove(work);
                                    await ctx.SaveChangesAsync();
                                }
                            }
                        }
                        ctx.jobSeekerEducations.Remove(i);
                        await ctx.SaveChangesAsync();

                    }
                }



            }
            return Ok();

        }
        [HttpDelete("api/profiles/skill/{id}")]
        public async Task<IActionResult> Deleteskill([FromBody] IEnumerable<VacancyIdModel> vacids, int id)
        {
            var item = await ctx.JobSeekerProfiles
                  .Where(item => item.JobSeekerID == id)
                  .Include(t => t.JobSeeker)
                  .ThenInclude(t => t.jobSeekerEducations)
                  .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerSkills)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.previousJobs)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerLanguages)
                    .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerWorkExperiences)
                  .FirstOrDefaultAsync();
            var branches = await ctx.jobSeekerSkills.Where(p => p.JobSeekerID == id).ToListAsync();
            foreach (var i in branches)
            {
                foreach (var item1 in vacids)
                {
                    if (i.Id == item1.Id)
                    {
                        var reusmes = await ctx.reusmes.Where(p => p.JobSeekerID == id)
                           .Include(e => e.jobSeeker)
                           .ThenInclude(e => e.jobSeekerSkills)
                           .ToListAsync();

                        foreach (var r in reusmes)
                        {
                            var works = await ctx.reusmeSkills.Where(t => t.ReusmeID == r.Id).ToListAsync();
                            foreach (var work in works)
                            {
                                if (work.Name == i.Name)
                                {
                                    ctx.reusmeSkills.Remove(work);
                                    await ctx.SaveChangesAsync();
                                }
                            }
                        }
                        ctx.jobSeekerSkills.Remove(i);
                        await ctx.SaveChangesAsync();

                    }
                }
            }
            return Ok();

        }
        [HttpDelete("api/profiles/language/{id}")]
        public async Task<IActionResult> Deletelan([FromBody] IEnumerable<VacancyIdModel> vacids, int id)
        {
            var item = await ctx.JobSeekerProfiles
                  .Where(item => item.JobSeekerID == id)
                  .Include(t => t.JobSeeker)
                  .ThenInclude(t => t.jobSeekerEducations)
                  .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerSkills)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.previousJobs)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerLanguages)
                    .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerWorkExperiences)
                  .FirstOrDefaultAsync();
            var branches = await ctx.jobSeekerLanguages.Where(p => p.JobSeekerID == id).ToListAsync();
            foreach (var i in branches)
            {
                foreach (var item1 in vacids)
                {
                    if (i.Id == item1.Id)
                    {
                        ctx.jobSeekerLanguages.Remove(i);
                        await ctx.SaveChangesAsync();

                    }
                }
            }
            return Ok();

        }
        [HttpDelete("api/profiles/experience/{id}")]
        public async Task<IActionResult> Deleteexp([FromBody] IEnumerable<VacancyIdModel> vacids, int id)
        {
            var item = await ctx.JobSeekerProfiles
                  .Where(item => item.JobSeekerID == id)
                  .Include(t => t.JobSeeker)
                  .ThenInclude(t => t.jobSeekerEducations)
                  .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerSkills)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.previousJobs)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerLanguages)
                    .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerWorkExperiences)
                  .FirstOrDefaultAsync();
            var worksofseeker = await ctx.jobSeekerWorkExperiences.Where(p => p.JobSeekerID == id).ToListAsync();
            foreach (var i in worksofseeker)
            {
                foreach (var item1 in vacids)
                {
                    if (i.Id == item1.Id)
                    {
                       
                        var reusmes = await ctx.reusmes.Where(p => p.JobSeekerID == id)
                            .Include(e=>e.jobSeeker)
                            .ThenInclude(e=>e.jobSeekerWorkExperiences)
                            .ToListAsync();
                     
                        foreach(var r in reusmes)
                        {
                            var works = await ctx.reusmeWorkExperiences.Where(t => t.ReusmeID == r.Id).ToListAsync();
                           foreach(var work in works)
                            {
                                if (work.Field == i.Field)
                                {
                                    ctx.reusmeWorkExperiences.Remove(work);
                                    await ctx.SaveChangesAsync();
                                }
                            }
                        }
                        ctx.jobSeekerWorkExperiences.Remove(i);
                        await ctx.SaveChangesAsync();

                    }
                }
            }
            return Ok();

        }
        [HttpDelete("api/profiles/previousjob/{id}")]
        public async Task<IActionResult> Deletepre([FromBody] IEnumerable<VacancyIdModel> vacids, int id)
        {
            var item = await ctx.JobSeekerProfiles
                  .Where(item => item.JobSeekerID == id)
                  .Include(t => t.JobSeeker)
                  .ThenInclude(t => t.jobSeekerEducations)
                  .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerSkills)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.previousJobs)
                   .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerLanguages)
                    .Include(e => e.JobSeeker)
                   .ThenInclude(p => p.jobSeekerWorkExperiences)
                  .FirstOrDefaultAsync();
            var branches = await ctx.previousJobs.Where(p => p.JobSeekerID == id).ToListAsync();
            foreach (var i in branches)
            {
                foreach (var item1 in vacids)
                {
                    if (i.Id == item1.Id)
                    {
                        ctx.previousJobs.Remove(i);
                        await ctx.SaveChangesAsync();

                    }
                }
            }
            return Ok();

        }
    }
}
