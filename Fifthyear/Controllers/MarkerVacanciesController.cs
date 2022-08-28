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
    public class MarkerVacanciesController : ControllerBase
    {
        private readonly Context ctx;
        public MarkerVacanciesController(Context ctx)
        {
            this.ctx = ctx;
        }
        [HttpGet("api/markervacancies")]
        public async Task<ActionResult<IEnumerable<MarkedVacancy>>> GetMarkerVacancies()
        {
            var Items = await ctx.markedVacancies
                .Include(P=>P.JobSeeker)
                .Include(e=>e.Vacancy)
                .ToListAsync();
            return Ok(Items);
        }
        //[HttpGet("api/markervacancies/{id}")]
        //public async Task<ActionResult<List<int>>> GetMarkerVacanciesForSeeker(int id)
        //{
        //    var Items = await ctx.markedVacancies
        //        .Where(p=>p.JobSeekerID==id)
        //        .Include(e => e.Vacancy)
        //        .ToListAsync();
        //    List<int> ids = new List<int>();
        //    foreach(var item in Items)
        //    {
        //        ids.Add(item.VacancyId);
        //    }
        //    return Ok(ids);
        //}
        [HttpPost("api/markervacancies/{seekerid}/{vacancyid}")]
        public async Task<ActionResult> Create(int seekerid,int vacancyid)
        {
           
                MarkedVacancy markerVac = new MarkedVacancy();
                markerVac.VacancyId = vacancyid;
                markerVac.JobSeekerID = seekerid;
                await ctx.markedVacancies.AddAsync(markerVac);
                await ctx.SaveChangesAsync();
            
            return Ok();
        }
        [HttpDelete("api/markervacancies/{seekerid}/{vacancyid}")]
        public async Task<IActionResult> Delete(int seekerid, int vacancyid)
        {
            var Items = await ctx.markedVacancies
               .Where(p => p.JobSeekerID == seekerid)
               .ToListAsync();
            foreach (var item in Items)
            {
                
                if (item.VacancyId ==vacancyid)
            {
                ctx.markedVacancies.Remove(item);
                await ctx.SaveChangesAsync();

            }

            }
           
            return Ok();

        }
    }
}
