using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<AppUser>> GetAppUsers();
        Task<AppUser> GetUserById(string id);
        bool Update(AppUser user);
        bool Add(AppUser user);
        bool Delete(AppUser user);
        bool Save();

    }
}
