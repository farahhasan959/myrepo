using Fifthyear.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fifthyear.Models;

namespace Fifthyear.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context context;
        private readonly AppSettings appSettings;

        public UsersController(Context context, IOptions<AppSettings> appSettings)
        {

            this.context = context;
            this.appSettings = appSettings.Value;
        }
        /*api/users/seekerlogout/{id}*/
        [HttpGet("seekerlogout/{id}")]
        public async Task<ActionResult> seekerlogout(int id) {
            var user =await context.jobSeekers.Where(p => p.Id == id).FirstOrDefaultAsync();
            user.FirebaseToken = null;
            await context.SaveChangesAsync();
            return Ok();
        
        }
        /*api/users/companylogout/{id}*/
        [HttpGet("companylogout/{id}")]
        public async Task<ActionResult> companylogout(int id)
        {
            var user = await context.employeers.Where(p => p.Id == id).FirstOrDefaultAsync();
            user.FirebaseToken = null;
            await context.SaveChangesAsync();
            return Ok();

        }
        [HttpPost("companylogin")]
        public IActionResult CAuthenticate([FromBody] AuthenticateModel model)
        {
            var userwithemail = context.employeers.Where(p => p.Email == model.Email).FirstOrDefault();
            if (userwithemail != null){
                var user = context.employeers.Where(p => p.Email == model.Email && p.Password == model.Password).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest("password is incorrect");
                }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name,user.Name),
                }),
                Expires = DateTime.UtcNow.AddMonths(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);
            user.TokenTime = tokenDescriptor.Expires;
                user.FirebaseToken = model.FirebaseToken;

                context.SaveChanges();

                return Ok(user);
            }
            else
            {
                return NotFound("Email is not Found");
            }
        }
        [HttpPost("seekerlogin")]
        public IActionResult SAuthenticate([FromBody] AuthenticateModel model)
        {
            var userwithemail = context.jobSeekers.Where(p => p.Email == model.Email).FirstOrDefault();
            if (userwithemail != null)
            {
                var user = context.jobSeekers.Where(p => p.Email == model.Email && p.Password == model.Password).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Password is incorrect");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name,user.Name),
                }),
                Expires = DateTime.UtcNow.AddMonths(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);
            user.TokenTime = tokenDescriptor.Expires;
            user.FirebaseToken = model.FirebaseToken;
             context.SaveChanges();

                return Ok(user);
            }
            else
            {
                return NotFound("Email is not Found");
            }
        }
        [HttpPost("companysignup")]
        public async Task<ActionResult<User>> CompanySignUp([FromBody] CompanySignUpModel model)
        {
            if (model != null) {
               
                var user = context.users.Where(p => p.Email == model.Email).FirstOrDefault();
                if(user != null)
                {
                    return BadRequest("this email is not valid");
                }
                if (user == null)
                {
                    var company = new Employeer();
                    company.Role = Role.Company;
                    company.Name = model.Name;
                    company.Email = model.Email;
                    //company.MainAddress = model.Address;
                    company.Password = model.Password;
                    company.Latitude = model.Latitude;
                    company.Longitude = model.Longitude;
                    company.Specialization = model.Specialize;
                    company.FirebaseToken = model.FirebaseToken;
                    await context.employeers.AddAsync(company);
                    await context.SaveChangesAsync();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, company.Id.ToString()),
                    new Claim(ClaimTypes.Role, company.Role),
                    new Claim(ClaimTypes.Name,company.Name),
                        }),
                        Expires = DateTime.UtcNow.AddMonths(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    company.Token = tokenHandler.WriteToken(token);
                    company.TokenTime = tokenDescriptor.Expires;
                    EmployeerProfile employeerProfile = new EmployeerProfile();
                    employeerProfile.AboutUs = "";
                    employeerProfile.EmployeerId = company.Id;                    
                    await context.employeerProfiles.AddAsync(employeerProfile);
                    await context.SaveChangesAsync();
                    return Ok(company);
                }
            }
          
             return BadRequest();
            

        }
        [HttpPost("seekersignup")]
        public async Task<ActionResult<User>> SeekerSignUp([FromBody] SeekerSignUpModel model)
        {
            if (model != null)
            {
                
                var user = context.users.Where(p => p.Email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    return BadRequest("this email is not valid");
                }
                if (user == null)
                {
                var seeker = new JobSeeker();
                seeker.Role = Role.Seeker;
                seeker.Name = model.Name;
                seeker.Email = model.Email;
                seeker.age = model.age;
                seeker.Password = model.Password;
                seeker.Gender = model.Gender;
                seeker.FirebaseToken = model.FirebaseToken;
                await context.jobSeekers.AddAsync(seeker);
                await context.SaveChangesAsync();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, seeker.Id.ToString()),
                    new Claim(ClaimTypes.Role, seeker.Role),
                    new Claim(ClaimTypes.Name,seeker.Name),
                        }),
                        Expires = DateTime.UtcNow.AddMonths(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    seeker.Token = tokenHandler.WriteToken(token);
                    seeker.TokenTime = tokenDescriptor.Expires;

                    JobSeekerProfile jobSeekerProfile = new JobSeekerProfile();
                    jobSeekerProfile.Bio = "";
                    jobSeekerProfile.JobSeekerID = seeker.Id;
                    await context.JobSeekerProfiles.AddAsync(jobSeekerProfile);
                    await context.SaveChangesAsync();

                    return Ok(seeker);
            }   }
            return BadRequest();
        }

    }
}
