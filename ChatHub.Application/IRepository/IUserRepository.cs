using ChatHub.Domain.Entities;

namespace ChatHub.Application.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
 