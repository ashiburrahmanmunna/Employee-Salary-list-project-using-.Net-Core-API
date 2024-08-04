﻿using HR_Project_API.Data.Interface;
using HR_Project_API.Models;

namespace HR_Project_API.Data.Implementation
{
    public class AttendancesRepository : Repository<Department>, IAttendancesRepository
    {
        public AttendancesRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
