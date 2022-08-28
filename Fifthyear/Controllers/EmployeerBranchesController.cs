using Fifthyear.Data;
using Fifthyear.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Controllers
{
    public class EmployeerBranchesController : ControllerBase
    {
        private readonly Context ctx;
      
        public EmployeerBranchesController(Context ctx )
        {
            this.ctx = ctx;
            //_hostingEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
      //  [HttpGet("api/profilebbs/{id}")]
        public async Task<ActionResult<IEnumerable<EmployeerBranch>>> GetProfiles(int id)
        {
            var item = await ctx.employeerProfiles
                .Where(p => p.EmployeerId == id)
               .FirstOrDefaultAsync();
            var Items = await ctx.employeerBranches
                .Where(p=>p.EmployeerProfileId==item.Id)
               
               .ToListAsync();
            return Ok(Items);
        }
      //  [HttpGet("api/profilebbs")]
        public async Task<ActionResult<IEnumerable<EmployeerBranch>>> GetProfilesall()
        {
           
            var Items = await ctx.employeerBranches
      

               .ToListAsync();
            return Ok(Items);
        }
       // [HttpDelete("api/profilebbs/{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var employeerBranches = await ctx.employeerBranches.FindAsync(id);
            if (employeerBranches == null)
            {
                return NotFound("This vacancy is not found");
            }

            ctx.employeerBranches.Remove(employeerBranches);
            await ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}
