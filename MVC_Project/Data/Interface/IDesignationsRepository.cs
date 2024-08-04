﻿using MVC_Project.Models;

namespace MVC_Project.Data.Interface
{
    public interface IDesignationsRepository:IRepository<Designation>
    {
        Task<Company> GetCompanyById(string comId);
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Designation>> GetAllAsync(string comId);
    }
}
