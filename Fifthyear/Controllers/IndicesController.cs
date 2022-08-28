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
    public class IndicesController : ControllerBase
    {
        private readonly Context ctx;
        public IndicesController(Context ctx)
        {
            this.ctx = ctx;
        }
        [HttpGet("api/indices/universities")]
        public async Task<ActionResult<IEnumerable<Indice>>> GetUniversities()
        {
            var Items = await ctx.indices.Where(p=>p.Module==1).ToListAsync();
            return Ok(Items);
        }
        [HttpGet("api/indices/skills")]
        public async Task<ActionResult<IEnumerable<Indice>>> GetSkills()
        {
            var Items = await ctx.indices.Where(p => p.Module == 0).ToListAsync();
            return Ok(Items);
        }
        [HttpGet("api/indices/educationspecialties")]
        public async Task<ActionResult<IEnumerable<Indice>>> GetEducationSpecialties()
        {
            var Items = await ctx.indices.Where(p => p.Module == 2).ToListAsync();
            return Ok(Items);
        }

    }
}
