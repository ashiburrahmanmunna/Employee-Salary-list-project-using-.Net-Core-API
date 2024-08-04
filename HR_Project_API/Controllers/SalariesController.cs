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
    public class SalariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Salaries
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                var salaryList = await _context.Salaries
                    .Include(s => s.Company)
                    .Include(s => s.Employee)
                    .ToListAsync();

                return Ok(salaryList);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Salaries/Details/5
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var salary = await _context.Salaries
                    .Include(s => s.Company)
                    .Include(s => s.Employee)
                    .FirstOrDefaultAsync(m => m.ComId == id);

                if (salary == null)
                {
                    return NotFound();
                }

                return Ok(salary);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Salaries
        [HttpPost]
        public async Task<IActionResult> Create(Salary salary)
        {
            try
            {
                var comId = salary.ComId;
                var empId = salary.EmpId;
                var dtYear = salary.dtYear;
                var dtMonth = salary.dtMonth;
                var isPaid = salary.IsPaid;

                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC CalculateSalary {comId}, {empId}, {dtYear},{dtMonth},{isPaid}");

                return Ok("Salary created successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Salaries/5
        [HttpPut]
        public async Task<IActionResult> Update(string id, Salary salary)
        {
            try
            {
                if (id != salary.ComId)
                {
                    return BadRequest("ID Mismatch");
                }

                _context.Update(salary);
                await _context.SaveChangesAsync();

                return Ok("Salary updated successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(salary.ComId))
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

        // DELETE: api/Salaries/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var salary = await _context.Salaries.FindAsync(id);
                if (salary == null)
                {
                    return NotFound();
                }

                _context.Salaries.Remove(salary);
                await _context.SaveChangesAsync();

                return Ok("Salary deleted successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private bool SalaryExists(string id)
        {
            return _context.Salaries.Any(e => e.ComId == id);
        }
    }

}
