using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ClubRepository : IClublRepository
    {
        private readonly ApplicationDbContext context;
        public ClubRepository(ApplicationDbContext context) { 
            this.context = context;
        }

        public bool Add(Club club)
        {   
            context.Add(club);
            return Save();
            
        }

        public bool Delete(Club club)
        {
            context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await context.Clubs.Include(x => x.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetByIdAsyncNotracking(int id)
        {
            return await context.Clubs.Include(x => x.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved=context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool Update(Club club)
        {
            context.Update(club);
            return Save();
        }
    }
}
