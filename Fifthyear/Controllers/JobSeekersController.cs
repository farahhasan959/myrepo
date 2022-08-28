using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fifthyear.Data;
using Fifthyear.Models;

namespace Fifthyear.Controllers
{
    public class JobSeekersController : ControllerBase
    {
        private readonly Context ctx;
        public JobSeekersController(Context ctx)
        {
            this.ctx = ctx;
        }
        [HttpGet("api/jobseekers")]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> GetJobSeekers()
        {
            var Items = await ctx.jobSeekers
                
              
                .ToListAsync();

            return Ok(Items);
        }

        
    }
}
