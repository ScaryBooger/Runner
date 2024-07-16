using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext context;

        public RaceRepository(ApplicationDbContext context) { 
            this.context = context;
        }
        public bool Add(Race race)
        {
            context.Add(race);
            return Save();
            
        }

        public bool Delete(Race race)
        {
            context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await context.Races.Include(x => x.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race> GetByIdAsyncNotracking(int id)
        {
            return await context.Races.Include(x => x.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
        {
            return await context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved=context.SaveChanges();
            return saved >0 ? true : false;
        }

        public bool Update(Race race)
        {
           context.Update(race);
            return Save();
        }
    }
}
