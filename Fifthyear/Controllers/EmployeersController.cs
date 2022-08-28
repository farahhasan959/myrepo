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
    public class EmployeersController: ControllerBase
    {
        private readonly Context ctx;
        public EmployeersController(Context ctx)
        {
            this.ctx = ctx;
        }
        [HttpGet("api/employeers")]
        public async Task<ActionResult<IEnumerable<Employeer>>> GetEmployeers()
        {
            var Items = await ctx.employeers
               .ToListAsync();
            return Ok(Items);
        }
      
    }
}
