using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class JobSeekerEducation
    {
        [Key]
        public int Id { get; set; }
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public string University { get; set; }

        [ForeignKey("JobSeeker")]
        public int? JobSeekerID { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public JobSeeker JobSeeker { get; set; }


        

    }
}
