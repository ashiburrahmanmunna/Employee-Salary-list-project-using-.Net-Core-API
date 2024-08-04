using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using MVC_Project.Data.Interface;
using MVC_Project.Models;

namespace MVC_Project.Data.Implementation
{
    public class EmployeesRepository : Repository<Employee>, IEmployeesRepository
    {
        public EmployeesRepository(ApplicationDbContext db) : base(db)
        {
        }
        public async Task<Company> GetCompanyById(string comId)
        {
            return await db.Set<Company>().FirstOrDefaultAsync(c => c.ComId == comId);
        }
        public async Task<List<Department>> GetDepartmentsByComId(string comId)
        {
            // Implement the retrieval of departments by ComId from the database
            var departments = await db.Departments.Where(d => d.ComId == comId).ToListAsync();
            return departments;
        }

        public async Task<List<Designation>> GetDesignationsByComId(string comId)
        {
            // Implement the retrieval of designations by ComId from the database
            var designations = await db.Designations.Where(d => d.ComId == comId).ToListAsync();
            return designations;
        }

        public async Task<List<Shift>> GetShiftsByComId(string comId)
        {
            // Implement the retrieval of shifts by ComId from the database
            var shifts = await db.Shifts.Where(s => s.ComId == comId).ToListAsync();
            return shifts;
        }

        public async Task CreateEmployee(string comId, string empId, string empCode, string empName, string shiftId, string deptId, string desigId, string gender, decimal gross, string dtJoin)
        {
            var parameters = new[]
            {
        new SqlParameter("@ComId", comId),
        new SqlParameter("@EmpId", empId),
        new SqlParameter("@EmpCode", empCode),
        new SqlParameter("@EmpName", empName),
        new SqlParameter("@ShiftId", shiftId),
        new SqlParameter("@DeptId", deptId),
        new SqlParameter("@DesigId", desigId),
        new SqlParameter("@Gender", gender),
        new SqlParameter("@Gross", gross),
        new SqlParameter("@dtJoin", dtJoin)
    };

            await db.Database.ExecuteSqlRawAsync("EXEC data_entry_in_employee @ComId, @EmpId, @EmpCode, @EmpName, @ShiftId, @DeptId, @DesigId, @Gender, @Gross, @dtJoin", parameters);
        }

        public async Task<IEnumerable<SelectListItem>> GetCompanies()
        {
            var companies = await db.Set<Company>().ToListAsync();
            return companies.Select(c => new SelectListItem
            {
                Value = c.ComId.ToString(),
                Text = c.ComName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetDepartments()
        {
            var departments = await db.Set<Department>().ToListAsync();
            return departments.Select(d => new SelectListItem
            {
                Value = d.DeptId.ToString(),
                Text = d.DeptName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetDesignations()
        {
            var designations = await db.Set<Designation>().ToListAsync();
            return designations.Select(d => new SelectListItem
            {
                Value = d.DesigId.ToString(),
                Text = d.DesigName
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetShifts()
        {
            var shifts = await db.Set<Shift>().ToListAsync();
            return shifts.Select(s => new SelectListItem
            {
                Value = s.ShiftId.ToString(),
                Text = s.ShiftName
            });
        }
        public async Task<IEnumerable<Employee>> GetAllAsync(string comId)
        {
            return await db.Employees
                .Include(d => d.Company)
                .Where(d => d.ComId == comId)
                .ToListAsync();
        }

    }
}
