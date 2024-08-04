using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Implementation
{
    public class AttendancesRepository : Repository<Department>, IAttendancesRepository
    {
        public AttendancesRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
