using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Implementation
{
    public class SalariesRepository : Repository<Department>, ISalariesRepository
    {
        public SalariesRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
