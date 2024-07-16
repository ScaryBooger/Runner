using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class DashboardRepository : IDashboardRepsitory
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DashboardRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Club>> GetAllUserClubs()

        {
            var userClub = httpContextAccessor.HttpContext?.User;
            var userClubs =dbContext.Clubs.Where(r => r.AppUserId == userClub.GetUserId());
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUuserRaces()
        {
            var userRace = httpContextAccessor.HttpContext?.User;
            var userRaces = dbContext.Races.Where(r => r.AppUserId == userRace.GetUserId());
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return await dbContext.Users.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(AppUser usr)
        {
            dbContext.Update(usr);
            return Save();
        }
        public bool Save() { 
        
            var saved=dbContext.SaveChanges();
            return saved >0? true: false;
        
        }
    }
}
