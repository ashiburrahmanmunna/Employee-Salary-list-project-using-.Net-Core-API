using MVC_Project_Repository_Pattern.Datas.Interface;
using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Implementation
{
    public class AttendanceSummariesRepository : Repository<Department>, IAttendanceSummariesRepository
    {
        public AttendanceSummariesRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
