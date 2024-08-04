using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project_Repository_Pattern.Datas;
using MVC_Project_Repository_Pattern.Models;

namespace HR_Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceSummariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceSummariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AttendanceSummaries
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                var attendanceSummaries = await _context.AttendanceSummaries
                    .Include(a => a.Company)
                    .Include(a => a.Employee)
                    .ToListAsync();

                return Ok(attendanceSummaries);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/AttendanceSummaries/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var attendanceSummary = await _context.AttendanceSummaries
                    .Include(a => a.Company)
                    .Include(a => a.Employee)
                    .FirstOrDefaultAsync(m => m.ComId == id);

                if (attendanceSummary == null)
                {
                    return NotFound();
                }

                return Ok(attendanceSummary);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/AttendanceSummaries
        [HttpPost]
        public async Task<IActionResult> Create(AttendanceSummary attendanceSummary)
        {
            try
            {
                var comId = attendanceSummary.ComId;
                var dtMonth = attendanceSummary.dtMonth;
                var dtYear = attendanceSummary.dtYear;

                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertIntoTempAndCalculateCounts {comId}, {dtMonth}, {dtYear}");

                return Ok("Created Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/AttendanceSummaries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, AttendanceSummary attendanceSummary)
        {
            try
            {
                if (id != attendanceSummary.ComId)
                {
                    return BadRequest("ID Mismatch");
                }

                _context.Update(attendanceSummary);
                await _context.SaveChangesAsync();

                return Ok("Updated Successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceSummaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // DELETE: api/AttendanceSummaries/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var attendanceSummary = await _context.AttendanceSummaries.FindAsync(id);
                if (attendanceSummary == null)
                {
                    return NotFound();
                }

                _context.AttendanceSummaries.Remove(attendanceSummary);
                await _context.SaveChangesAsync();

                return Ok("Deleted Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private bool AttendanceSummaryExists(string id)
        {
            return _context.AttendanceSummaries.Any(e => e.ComId == id);
        }
    }

}
