using Microsoft.EntityFrameworkCore;

using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Implementation
{
    public class DepartmentsRepository : Repository<Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(ApplicationDbContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            return await db.Set<Company>().ToListAsync();
        }
        public async Task<IEnumerable<Department>> GetAllAsync(string comId)
        {
            return await db.Departments
                .Include(d => d.Company)
                .Where(d => d.ComId == comId)
                .ToListAsync();
        }

    }
}
