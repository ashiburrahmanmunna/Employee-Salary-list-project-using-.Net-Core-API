using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Implementation
{
    public class ShiftsRepository : Repository<Shift>, IShiftsRepository
    {
        public ShiftsRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
