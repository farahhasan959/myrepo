using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class Vacancy
    {
        
        [Key]
        public int Id{ get; set; }
       
        public string spicialization { get; set; }
        public string job { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public string Descritption { get; set; }
        
        [ForeignKey("Employeer")]
        public int? EmployeerId { get; set; }
        public Employeer employeer { get; set; }
        [ForeignKey("EmployeerBranch")]
        public int? BranchId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public EmployeerBranch EmployeerBranch { get; set; }

        public ICollection<NotificationResponse> notifications { get; set; } = new HashSet<NotificationResponse>();
        public ICollection<VacancyWorkExperience> vacancyWorkExperiences { get; set; } = new HashSet<VacancyWorkExperience>();
        public ICollection<VacancySkill> vacancySkills { get; set; } = new HashSet<VacancySkill>();
        public ICollection<VacancyLanguage> VacancyLanguages { get; set; } = new HashSet<VacancyLanguage>();
        public ICollection<VacancyEducation> vacancyEducations { get; set; } = new HashSet<VacancyEducation>();
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public ICollection<MarkedVacancy> markedVacancies { get; set; } = new HashSet<MarkedVacancy>();
        public Vacancy Clone()
        {
            return this.MemberwiseClone() as Vacancy;
        }
    }
}
