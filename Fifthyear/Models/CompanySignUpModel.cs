using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class CompanySignUpModel
    {
 
        public string Name { get; set; }
        public string Specialize { get; set; }  

        public string Email { get; set; }
        public string Password { get; set; }
        public string FirebaseToken { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
