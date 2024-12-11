using ChatHub.Application.IRepository;

namespace ChatHub.Application
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IMessageRepository MessageRepository { get; }

        public Task<int> SaveChangeAsync();

        void SaveChanges();

        void BeginTransaction();

        void Commit();
    }
}
