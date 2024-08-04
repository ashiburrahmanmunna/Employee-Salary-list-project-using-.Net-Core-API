using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project.Data;
using MVC_Project.Models;

namespace MVC_Project.Controllers
{
    public class SalariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salaries
        public async Task<IActionResult> Index()
        {
            var comId = Request.Cookies["ComId"];
            if (string.IsNullOrEmpty(comId))
            {
                return Problem("ComId cookie is missing or invalid.");
            }
            var applicationDbContext = _context.Salaries.Include(s => s.Company).Include(s => s.Employee).Where(s=>s.ComId==comId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Salaries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Salaries == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Company)
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.ComId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salaries/Create
        public IActionResult Create()
        {
            var comId = Request.Cookies["ComId"];
            if (string.IsNullOrEmpty(comId))
            {
                return Problem("ComId cookie is missing or invalid.");
            }
            var company = _context.Companies.FirstOrDefault(c => c.ComId == comId);
            if (company == null)
            {
                return Problem("Company not found for the given ComId.");
            }

            var employees = _context.Employees.Where(e => e.ComId == comId).ToList();
            var empIdSelectList = new SelectList(employees, "EmpId", "EmpId");
            var empNameSelectList = new SelectList(employees, "EmpName", "EmpName");

            ViewData["ComId"] = new SelectList(new List<Company> { company }, "ComId", "ComId");
            ViewData["ComName"] = new SelectList(new List<Company> { company }, "ComName", "ComName");
            ViewData["EmpId"] = empIdSelectList;
            ViewData["EmpName"] = empNameSelectList;
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salary salary) //[Bind("ComId,EmpId,dtMonth,dtYear,Gross,Basic,HRent,Medical,Others,AbsentAmount,PaymentAmount,IsPaid,PaidAmount")] 
        {
            //if (ModelState.IsValid)
            //{
            var comId = salary.ComId;
            var empId = salary.EmpId;
            var dtYear = salary.dtYear;
            var dtMonth = salary.dtMonth;
            var isPaid = salary.IsPaid;

            await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC CalculateSalary {comId}, {empId}, {dtYear},{dtMonth},{isPaid}");

            return RedirectToAction(nameof(Index));
            //}
            ViewData["ComId"] = new SelectList(_context.Companies, "ComId", "ComId", salary.ComId);
            ViewData["EmpId"] = new SelectList(_context.Employees, "EmpId", "EmpId", salary.EmpId);
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Salaries == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries.FirstOrDefaultAsync(a => a.EmpId == id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["ComId"] = new SelectList(_context.Companies, "ComId", "ComId", salary.ComId);
            ViewData["EmpId"] = new SelectList(_context.Employees, "EmpId", "EmpId", salary.EmpId);
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ComId,EmpId,dtMonth,dtYear,Gross,Basic,HRent,Medical,Others,AbsentAmount,PaymentAmount,IsPaid,PaidAmount")] Salary salary)
        {
            if (id != salary.EmpId)
            {
                return NotFound();
            }

          
            try
            {
                var comId = salary.ComId;
                var empId = salary.EmpId;
                var dtYear = salary.dtYear;
                var dtMonth = salary.dtMonth;
                var isPaid = salary.IsPaid;

                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC CalculateSalary {comId}, {empId}, {dtYear},{dtMonth},{isPaid}");
                //_context.Update(salary);
                //await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            ViewData["ComId"] = new SelectList(_context.Companies, "ComId", "ComId", salary.ComId);
            ViewData["EmpId"] = new SelectList(_context.Employees, "EmpId", "EmpId", salary.EmpId);
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Salaries == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Company)
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Salaries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Salaries'  is null.");
            }
            var salary = await _context.Salaries.FirstOrDefaultAsync(x=>x.EmpId==id);
            if (salary != null)
            {
                _context.Salaries.Remove(salary);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(string id)
        {
          return (_context.Salaries?.Any(e => e.ComId == id)).GetValueOrDefault();
        }
    }
}
