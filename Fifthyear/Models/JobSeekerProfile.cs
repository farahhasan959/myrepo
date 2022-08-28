
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class JobSeekerProfile
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        public string ImageFile { get; set; }
        public string ProfileEmail { get; set; }

        [ForeignKey("JobSeeker")]
        public int? JobSeekerID { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public JobSeeker JobSeeker { get; set; }
    }
}
