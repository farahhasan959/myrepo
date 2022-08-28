using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class VacancySkill
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public bool Mandatory { get; set; }

        [ForeignKey("Vacancy")]
        public int? VacancyId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Vacancy Vacancy { get; set; }
        
    }
}
