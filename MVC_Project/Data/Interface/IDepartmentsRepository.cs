using MVC_Project.Models;

namespace MVC_Project.Data.Interface
{
    public interface IDepartmentsRepository:IRepository<Department>
    {
        Task<Company> GetCompanyById(string comId);
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Department>> GetAllAsync(string comId);
    }
}
