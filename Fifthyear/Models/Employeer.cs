using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class Employeer : User
    {

        
        public string Specialization { get; set; }
        //public string MainAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public virtual EmployeerProfile EmployeerProfile { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public ICollection<Vacancy> vacancies { get; set; }


    }
}
