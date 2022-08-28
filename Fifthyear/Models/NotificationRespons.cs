using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class NotificationResponse
    {
        [Key]
        public int Id { get; set; } 
        
        [DefaultValue(false)]
        public bool IsSeen { get; set; }     
        public string Message { get; set; }
        [ForeignKey("Reusme")]
        public int? ReusmeID { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Reusme reusme { get; set; }
        [ForeignKey("Vacancy")]
        public int? VacancyId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Vacancy Vacancy { get; set; }
        public int? companyId { get; set; }
        public int? seekerId { get; set; }
    }
}
