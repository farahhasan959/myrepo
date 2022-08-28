using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class EmployeerEmail
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }

        [ForeignKey("EmployeerProfile")]

        public int? EmployeerProfileId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public virtual EmployeerProfile EmployeerProfile { get; set; }
    }
}
