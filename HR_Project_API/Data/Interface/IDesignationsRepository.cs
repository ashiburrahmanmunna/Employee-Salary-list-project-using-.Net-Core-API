using HR_Project_API.Models;

namespace HR_Project_API.Data.Interface
{
    public interface IDesignationsRepository:IRepository<Designation>
    {
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Designation>> GetAllAsync(string comId);
    }
}
