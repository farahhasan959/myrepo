using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class EmployeerProfile
    {
        [Key]
        public int Id { get; set; }
        public string AboutUs { get; set; }
        public string ImageFile { get; set; }

        [ForeignKey("Employeer")]
        public int? EmployeerId { get; set; }
        public virtual Employeer Employeer { get; set; }
        public ICollection<EmployeerBranch> employeerBranches { get; set; } = new HashSet<EmployeerBranch>();
        public ICollection<EmployeerEmail> employeerEmails { get; set; } = new HashSet<EmployeerEmail>();
    }
}
