using Fifthyear.Data;
using Fifthyear.Models;
using Fifthyear.Services;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Controllers
{
    public class AlgorithmOperationsController : ControllerBase
    {
        private readonly Context ctx;
      
        public AlgorithmOperationsController(Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("api/companymatching/{id}")]
        public async Task<ActionResult<IEnumerable<AlgorithmResult>>> Getmatchestocompany(int id)
        {
            var results =await ctx.algorithmResults.Where(p => p.CompanyId == id).ToListAsync();
            return Ok(results);


        }
        [HttpDelete("api/company")]
        public async Task<ActionResult<IEnumerable<AlgorithmResult>>> delete()
        {
            var results =await ctx.algorithmResults.ToListAsync();
            foreach(var i in results)
            {
                ctx.algorithmResults.Remove(i);
                await ctx.SaveChangesAsync();
            }
            return Ok();


        }
        [HttpDelete("api/compahhhhhhhny")]
        public async Task<ActionResult<IEnumerable<AlgorithmResult>>> deleteeeeeee()
        {
            var results = await ctx.notifications.ToListAsync();
            foreach (var i in results)
            {
                ctx.notifications.Remove(i);
                await ctx.SaveChangesAsync();
            }
            return Ok();


        }






        [HttpGet("api/jobseekermatching/{id}")]
        public async Task<ActionResult<IEnumerable<AlgorithmResult>>> Getmatchestoseeker(int id)
        {
            var results =await ctx.algorithmResults.Where(p => p.SeekerId == id).ToListAsync();
          
           return Ok(results);
           


        }
       
    }
}
