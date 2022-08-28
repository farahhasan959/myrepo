using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class VacancyAddModel
    {
        public int BranchId { get; set; }
        public string spicialization { get; set; }
        public string job { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public string Descritption { get; set; }
        public ICollection<VacancyWorkExperience> vacancyWorkExperiences { get; set; } = new HashSet<VacancyWorkExperience>();
        public ICollection<VacancySkill> vacancySkills { get; set; } = new HashSet<VacancySkill>();
        public ICollection<VacancyLanguage> VacancyLanguages { get; set; } = new HashSet<VacancyLanguage>();
        public ICollection<VacancyEducation> vacancyEducations { get; set; } = new HashSet<VacancyEducation>();
    }
}
