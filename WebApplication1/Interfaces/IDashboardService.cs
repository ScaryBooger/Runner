using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IDashboardRepsitory
    {
        Task<List<Race>> GetAllUuserRaces();
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);

        bool Update(AppUser usr);
        bool Save();

    }
}
