using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project_Repository_Pattern.Datas;
using MVC_Project_Repository_Pattern.Models;
using MVC_Project_Repository_Pattern.Datas.Interface;

namespace HR_Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignationsController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly IDesignationsRepository designationRepository;
        public DesignationsController(IDesignationsRepository designationRepository)
        {
            this.designationRepository = designationRepository;
        }

        // GET: Designations
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                var departments = await designationRepository.GetAllAsync();

                return Ok(departments);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: Designations/Details/5
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                if (id == null || designationRepository == null)
                {
                    return NotFound();
                }

                var department = await designationRepository.Single(x => x.DesigId == id);
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

        // POST: Designations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Designation designation)
        {
            try
            {
                await designationRepository.Create(designation);
                return Ok("Save Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: Designations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit(string id, [Bind("ComId,DesigId,DesigName")] Designation designation)
        {
            try
            {
                if (id != designation.DesigId)
                {
                    return NotFound();
                }

                //if (ModelState.IsValid)
                //{
                var updatedCompany = await designationRepository.Update(designation, id);
                if (updatedCompany == null)
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

        // POST: Designations/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await designationRepository.Delete(id);

                return Ok("Deleted Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);

            }
            
        }

        //private async Task<bool> DesignationExists(string id)
        //{
        //    var company = await designationRepository.GetById(id);
        //    return company != null;
        //}
    }
}
