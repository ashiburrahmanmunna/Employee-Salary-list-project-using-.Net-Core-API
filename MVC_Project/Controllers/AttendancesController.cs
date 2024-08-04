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
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
             var comId = Request.Cookies["ComId"];
            if (string.IsNullOrEmpty(comId))
            {
                return Problem("ComId cookie is missing or invalid.");
            }
            var applicationDbContext = _context.Attendances.Include(a => a.Company).Include(a => a.Employee).Where(a => a.ComId == comId); ;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Company)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.ComId == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
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

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComId,EmpId,dtDate,InTime,OutTime")] Attendance attendance)
        {
            // Retrieve Shift details based on ComId
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.ComId == attendance.ComId);

            // Calculate AttStatus based on InTime and Shift details
            if (attendance.InTime == null)
            {
                attendance.AttStatus = "A";
            }
            else if (attendance.InTime > shift.ShiftOut.TimeOfDay)
            {
                attendance.AttStatus = "L";
            }
            else if (attendance.InTime > shift.ShiftIn.TimeOfDay && attendance.InTime <= shift.ShiftLate.TimeOfDay)
            {
                attendance.AttStatus = "L";
            }
            else if (attendance.InTime <= shift.ShiftIn.TimeOfDay)
            {
                attendance.AttStatus = "P";
            }
            else if (attendance.InTime == TimeSpan.Zero)
            {
                attendance.AttStatus = "H";
            }
            else
            {
                attendance.AttStatus = "P";
            }

            // Save the attendance record
            _context.Add(attendance);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ComId,EmpId,dtDate,AttStatus,InTime,OutTime")] Attendance attendance)
        //{
        //    var comId = attendance.ComId;
        //    var empId = attendance.EmpId;
        //    var dtDate = attendance.dtDate;
        //    var inTime = attendance.InTime;
        //    var outTime = attendance.OutTime;

        //    await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateAttendanceStatus {comId}, {empId}, {dtDate}, {inTime}, {outTime}");

        //    return RedirectToAction(nameof(Index));


        //    _context.Add(attendance);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    //}
        //    ViewData["ComId"] = new SelectList(_context.Companies, "ComId", "ComId", attendance.ComId);
        //    ViewData["EmpId"] = new SelectList(_context.Employees, "EmpId", "EmpId", attendance.EmpId);
        //    return View(attendance);
        //}

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FirstOrDefaultAsync(a=>a.AttendanceId == id);  //FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["ComId"] = new SelectList(_context.Companies, "ComId", "ComId", attendance.ComId);
            ViewData["EmpId"] = new SelectList(_context.Employees, "EmpId", "EmpId", attendance.EmpId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComId,EmpId,AttStatus,dtDate,InTime,OutTime")] Attendance attendance)
        {
            //if (id != attendance.AttendanceId)
            //{
            //    return NotFound();
            //}

            try
            {
                // Retrieve Shift details based on ComId
                var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.ComId == attendance.ComId);

                // Calculate AttStatus based on InTime and Shift details
                if (attendance.InTime == null)
                {
                    attendance.AttStatus = "A";
                }
                else if (attendance.InTime > shift.ShiftOut.TimeOfDay)
                {
                    attendance.AttStatus = "A";
                }
                else if (attendance.InTime > shift.ShiftIn.TimeOfDay && attendance.InTime <= shift.ShiftLate.TimeOfDay)
                {
                    attendance.AttStatus = "L";
                }
                else if (attendance.InTime <= shift.ShiftIn.TimeOfDay)
                {
                    attendance.AttStatus = "P";
                }
                else if (attendance.InTime == TimeSpan.Zero)
                {
                    attendance.AttStatus = "H";
                }
                else
                {
                    attendance.AttStatus = "P";
                }

                // Update the attendance record
                // Delete the existing attendance record
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();

                // Create a new attendance record with the updated ComId
                var newAttendance = new Attendance
                {
                    ComId = attendance.ComId,
                    EmpId = attendance.EmpId,
                    AttStatus = attendance.AttStatus,
                    dtDate = attendance.dtDate,
                    InTime = attendance.InTime,
                    OutTime = attendance.OutTime
                };

                _context.Attendances.Add(newAttendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(attendance.EmpId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("ComId,EmpId,AttStatus,dtDate,InTime,OutTime")] Attendance attendance) 
        //{
        //    if (id != attendance.EmpId)
        //    {
        //        return NotFound();
        //    }

        //    //if (ModelState.IsValid)
        //    //{
        //    try
        //    {
        //        var comId = attendance.ComId;
        //        var empId = attendance.EmpId;
        //        var dtDate = attendance.dtDate;
        //        var inTime = attendance.InTime;
        //        var outTime = attendance.OutTime;

        //        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateAttendanceStatus {comId}, {empId}, {dtDate}, {inTime}, {outTime}");

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (DbUpdateConcurrencyException e)
        //    {
        //        if (!AttendanceExists(attendance.EmpId))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //    //}
        //    ViewData["ComId"] = new SelectList(_context.Companies, "ComId", "ComId", attendance.ComId);
        //    ViewData["EmpId"] = new SelectList(_context.Employees, "EmpId", "EmpId", attendance.EmpId);
        //    return View(attendance);
        //}

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Company)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Attendances == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Attendances'  is null.");
            }
            //var attendance = await _context.Attendances.FindAsync(id);
            var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.EmpId == id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(string id)
        {
          return (_context.Attendances?.Any(e => e.ComId == id)).GetValueOrDefault();
        }
    }
}
