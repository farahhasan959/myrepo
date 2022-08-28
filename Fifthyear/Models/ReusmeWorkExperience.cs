using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class ReusmeWorkExperience
    {
        public int Id { get; set; }
        public string Field { get; set; }
        public int years { get; set; }

        [ForeignKey("Reusme")]
        public int? ReusmeID { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Reusme reusme { get; set; }
    }
}
