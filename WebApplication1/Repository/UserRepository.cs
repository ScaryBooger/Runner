using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public bool Add(AppUser user)
        {
           context.Add(user);
            return Save();
           
        }

        public bool Delete(AppUser user)
        {
            context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAppUsers()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved=context.SaveChanges();
            return saved >0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            context.Update(user);

            return Save();
        }
    }
}
