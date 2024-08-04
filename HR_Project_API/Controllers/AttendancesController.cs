using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project_Repository_Pattern.Datas;
using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace HR_Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            var applicationDbContext = _context.Attendances.Include(a => a.Company).Include(a => a.Employee);
            return Ok(await applicationDbContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
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

            return Ok(attendance);
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

            return Ok("Created Successfully");
        }

        // POST: Attendances/Edit/5
        [HttpPut]
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
                return Ok("Updated Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        // POST: Attendances/Delete/5
        [HttpDelete]
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
            return Ok("Deleted Successfully");
        }
    }


}