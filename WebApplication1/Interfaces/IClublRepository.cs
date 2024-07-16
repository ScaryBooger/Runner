using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IClublRepository

    {
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNotracking(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();



    }
}
