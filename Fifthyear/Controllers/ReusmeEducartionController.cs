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
    public class ReusmeEducartionController :ControllerBase
    {
        private readonly Context ctx;
   
    public ReusmeEducartionController(Context ctx)
    {
        this.ctx = ctx;
    }
       // [HttpGet("api/rrrr")]
        public async Task<ActionResult<IEnumerable<ReusmeEducation>>> GetEmployeers()
        {
            var Items = await ctx.reusmeEducations
               .ToListAsync();
            return Ok(Items);
        }
       // [HttpPut("api/rrrrrr/{id}")]
        public async Task<ActionResult<ReusmeEducation>> UpdateReusme

          ([FromBody] ReusmeEducation reusme, [FromRoute] int id)
        {
            var reus = await ctx.reusmeEducations.Where(item => item.Id == id).FirstOrDefaultAsync();

            if (reus == null)
            {
                return NotFound();
            }
            reus.Specialization = reusme.Specialization;
           
            await ctx.SaveChangesAsync();
            return NoContent();
        }
        //[HttpGet("api/vv")]
        public async Task<ActionResult<IEnumerable<VacancyEducation>>> GetEhhmployeers()
        {
            var Items = await ctx.vacancyEducations
               .ToListAsync();
            return Ok(Items);
        }
       // [HttpPut("api/vvvv/{id}")]
        public async Task<ActionResult<VacancyEducation>> UpdatejjReusme

          ([FromBody] VacancyEducation reusme, [FromRoute] int id)
        {
            var reus = await ctx.vacancyEducations.Where(item => item.Id == id).FirstOrDefaultAsync();

            if (reus == null)
            {
                return NotFound();
            }
            reus.Specialization = reusme.Specialization;

            await ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
