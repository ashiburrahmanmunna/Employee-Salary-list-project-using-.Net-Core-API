using MVC_Project_Repository_Pattern.Models;

namespace MVC_Project_Repository_Pattern.Datas.Interface
{
    public interface IDesignationsRepository:IRepository<Designation>
    {
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Designation>> GetAllAsync(string comId);
    }
}
