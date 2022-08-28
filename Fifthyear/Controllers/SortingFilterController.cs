using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fifthyear.Data;
using Fifthyear.Models;

namespace Fifthyear.Controllers
{
    public class SortingFilterController : ControllerBase
    {
        private readonly Context ctx;
        public SortingFilterController(Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("api/{vacancyId}")]
        public async Task<ActionResult<IEnumerable<ReusmeReturnedModel>>> sortOneVacancyWithAllReusmes(int vacancyId)
        {
            var reusmes = ctx.reusmes

                .Include(s => s.reusmeEducations)
                .Include(s => s.reusmeSkills)
                .Include(s => s.reusmeWorkExperiences)
                .Include(s => s.jobSeeker)
                .ThenInclude(a => a.jobSeekerLanguages)
                .ToList();
            var vacancy = ctx.vacancies
               .Where(p => p.Id == vacancyId)
               .Include(s => s.vacancyWorkExperiences)
               .Include(s => s.VacancyLanguages)
               .Include(s => s.vacancySkills)
               .Include(s => s.vacancyEducations)
               .FirstOrDefault();
            List<Tuple<int, double>> sorts = new List<Tuple<int, double>>();

            foreach (var item1 in reusmes)
            {
                var reusmeavg = sortOneVacancywithOneReusme(vacancyId, item1.Id);
                sorts.Add(new Tuple<int, double>(item1.Id, reusmeavg));
            }
            List<ReusmeReturnedModel> vacancyReturnModels = new List<ReusmeReturnedModel>();
            foreach (var reusme in sorts.OrderByDescending(t => t.Item2).ToList())
            {
                var item = ctx.reusmes
               .Where(s => s.Id == reusme.Item1)
             .Include(s => s.reusmeEducations)
             .Include(s => s.reusmeSkills)
             .Include(s => s.reusmeWorkExperiences)
             .Include(s => s.jobSeeker)
             .ThenInclude(a => a.jobSeekerLanguages)
             .Include(e => e.jobSeeker)
                .ThenInclude(p => p.JobSeekerProfile)
                .Include(e => e.jobSeeker)
                .ThenInclude(p => p.previousJobs)
             .FirstOrDefault();
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
        [HttpPost("api/{seekerid}")]
        public async Task<ActionResult<IEnumerable<VacancyReturnedToSeekerModel>>> sortOneReusmeWithAllVacancies(int seekerid, [FromBody] FilterModel filterModel)
        {
            var Allvacancies = ctx.vacancies
                  .Include(s => s.vacancyWorkExperiences)
                  .Include(s => s.VacancyLanguages)
                  .Include(s => s.vacancySkills)
                  .Include(s => s.vacancyEducations)
                  .Include(s=>s.employeer)
                  .ToList();
            List<Vacancy> vacancies = new List<Vacancy>();
            vacancies = Allvacancies;
            List<Vacancy> vacancies1 = new List<Vacancy>();
            if (filterModel.company != null)
            {
                vacancies = vacancies.Where(p => p.employeer.Name.ToLower().Contains(filterModel.company.ToLower())).ToList();
               
            }
            if (filterModel.spicialization != null)
            {
                vacancies = vacancies.Where(p => p.spicialization.ToLower().Contains(filterModel.spicialization.ToLower())).ToList();
       
            }
            if (filterModel.city != null)
            {
                
                var branches = await ctx.employeerBranches.Where(p => p.City.ToLower().Contains(filterModel.city.ToLower())).ToListAsync();


                foreach (var item in vacancies)
                {
                    foreach (var item1 in branches)
                    {
                        if (item.BranchId == item1.Id)
                        {
                            vacancies1.Add(item);
                        }

                    }
                }
                vacancies = vacancies1;
            }


            if (filterModel.reusmeId != -1)

            {
                var reusme = ctx.reusmes
                    .Where(p => p.Id == filterModel.reusmeId)
                    .Include(s => s.reusmeEducations)
                    .Include(s => s.reusmeSkills)
                    .Include(s => s.reusmeWorkExperiences)
                    .Include(s => s.jobSeeker)
                    .ThenInclude(a => a.jobSeekerLanguages)
                    .FirstOrDefault();
               
                List<Tuple<int, double>> sorts = new List<Tuple<int, double>>();

                foreach (var item1 in vacancies)
                {
                    var reusmeavg = sortOneReusmewithOneVacancy(item1.Id, filterModel.reusmeId);
                    sorts.Add(new Tuple<int, double>(item1.Id, reusmeavg));
                }
                List<VacancyReturnedToSeekerModel> vacancyReturnModelss = new List<VacancyReturnedToSeekerModel>();
                foreach (var vacancy in sorts.OrderByDescending(t => t.Item2).ToList())
                {
                    var item = await ctx.vacancies
                    .Where(p => p.Id == vacancy.Item1)
                     .Include(e => e.employeer)
                     .Include(p => p.VacancyLanguages)
                     .Include(p => p.vacancyEducations)
                     .Include(p => p.vacancySkills)
                     .Include(p => p.vacancyWorkExperiences)
                     .Include(e => e.employeer)
                     .ThenInclude(e => e.EmployeerProfile)

                     .FirstOrDefaultAsync();
                    VacancyReturnedToSeekerModel vacancyReturnModel = new VacancyReturnedToSeekerModel();

                    vacancyReturnModel.Id = item.Id;
                    vacancyReturnModel.job = item.job;
                    vacancyReturnModel.Descritption = item.Descritption;
                    vacancyReturnModel.spicialization = item.spicialization;
                    vacancyReturnModel.MaxSalary = item.MaxSalary;
                    vacancyReturnModel.MinSalary = item.MinSalary;
                    vacancyReturnModel.MaxHours = item.MaxHours;
                    vacancyReturnModel.MinHours = item.MinHours;
                    vacancyReturnModel.CompanyId = item.EmployeerId;
                    vacancyReturnModel.Name = item.employeer.Name;
                    vacancyReturnModel.ImageFile = item.employeer.EmployeerProfile.ImageFile;
                    var markedVacancies = await ctx.markedVacancies
                   .Where(p => p.JobSeekerID == seekerid)
                   .Include(e => e.Vacancy)
                   .ToListAsync();
                    foreach (var marked in markedVacancies)
                    {
                        if (marked.VacancyId == item.Id)
                        {
                            vacancyReturnModel.marked = true;
                        }
                    }
                    foreach (var i in item.vacancyEducations)
                    {
                        VacancyEducationModel vacancyLanguageModel = new VacancyEducationModel();
                        vacancyLanguageModel.Id = i.Id;
                        vacancyLanguageModel.Specialization = i.Specialization;

                        vacancyLanguageModel.Degree = i.Degree;

                        vacancyReturnModel.vacancyEducations.Add(vacancyLanguageModel);

                    }
                    foreach (var i in item.vacancySkills)
                    {
                        VacancySkillModel vacancyLanguageModel = new VacancySkillModel();
                        vacancyLanguageModel.Id = i.Id;
                        vacancyLanguageModel.Name = i.Name;


                        vacancyReturnModel.vacancySkills.Add(vacancyLanguageModel);

                    }
                    foreach (var i in item.vacancyWorkExperiences)
                    {
                        VacancyWorkExperienceModel vacancyWorkExperienceModel1 = new VacancyWorkExperienceModel();
                        vacancyWorkExperienceModel1.Field = i.Field;
                        vacancyWorkExperienceModel1.years = i.years;
                        vacancyWorkExperienceModel1.Id = i.Id;
                        vacancyReturnModel.vacancyWorkExperiences.Add(vacancyWorkExperienceModel1);

                    }
                    foreach (var i in item.VacancyLanguages)
                    {
                        VacancyLanguageModel vacancyLanguageModel = new VacancyLanguageModel();
                        vacancyLanguageModel.Id = i.Id;
                        vacancyLanguageModel.Name = i.Name;
                        vacancyLanguageModel.Level = i.Level;

                        vacancyReturnModel.VacancyLanguages.Add(vacancyLanguageModel);

                    }


                    var branch = await ctx.employeerBranches.Where(p => p.Id == item.BranchId).FirstOrDefaultAsync();
                    if (branch != null)
                    {
                        vacancyReturnModel.Address = branch.City;
                    }
                    vacancyReturnModelss.Add(vacancyReturnModel);
                }
                return Ok(vacancyReturnModelss);
            }
            List<VacancyReturnedToSeekerModel> vacancyReturnModels = new List<VacancyReturnedToSeekerModel>();
            foreach (var vacancy in vacancies)
            {
                var item = await ctx.vacancies
                .Where(p => p.Id == vacancy.Id)
                 .Include(e => e.employeer)
                 .Include(p => p.VacancyLanguages)
                 .Include(p => p.vacancyEducations)
                 .Include(p => p.vacancySkills)
                 .Include(p => p.vacancyWorkExperiences)
                 .Include(e => e.employeer)
                 .ThenInclude(e => e.EmployeerProfile)

                 .FirstOrDefaultAsync();
                VacancyReturnedToSeekerModel vacancyReturnModel = new VacancyReturnedToSeekerModel();

                vacancyReturnModel.Id = item.Id;
                vacancyReturnModel.job = item.job;
                vacancyReturnModel.Descritption = item.Descritption;
                vacancyReturnModel.spicialization = item.spicialization;
                vacancyReturnModel.MaxSalary = item.MaxSalary;
                vacancyReturnModel.MinSalary = item.MinSalary;
                vacancyReturnModel.MaxHours = item.MaxHours;
                vacancyReturnModel.MinHours = item.MinHours;
                vacancyReturnModel.CompanyId = item.EmployeerId;
                vacancyReturnModel.Name = item.employeer.Name;
                vacancyReturnModel.ImageFile = item.employeer.EmployeerProfile.ImageFile;
                var markedVacancies = await ctx.markedVacancies
               .Where(p => p.JobSeekerID == seekerid)
               .Include(e => e.Vacancy)
               .ToListAsync();
                foreach (var marked in markedVacancies)
                {
                    if (marked.VacancyId == item.Id)
                    {
                        vacancyReturnModel.marked = true;
                    }
                }
                foreach (var i in item.vacancyEducations)
                {
                    VacancyEducationModel vacancyLanguageModel = new VacancyEducationModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Specialization = i.Specialization;

                    vacancyLanguageModel.Degree = i.Degree;

                    vacancyReturnModel.vacancyEducations.Add(vacancyLanguageModel);

                }
                foreach (var i in item.vacancySkills)
                {
                    VacancySkillModel vacancyLanguageModel = new VacancySkillModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Name = i.Name;


                    vacancyReturnModel.vacancySkills.Add(vacancyLanguageModel);

                }
                foreach (var i in item.vacancyWorkExperiences)
                {
                    VacancyWorkExperienceModel vacancyWorkExperienceModel1 = new VacancyWorkExperienceModel();
                    vacancyWorkExperienceModel1.Field = i.Field;
                    vacancyWorkExperienceModel1.years = i.years;
                    vacancyWorkExperienceModel1.Id = i.Id;
                    vacancyReturnModel.vacancyWorkExperiences.Add(vacancyWorkExperienceModel1);

                }
                foreach (var i in item.VacancyLanguages)
                {
                    VacancyLanguageModel vacancyLanguageModel = new VacancyLanguageModel();
                    vacancyLanguageModel.Id = i.Id;
                    vacancyLanguageModel.Name = i.Name;
                    vacancyLanguageModel.Level = i.Level;

                    vacancyReturnModel.VacancyLanguages.Add(vacancyLanguageModel);

                }


                var branch = await ctx.employeerBranches.Where(p => p.Id == item.BranchId).FirstOrDefaultAsync();
                if (branch != null)
                {
                    vacancyReturnModel.Address = branch.City;
                }
                vacancyReturnModels.Add(vacancyReturnModel);
            }
            return Ok(vacancyReturnModels);


            //else
            //{
            //    return NoContent();
            //}

        }
    




       // [HttpGet("api/{vacancyId}/{reusmeId}")]
        public double sortOneVacancywithOneReusme(int vacancyId, int reusmeId)
        {
            var vacancy = ctx.vacancies
                .Where(p => p.Id == vacancyId)
                .Include(s=>s.vacancyWorkExperiences)
                .Include(s => s.VacancyLanguages)
                .Include(s => s.vacancySkills)
                .Include(s => s.vacancyEducations)
                .FirstOrDefault();
            var reusme  = ctx.reusmes
                .Where(a => a.Id == reusmeId)
                .Include(s => s.reusmeEducations)
                .Include(s => s.reusmeSkills)
                .Include(s => s.reusmeWorkExperiences)
                .Include(s=>s.jobSeeker)
                .ThenInclude(a=>a.jobSeekerLanguages)
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
                            i = Convert.ToDouble(item1.years) / Convert.ToDouble(item.years);
                            if (i > 1) { i = 1; }
                            sum = sum + i;
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
                            if (item1.Level == "Beginner")
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
                    if (item.Specialization.ToLower() == item1.Specialization.ToLower())
                    {
                        if (item.Degree.ToLower() == "Doctorate".ToLower())
                        {
                            if(item1.Degree.ToLower() == "Doctorate".ToLower())
                            {
                                s = 1;
                                sum = sum + s;
                            }
                            else if(item1.Degree.ToLower() == "Master's degree".ToLower())
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
                        else if(item.Degree.ToLower() == "Master's degree".ToLower())
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

            else if (vacancy.vacancyWorkExperiences.Count() == 0 && vacancy.vacancyEducations.Count() == 0
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
        public double sortOneReusmewithOneVacancy(int vacancyId, int reusmeId)
        {
            var vacancy = ctx.vacancies
                .Where(p => p.Id == vacancyId)
                .Include(s => s.vacancyWorkExperiences)
                .Include(s => s.VacancyLanguages)
                .Include(s => s.vacancySkills)
                .Include(s => s.vacancyEducations)
                .FirstOrDefault();
            var reusme = ctx.reusmes
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

    }
}
