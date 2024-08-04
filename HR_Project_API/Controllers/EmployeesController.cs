using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project_Repository_Pattern.Datas;
using MVC_Project_Repository_Pattern.Datas.Implementation;
using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace HR_Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesRepository employeesRepository;

        public EmployeesController(IEmployeesRepository employeesRepository)
        {
            this.employeesRepository = employeesRepository;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IActionResult> GetAllData(string comid, string deptid, string desigid)
        {
            try
            {
                var employeeList = await employeesRepository.GetAllAsync(comid, deptid, desigid);
                return Ok(employeeList);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Employees/Details/5
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var singleEmployee = await employeesRepository.Single(x => x.EmpId == id);
                if (singleEmployee == null)
                {
                    return NotFound();
                }
                return Ok(singleEmployee);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Employees/Create
        //[HttpGet("Create")]
        //public async Task<IActionResult> Create(string id)
        //{
        //    try
        //    {
        //        var companies = await employeesRepository.GetCompanies();
        //        var departments = await employeesRepository.GetDepartments();
        //        var designations = await employeesRepository.GetDesignations();
        //        var shifts = await employeesRepository.GetShifts();

        //        if (string.IsNullOrWhiteSpace(id))
        //        {
        //            return Ok(new Employee() { ComId = string.Empty });
        //        }
        //        else
        //        {
        //            var singleEmployee = await employeesRepository.Single(x => x.ComId == id);
        //            if (singleEmployee == null)
        //                return NotFound();
        //            return Ok(singleEmployee);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        // POST: api/Employees/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee)
            {
            try
            {
                string dtJoinString = employee.dtJoin.ToString("o");
                await employeesRepository.CreateEmployee(employee.ComId, employee.EmpId, employee.EmpCode, employee.EmpName, employee.ShiftId, employee.DeptId, employee.DesigId, employee.Gender, employee.Gross, dtJoinString);
                return Ok("Created Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Employees/Edit/5
        //[HttpGet("Edit/{id}")]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    try
        //    {
        //        var companies = await employeesRepository.GetCompanies();
        //        var departments = await employeesRepository.GetDepartments();
        //        var designations = await employeesRepository.GetDesignations();
        //        var shifts = await employeesRepository.GetShifts();

        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var employee = await employeesRepository.GetById(id);
        //        if (employee == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(employee);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        // PUT: api/Employees/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.EmpId)
                {
                    return BadRequest("ID Mismatch");
                }

                var updatedEmployee = await employeesRepository.Update(employee, id);
                if (updatedEmployee == null)
                {
                    return NotFound();
                }
                return Ok("Updated Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Employees/Delete/5
        //[HttpGet("Delete/{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    try
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var employee = await employeesRepository.GetById(id);
        //        if (employee == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(employee);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }
        //}

        // DELETE: api/Employees/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var attendances = await employeesRepository.Where(a => a.EmpId == id).ToListAsync();
                foreach (var attendance in attendances)
                {
                    await employeesRepository.Delete(attendance.EmpId);
                }

                await employeesRepository.Delete(id);

                return Ok("Deleted Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }

}
