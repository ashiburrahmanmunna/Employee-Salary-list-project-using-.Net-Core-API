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
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftsRepository shiftsRepository;

        public ShiftsController(IShiftsRepository shiftsRepository)
        {
            this.shiftsRepository = shiftsRepository;
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                var departments = await shiftsRepository.GetAllAsync();
                return Ok(departments);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Shifts/GetById?id=5
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var department = await shiftsRepository.Single(x => x.ShiftId == id);
                if (department == null)
                {
                    return NotFound();
                }
                return Ok(department);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Shifts
        [HttpPost]
        public async Task<IActionResult> Create(Shift shift)
        {
            try
            {
                await shiftsRepository.Create(shift);
                return Ok("Save Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Shifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, Shift shift)
        {
            try
            {
                if (id != shift.ShiftId)
                {
                    return BadRequest("ID Mismatch");
                }

                var updatedShift = await shiftsRepository.Update(shift, id);
                if (updatedShift == null)
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

        // DELETE: api/Shifts?id=5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await shiftsRepository.Delete(id);
                return Ok("Deleted Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }

}
