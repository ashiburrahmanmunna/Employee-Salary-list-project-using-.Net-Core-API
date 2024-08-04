using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Interface
{
    public interface IDepartmentsRepository:IRepository<Department>
    {
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Department>> GetAllAsync(string comId);
    }
}
