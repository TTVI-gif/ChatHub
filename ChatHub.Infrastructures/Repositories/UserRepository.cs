using ChatHub.Application.IRepository;
using ChatHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatHub.Infrastructures.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext context) : base(context)
        {

        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            username = username.Trim();
            return await _dbSet.FirstOrDefaultAsync(x => username.Equals(x.UserName) && !x.IsDeleted);
        }
    }
}
