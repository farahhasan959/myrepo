using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fifthyear.Data;
using Fifthyear.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
//int intid = Convert.ToInt32(userid);
//var user = await context.users.Where(p => p.Id == intid).FirstOrDefaultAsync();
namespace Fifthyear.Controllers
{
    public class EmployeerProfilesController : ControllerBase
    {
        private readonly Context ctx;
        private readonly IHostingEnvironment _host;
        public EmployeerProfilesController(Context ctx, IHostingEnvironment _host)
        {
            this.ctx = ctx;
            this._host = _host;
        }
       // [HttpGet("api/profiles")]
        public async Task<ActionResult<IEnumerable<EmployeerProfile>>> GetProfiles()
        {
            var Items = await ctx.employeerProfiles
                .Include(p => p.employeerBranches)
                .Include(e => e.employeerEmails)
               .ToListAsync();
            return Ok(Items);
        }
        [HttpGet("api/profiles/{id}")]
        public async Task<ActionResult<EmployeerProfile>> GetProfileById([FromRoute] int id)
        {
            var profile = await ctx.employeerProfiles
                .Where(item => item.EmployeerId == id)
                .Include(e => e.Employeer)
                .Include(p => p.employeerBranches)
                .Include(e => e.employeerEmails)       
               
                .FirstOrDefaultAsync();
            if (profile != null)
            {
                return Ok(profile);
            }
            return NotFound("This profile is not found");
        }

        [HttpPut("api/profiles/{id}")]
        public async Task<ActionResult> UpdateProfile

            ([FromBody] EmployeerProfile employeerProfile, [FromRoute] int id)
        {
            var profile = await ctx.employeerProfiles
                .Where(item => item.EmployeerId == id)
                .Include(item => item.employeerBranches)
                .Include(item => item.employeerEmails)
                .FirstOrDefaultAsync();
            if (profile == null)
            {
                return NotFound("This profile is not found");
            }
            profile.AboutUs = employeerProfile.AboutUs;
            profile.employeerEmails = employeerProfile.employeerEmails;
           
                foreach (var item in employeerProfile.employeerBranches)
                {
                           item.EmployeerProfileId = profile.Id;
                        

                           await ctx.employeerBranches.AddAsync(item);
                            await ctx.SaveChangesAsync();

                    
                }
        
           
         
            await ctx.SaveChangesAsync();       
            return Ok(profile);
        }
        //[HttpPut("api/profiles/image/{id}")]
        //public async Task<ActionResult> UpdateImageProfile

        //   ([FromBody] ImageEditModel employeerProfile, [FromRoute] int id)
        //{
        //    var profile = await ctx.employeerProfiles
        //        .Where(item => item.EmployeerId == id)
        //        .Include(item => item.employeerBranches)
        //        .Include(item => item.employeerEmails)
        //        .FirstOrDefaultAsync();


        //    if (profile == null)
        //    {
        //        return NotFound("This profile is not found");
        //    }

        //    profile.ImageFile = employeerProfile.ImageFile;       
        //    await ctx.SaveChangesAsync();

        //    return NoContent();
        //}
        [HttpPut("api/profiles/image/{id}")]
        public async Task<ActionResult<string>> UpdateImageProfile

           ([FromBody] ImageEditModel img, [FromRoute] int id)
        {
            var profile = await ctx.employeerProfiles
                .Where(item => item.EmployeerId == id)
                .Include(item => item.employeerBranches)
                .Include(item => item.employeerEmails)
                .FirstOrDefaultAsync();


            if (profile == null)
            {
                return NotFound("This profile is not found");
            }
            MemoryStream ms = new MemoryStream(img.ImageFile, 0, img.ImageFile.Length);
            ms.Write(img.ImageFile, 0, img.ImageFile.Length);
            IFormFile formFile = new FormFile(ms, 0, ms.Length, profile.AboutUs, ".jpg");
            string newFileName = string.Empty;
            string fn = formFile.FileName;
            string extension = Path.GetExtension(fn);
            newFileName = Guid.NewGuid().ToString() + extension;
            string fileName = Path.Combine(_host.WebRootPath + "/images", newFileName);
            await formFile.CopyToAsync(new FileStream(fileName, FileMode.Create));

            profile.ImageFile = newFileName;
            await ctx.SaveChangesAsync();


            return newFileName;
        }

        [HttpDelete("api/profiles/branch/{id}")]
        public async Task<IActionResult> Delete([FromBody] IEnumerable<VacancyIdModel> vacids, int id)
        {
            var Item = await ctx.employeerProfiles
               .Where(p => p.EmployeerId == id)
               .FirstOrDefaultAsync();
       
            var branches = await ctx.employeerBranches.Where(p => p.EmployeerProfileId == Item.Id).ToListAsync();
                foreach (var item in branches)
            {

                foreach (var item1 in vacids)
                {
                    if (item.Id == item1.Id)
                    {
                        var vacancy =await ctx.vacancies.Where(p => p.BranchId == item.Id).FirstOrDefaultAsync();
                        if (vacancy != null)
                        {
                            ctx.vacancies.Remove(vacancy);
                            await ctx.SaveChangesAsync();
                        }
                        ctx.employeerBranches.Remove(item);
                        await ctx.SaveChangesAsync();
                        


                    }
                }



            }
            return Ok();

        }

    }
}
