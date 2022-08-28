using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class AuthenticateModel
    {
        
        public string Email { get; set; }

       
        public string Password { get; set; }
        public string FirebaseToken { get; set; }
    }
}
