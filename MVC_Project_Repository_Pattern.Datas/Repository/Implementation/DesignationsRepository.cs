using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

using Microsoft.EntityFrameworkCore;

namespace MVC_Project_Repository_Pattern.Datas.Implementation
{
    public class DesignationsRepository : Repository<Designation>, IDesignationsRepository
    {
        public DesignationsRepository(ApplicationDbContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            return await db.Set<Company>().ToListAsync();
        }
        public async Task<IEnumerable<Designation>> GetAllAsync(string comId)
        {
            return await db.Designations
                .Include(d => d.Company)
                .Where(d => d.ComId == comId)
                .ToListAsync();
        }
    }
}
