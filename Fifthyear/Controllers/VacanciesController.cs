using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fifthyear.Data;
using Fifthyear.Models;
using Microsoft.Extensions.Configuration;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using Fifthyear.Services;

namespace Fifthyear.Controllers
{
    public class VacanciesController : ControllerBase
    {
        private readonly Context ctx;
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly INotificationService _notificationService;
        FirebaseApp defaultApp;
        private readonly FirebaseMessaging messaging;
        private IConfiguration _config;
        public VacanciesController(Context ctx, IConfiguration configuration)
        {
            this.ctx = ctx;
            _config = configuration;
            AppOptions appOp = new AppOptions() { Credential = GoogleCredential.FromFile(AppDomain.CurrentDomain.BaseDirectory + "hireme-c5c3a-firebase-adminsdk-raaqv-3a97010105.json") };
            FirebaseApp app;
            if (FirebaseApp.DefaultInstance == null)
                app = FirebaseApp.Create(appOp);
            else
                app = FirebaseApp.GetInstance(FirebaseApp.DefaultInstance.Name);
            messaging = FirebaseMessaging.GetMessaging(app);
        }
        //[HttpGet("api/companyvacancies")]
        public async Task<ActionResult<IEnumerable<Vacancy>>> GetVacanciesFdddddorCompany(int id)
        {
            var Items = await ctx.vacancies
                   
                   
                   .ToListAsync();
            return Ok(Items);

        }

            [HttpGet("api/companyvacancies/{id}")]
        public async Task<ActionResult<IEnumerable<Vacancy>>> GetVacanciesForCompany(int id)
        {
            List<VacancyReturnModel> vacancyReturnModels=new List<VacancyReturnModel>();
            var Items = await ctx.vacancies
                .Where(p=>p.EmployeerId==id)
                .Include(p => p.VacancyLanguages)
                .Include(p => p.vacancyEducations)
                .Include(p => p.vacancySkills)
                .Include(p => p.vacancyWorkExperiences)
                .Include(e => e.employeer)              
                .ToListAsync();
            foreach(var item in Items)
            {
                VacancyReturnModel vacancyReturnModel = new VacancyReturnModel();
                //IEnumerable <VacancyWorkExperienceModel> vacancyWorkExperienceModel;
                vacancyReturnModel.Id = item.Id;
                vacancyReturnModel.job = item.job;
                vacancyReturnModel.Descritption = item.Descritption;
                vacancyReturnModel.spicialization = item.spicialization;
                vacancyReturnModel.MaxSalary = item.MaxSalary;
                vacancyReturnModel.MinSalary = item.MinSalary;
                vacancyReturnModel.MaxHours = item.MaxHours;
                vacancyReturnModel.MinHours = item.MinHours;
              
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
        }
        [HttpGet("api/vacanciesforseeker/{id}")]
        public async Task<ActionResult<IEnumerable<Vacancy>>> GetVacanciesForSeeker(int id)
        {
            List<VacancyReturnedToSeekerModel> vacancyReturnModels = new List<VacancyReturnedToSeekerModel>();
            var Items = await ctx.vacancies
                .Include(e => e.employeer)
                .Include(p => p.VacancyLanguages)
                .Include(p => p.vacancyEducations)
                .Include(p => p.vacancySkills)
                .Include(p => p.vacancyWorkExperiences)
                .Include(e => e.employeer)
                .ThenInclude(e => e.EmployeerProfile)

                .ToListAsync();
            foreach (var item in Items)
            {
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
               .Where(p => p.JobSeekerID == id)
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
        }

       
       
        
       // [HttpGet("api/vacancies/filterbycompany/{company}")]
        public async Task<ActionResult<List<int>>> filterbyCompany(string company)
        {
            var Items = await ctx.vacancies
                .Include(p=>p.employeer)
                .Where(p => p.employeer.Name.Contains(company))
        
                .ToListAsync();
            List<int> vac = new List<int>();
            foreach (var item in Items)
            {

                vac.Add(item.Id);

            }
            List<VacancyReturnedToSeekerModel> vacancyReturnModels = new List<VacancyReturnedToSeekerModel>();
            foreach (var id in vac)
            {
                var item = await ctx.vacancies
                  .Where(p => p.Id == id)
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
               .Where(p => p.JobSeekerID == id)
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


                var branchc = await ctx.employeerBranches.Where(p => p.Id == item.BranchId).FirstOrDefaultAsync();
                if (branchc != null)
                {
                    vacancyReturnModel.Address = branchc.City;
                }
                vacancyReturnModels.Add(vacancyReturnModel);








            }

            return Ok(vacancyReturnModels);
        }
       // [HttpGet("api/vacancies/filterbyspicialization/{spicialization}")]
        public async Task<ActionResult<List<int>>> filterbySpicialization(string spicialization)
        {
            var Items = await ctx.vacancies
                .Where(p => p.spicialization.Contains(spicialization))
                .ToListAsync();
            List<int> vac = new List<int>();
            foreach (var item in Items)
            {
               
               vac.Add(item.Id);
                   
            }
            List<VacancyReturnedToSeekerModel> vacancyReturnModels = new List<VacancyReturnedToSeekerModel>();
            foreach (var id in vac)
            {
                var item = await ctx.vacancies
                  .Where(p => p.Id == id)
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
               .Where(p => p.JobSeekerID == id)
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


                var branchc = await ctx.employeerBranches.Where(p => p.Id == item.BranchId).FirstOrDefaultAsync();
                if (branchc != null)
                {
                    vacancyReturnModel.Address = branchc.City;
                }
                vacancyReturnModels.Add(vacancyReturnModel);








            }

            return Ok(vacancyReturnModels);
        }
       // [HttpGet("api/vacancies/filterbyLocation/{city}")]
        public async  Task<ActionResult<IEnumerable<Vacancy>>> filterbyLocation(string city)
        {

            List<int> vac = new List<int>();
            var branch = await ctx.employeerBranches.Where(p => p.City.Contains(city)).ToListAsync();

            var Items = await ctx.vacancies
                .Include(P => P.employeer)
                .ThenInclude(p => p.EmployeerProfile)
                .ToListAsync();

            foreach (var item in Items)
            {
                foreach (var item1 in branch)
                {
                    if (item.BranchId ==item1.Id)
                    {
                        vac.Add(item.Id);
                    }

                }
            }
            List<VacancyReturnedToSeekerModel> vacancyReturnModels = new List<VacancyReturnedToSeekerModel>();
            foreach(var id in vac) {
                var item = await ctx.vacancies
                  .Where(p=>p.Id==id)
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
               .Where(p => p.JobSeekerID == id)
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


                var branchc = await ctx.employeerBranches.Where(p => p.Id == item.BranchId).FirstOrDefaultAsync();
                if (branchc != null)
                {
                    vacancyReturnModel.Address = branchc.City;
                }
                vacancyReturnModels.Add(vacancyReturnModel);








            }

            return Ok(vacancyReturnModels);
        }




        public async Task<(List<Vacancy>, List<Reusme>, int n)> Preparation()
        {
            var vacancies = ctx.vacancies
              .Include(s => s.vacancyWorkExperiences)
              .Include(s => s.VacancyLanguages)
              .Include(s => s.vacancySkills)
              .Include(s => s.vacancyEducations)
              .ToList();

            var reusmes = ctx.reusmes
               .Include(s => s.reusmeEducations)
               .Include(s => s.reusmeSkills)
               .Include(s => s.reusmeWorkExperiences)
               .Include(s => s.jobSeeker)
               .ThenInclude(a => a.jobSeekerLanguages)
               .Include(s=>s.jobSeeker)
               .ToList();

            if (vacancies.Count() == 0 || reusmes.Count() == 0)
            {
                return (vacancies,reusmes,0);
            }
            if (Math.Abs(vacancies.Count()-reusmes.Count()) >= 3)
            {
                return (vacancies, reusmes, -1);
            }
            var maxVacanctId = ctx.vacancies.Select(p => p.Id).Max();
            var maxReusmeId = ctx.reusmes.Select(p => p.Id).Max();

            List<Vacancy> vacanciesListWithNItem = new List<Vacancy>();
            List<Reusme> reusmesListWithNItem = new List<Reusme>();

            int ReusmesNumber = reusmes.Count();
            int VacanciesNumber = vacancies.Count();
            int N = ReusmesNumber;

            foreach (var item in vacancies)
            {
                vacanciesListWithNItem.Add(item);
            }
            foreach (var item in reusmes)
            {
                reusmesListWithNItem.Add(item);
            }
            if (ReusmesNumber > VacanciesNumber)
            {
                N = ReusmesNumber;
                Random rnd = new Random();
                List<int> vv = new List<int>();
                int k = ReusmesNumber - VacanciesNumber;
                for (var l = 0; l < k; l++)
                {
                    vv.Add(rnd.Next(1, maxVacanctId));
                }


                foreach (var f in vv)
                {
                    var vacancy = await ctx.vacancies.Where(p => p.Id == f).FirstOrDefaultAsync();
                    if (vacancy == null)
                    {
                        var vacc = await ctx.vacancies.Where(p => p.Id == maxVacanctId).FirstOrDefaultAsync();
                        var newVacancy = vacc.Clone();
                        newVacancy.Id = maxVacanctId + 1;
                        vacanciesListWithNItem.Add(newVacancy);
                    }
                    else
                    {
                        var newVacancy = vacancy.Clone();
                        newVacancy.Id = maxVacanctId + 1;
                        vacanciesListWithNItem.Add(newVacancy);
                    }



                }

            }
            else if (VacanciesNumber > ReusmesNumber)
            {
                N = VacanciesNumber;
                Random rnd = new Random();
                List<int> vv = new List<int>();
                int k = VacanciesNumber - ReusmesNumber;
                for (var l = 0; l < k; l++)
                {
                    vv.Add(rnd.Next(1, maxReusmeId));
                }


                foreach (var f in vv)
                {
                    var reusme = await ctx.reusmes.Where(p => p.Id == f).FirstOrDefaultAsync();
                    if (reusme == null)
                    {
                        var reu = await ctx.reusmes.Where(p => p.Id == maxReusmeId).FirstOrDefaultAsync();
                        var newReusme = reu.Clone();
                        newReusme.Id = maxReusmeId + 1;
                        reusmesListWithNItem.Add(newReusme);
                    }
                    else
                    {
                        var newReusme = reusme.Clone();
                        newReusme.Id = maxReusmeId + 1;
                        reusmesListWithNItem.Add(newReusme);
                    }


                }
            }


            return (vacanciesListWithNItem, reusmesListWithNItem, N);

        }
        public async Task<(int[,], int[,])> SortBeforeAlgorithm()
        {
            List<Vacancy> vacancies = new List<Vacancy>();
            List<Reusme> reusmes = new List<Reusme>();
            int N;
            (vacancies, reusmes, N) = await Preparation();

            int[,] vac = new int[N, N];
            int[,] rus = new int[N, N];
            List<int> v = new List<int>();
            List<int> r = new List<int>();



            var i = 0;
            foreach (var item in reusmes)
            {

                List<Tuple<int, int, double>> sortRuss = new List<Tuple<int, int, double>>();
                foreach (var item1 in vacancies)
                {
                    var reusmeavg = sortOneReusmewithOneVacancy(item1, item);
                    sortRuss.Add(new Tuple<int, int, double>(item1.Id, item.Id, reusmeavg));
                }
                r = sortRuss.OrderByDescending(t => t.Item3).Select(p => p.Item1).ToList();
                for (var d = 0; d < N; d++)
                {
                    rus[i, d] = r[d];
                }
                i = i + 1;

            }
            var j = 0;
            foreach (var item in vacancies)
            {

                List<Tuple<int, int, double>> sortVacs = new List<Tuple<int, int, double>>();
                foreach (var item1 in reusmes)
                {
                    var reusmeavg = sortOneVacancywithOneReusme(item, item1);
                    sortVacs.Add(new Tuple<int, int, double>(item1.Id, item.Id, reusmeavg));
                }
                v = sortVacs.OrderByDescending(t => t.Item3).Select(p => p.Item1).ToList();
                for (var d = 0; d < N; d++)
                {
                    vac[j, d] = v[d];
                }
                j = j + 1;
            }

            return (rus, vac);
        }
        public List<Tuple<int, double>> sortOneVacancyWithAllReusmes(List<Reusme> reusmes, Vacancy vacancy)
        {

            List<Tuple<int, double>> sorts = new List<Tuple<int, double>>();

            foreach (var item1 in reusmes)
            {
                var reusmeavg = sortOneVacancywithOneReusme(vacancy, item1);
                sorts.Add(new Tuple<int, double>(item1.Id, reusmeavg));
            }
            return sorts.OrderByDescending(t => t.Item2).ToList();
        }
        public List<Tuple<int, int, double>> sortOneReusmeWithAllVacancies(Reusme reusme, List<Vacancy> vacancies)
        {

            List<int> r = new List<int>();

            List<Tuple<int, int, double>> sorts = new List<Tuple<int, int, double>>();

            foreach (var item1 in vacancies)
            {
                var reusmeavg = sortOneReusmewithOneVacancy(item1, reusme);
                sorts.Add(new Tuple<int, int, double>(item1.Id, reusme.Id, reusmeavg));

            }
            r = sorts.OrderByDescending(t => t.Item3).Select(p => p.Item1).ToList();

            return sorts.OrderByDescending(t => t.Item3).ToList();
        }
        public double sortOneVacancywithOneReusme(Vacancy vacancy, Reusme reusme)
        {

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
        public double sortOneReusmewithOneVacancy(Vacancy vacancy, Reusme reusme)
        {

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
        [HttpPost("api/vacancies/{id}")]
        public async Task<ActionResult<Vacancy>> CreateVacancy([FromBody] VacancyAddModel vacancyAdd, [FromRoute] int id)
        {
            Vacancy vacancy = new Vacancy();
            vacancy.EmployeerId = id;
            vacancy.BranchId = vacancyAdd.BranchId;
            vacancy.spicialization = vacancyAdd.spicialization;
            vacancy.job = vacancyAdd.job;
            vacancy.MaxHours = vacancyAdd.MaxHours;
            vacancy.MaxSalary = vacancyAdd.MaxSalary;
            vacancy.MinHours = vacancyAdd.MinHours;
            vacancy.MinSalary = vacancyAdd.MinSalary;
            vacancy.Descritption = vacancyAdd.Descritption;
            vacancy.vacancyEducations = vacancyAdd.vacancyEducations;
            vacancy.VacancyLanguages = vacancyAdd.VacancyLanguages;
            vacancy.vacancySkills = vacancyAdd.vacancySkills;
            vacancy.vacancyWorkExperiences = vacancyAdd.vacancyWorkExperiences;
            await ctx.vacancies.AddAsync(vacancy);
            await ctx.SaveChangesAsync();
            var n = await match();
            return Ok(vacancy);


        }
        [HttpDelete("api/vacancies/{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var vacancy = await ctx.vacancies.FindAsync(id);
            int? employesid = vacancy.EmployeerId;
            if (vacancy == null)
            {
                return NotFound("This vacancy is not found");
            }

            ctx.vacancies.Remove(vacancy);
            await ctx.SaveChangesAsync();
            var n =await match();
            return NoContent();
        }
        public async Task<ActionResult<string>> match()
        {
            //rus contains vacancies
            //vac contains reusmes
            int N;
            List<Vacancy> vacancies = new List<Vacancy>();
            List<Reusme> reusmes = new List<Reusme>();
            (vacancies, reusmes, N) = await Preparation();
            if(N==0)
            {
                var algorithmresultss = await ctx.algorithmResults.ToListAsync();
                foreach (var a in algorithmresultss)
                {
                    ctx.algorithmResults.Remove(a);
                    await ctx.SaveChangesAsync();
                }
                return "you can't match";
            }
            if (N == -1)
            {
                
                return "you can't match";
            }
            int[,] vac = new int[N, N];
            int[,] rus = new int[N, N];
            List<int> vv = new List<int>();
            List<int> rr = new List<int>();

            Dictionary<int, int> vacHash = new Dictionary<int, int>();
            Dictionary<int, int> vacOrg = new Dictionary<int, int>();

            Dictionary<int, int> rusHash = new Dictionary<int, int>();
            Dictionary<int, int> rusOrg = new Dictionary<int, int>();

            for (int i = 0; i < vacancies.Count; i++)
            {
                int x = vacancies[i].Id;
                vacHash[x] = i;
                vacOrg[i] = x;
            }

            for (int i = 0; i < reusmes.Count; i++)
            {
                int x = reusmes[i].Id;
                rusHash[x] = i;
                rusOrg[i] = x;
            }

            (rus, vac) = await SortBeforeAlgorithm();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rus[i, j] = vacHash[rus[i, j]];
                    vac[i, j] = rusHash[vac[i, j]];
                }
            }
            SMP smp = new SMP();
            List<KeyValuePair<int, int>> answer = smp.solve(N, rus, vac);

            List<KeyValuePair<int, int>> finalAnswer = new List<KeyValuePair<int, int>>();

            foreach (KeyValuePair<int, int> A in answer)
            {
                finalAnswer.Add(new KeyValuePair<int, int>(rusOrg[A.Key], vacOrg[A.Value]));
            }
            foreach (var item in finalAnswer)
            {
                Vacancy vacancy = vacancies.Find(p => p.Id == item.Value);
                var vacancyctx = await ctx.vacancies.Where(p => p.Id == item.Value).FirstOrDefaultAsync();

                if (vacancyctx == null)
                {
                    var vacancyjob = await ctx.vacancies.Where(p => p.job == vacancy.job && p.Descritption == vacancy.Descritption && p.spicialization == vacancy.spicialization && p.EmployeerId==vacancy.EmployeerId).FirstOrDefaultAsync();
                    vv.Add(vacancyjob.Id);
                }
                else
                {
                    vv.Add(vacancyctx.Id);
                }
               
                Reusme reusme = reusmes.Find(p => p.Id == item.Key);
                var reusmectx = await ctx.reusmes.Where(p => p.Id == item.Key).FirstOrDefaultAsync();
                if (reusmectx == null)
                {
                    var reusmetitle = await ctx.reusmes.Where(p => p.Title == reusme.Title && p.spicialization ==reusme.spicialization && p.JobSeekerID==reusme.JobSeekerID).FirstOrDefaultAsync();
                    rr.Add(reusmetitle.Id);
                }
                else
                {
                    rr.Add(reusmectx.Id);
                }
               
            }
            
            List<KeyValuePair<int, int>> ans = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < rr.Count(); i++)
            {
                ans.Add(new KeyValuePair<int, int>(rr[i], vv[i]));

            }
            var algorithmresults = await ctx.algorithmResults.ToListAsync();
            foreach (var a in algorithmresults)
            {
                ctx.algorithmResults.Remove(a);
                await ctx.SaveChangesAsync();
            }
            foreach (KeyValuePair<int, int> A in ans)
            {
                var vacancy = await ctx.vacancies.Where(p => p.Id == A.Value).FirstOrDefaultAsync();
                var resume = ctx.reusmes.Where(p => p.Id == A.Key).FirstOrDefault();
                if (sortOneReusmewithOneVacancy(vacancy,resume) >60.00 && sortOneVacancywithOneReusme(vacancy, resume) >60.00) {
                    AlgorithmResult algorithmResult = new AlgorithmResult();
                    algorithmResult.ReusmeID = A.Key;
                    algorithmResult.VacancyId = A.Value;
                    //var vaca = await ctx.vacancies.Where(p => p.Id == algorithmResult.VacancyId).FirstOrDefaultAsync();
                    //var res = ctx.reusmes.Where(p => p.Id == algorithmResult.ReusmeID).FirstOrDefault();
                    algorithmResult.CompanyId = vacancy.EmployeerId;
                    algorithmResult.SeekerId = resume.JobSeekerID;
                    await ctx.algorithmResults.AddAsync(algorithmResult);
                    await ctx.SaveChangesAsync();
                }
            }
            foreach (var a in algorithmresults)
            {
                var company = ctx.employeers.Where(p => p.Id == a.CompanyId).FirstOrDefault();
                if (company.FirebaseToken != null)
                {
                    var message = new Message()
                    {

                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = "There Is A Match !!",
                            Body = "A match happened to one of your vacancies"
                        },


                        Token = company.FirebaseToken,
                    };
                    string response = await messaging.SendAsync(message);
                }
                

                var seeker = ctx.jobSeekers.Where(p => p.Id == a.SeekerId).FirstOrDefault();
                if (seeker.FirebaseToken != null)
                {
                    var message2 = new Message()
                    {

                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = "There Is A Match !!",
                            Body = "A match happened to one of your resumes"
                        },


                        Token = seeker.FirebaseToken,
                    };
                    string response2 = await messaging.SendAsync(message2);
                }
            }


            return "yes";
        }
        [HttpPost("api/reusmes/{id}")]
        public async Task<ActionResult<Reusme>> CreateReusme([FromBody] ReusmeAddModel reusmeadd, int id)
        {
            Reusme reusme = new Reusme();
            reusme.JobSeekerID = id;
            reusme.Title = reusmeadd.Title;
            reusme.spicialization = reusmeadd.spicialization;
            await ctx.reusmes.AddAsync(reusme);
            await ctx.SaveChangesAsync();
            // List<ReusmeEducation> reusmeEducationslist = new List<ReusmeEducation>();
            foreach (var educationid in reusmeadd.reusmeEducations)
            {
                var jobseekereducations = await ctx.jobSeekerEducations.Where(p => p.JobSeekerID == id).ToListAsync();
                foreach (var education in jobseekereducations)
                {
                    if (educationid == education.Id)
                    {
                        ReusmeEducation reusmeEducation = new ReusmeEducation();
                        // reusmeEducation.Id = educationid;
                        reusmeEducation.ReusmeID = reusme.Id;
                        reusmeEducation.Specialization = education.Specialization;
                        reusmeEducation.University = education.University;
                        reusmeEducation.Degree = education.Degree;
                        await ctx.reusmeEducations.AddAsync(reusmeEducation);
                        await ctx.SaveChangesAsync();


                    }
                }
            }
            foreach (var educationid in reusmeadd.reusmeSkills)
            {
                var jobseekereducations = await ctx.jobSeekerSkills.Where(p => p.JobSeekerID == id).ToListAsync();
                foreach (var education in jobseekereducations)
                {
                    if (educationid == education.Id)
                    {
                        ReusmeSkill reusmeEducation = new ReusmeSkill();
                        // reusmeEducation.Id = educationid;
                        reusmeEducation.ReusmeID = reusme.Id;
                        reusmeEducation.Name = education.Name;

                        await ctx.reusmeSkills.AddAsync(reusmeEducation);
                        await ctx.SaveChangesAsync();


                    }
                }
            }
            foreach (var educationid in reusmeadd.reusmeWorkExperiences)
            {
                var jobseekereducations = await ctx.jobSeekerWorkExperiences.Where(p => p.JobSeekerID == id).ToListAsync();
                foreach (var education in jobseekereducations)
                {
                    if (educationid == education.Id)
                    {
                        ReusmeWorkExperience reusmeEducation = new ReusmeWorkExperience();
                        // reusmeEducation.Id = educationid;
                        reusmeEducation.ReusmeID = reusme.Id;
                        reusmeEducation.Field = education.Field;
                        reusmeEducation.years = education.years;

                        await ctx.reusmeWorkExperiences.AddAsync(reusmeEducation);
                        await ctx.SaveChangesAsync();


                    }
                }
            }
        
            await match();
            return Ok(reusme);
        }
        [HttpDelete("api/reusmes/{id}")]
        public async Task<IActionResult> DeleteReusme(int id)
        {

            var reusme = await ctx.reusmes.FindAsync(id);
            int? seekerid = reusme.JobSeekerID;
            if (reusme == null)
            {
                return NotFound();
            }

            ctx.reusmes.Remove(reusme);
            await ctx.SaveChangesAsync();
            var n = await match();
            return NoContent();
        }
        [HttpPut("api/reusmes/{id}")]
        public async Task<ActionResult<Reusme>> UpdateReusme

          ([FromBody] ReusmeAddModel reusme, [FromRoute] int id)
        {
            var reus = await ctx.reusmes
                .Where(item => item.Id == id)
                .Include(it => it.reusmeEducations)
                .Include(it => it.reusmeSkills)
                .Include(it => it.reusmeWorkExperiences)
                .FirstOrDefaultAsync();

            if (reus == null)
            {
                return NotFound();
            }

            reus.Title = reusme.Title;
            reus.spicialization = reusme.spicialization;
            List<ReusmeEducation> reusmeEducations = new List<ReusmeEducation>();
            List<ReusmeSkill> reusmeSkills = new List<ReusmeSkill>();
            List<ReusmeWorkExperience> reusmeWorkExperiences = new List<ReusmeWorkExperience>();
            foreach (var education in reusme.reusmeEducations)
            {
                var jobedu = ctx.jobSeekerEducations.Where(p => p.Id == education).FirstOrDefault();
                ReusmeEducation reusmeEducation = new ReusmeEducation();
                reusmeEducation.Specialization = jobedu.Specialization;
                reusmeEducation.University = jobedu.University;
                reusmeEducation.Degree = jobedu.Degree;
                reusmeEducations.Add(reusmeEducation);
            }
            foreach (var exp in reusme.reusmeWorkExperiences)
            {
                var jobexp = ctx.jobSeekerWorkExperiences.Where(p => p.Id == exp).FirstOrDefault();
                ReusmeWorkExperience reusmeWorkExperience = new ReusmeWorkExperience();
                reusmeWorkExperience.Field = jobexp.Field;
                reusmeWorkExperience.years = jobexp.years;
                reusmeWorkExperiences.Add(reusmeWorkExperience);
            }
            foreach (var exp in reusme.reusmeSkills)
            {
                var jobexp = ctx.jobSeekerSkills.Where(p => p.Id == exp).FirstOrDefault();
                ReusmeSkill reusmeWorkExperience = new ReusmeSkill();
                reusmeWorkExperience.Name = jobexp.Name;
                reusmeSkills.Add(reusmeWorkExperience);
            }
            reus.reusmeWorkExperiences = reusmeWorkExperiences;
            reus.reusmeSkills = reusmeSkills;
            reus.reusmeEducations = reusmeEducations;
            await ctx.SaveChangesAsync();
            await match();
            return NoContent();
        }
    
}
}
