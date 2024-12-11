using ChatHub.Application;
using ChatHub.Application.IRepository;

namespace ChatHub.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        public UnitOfWork(
           ChatDbContext chatDbContext,
           IUserRepository userRepository,
           IMessageRepository messageRepository)
        {
            _chatDbContext = chatDbContext;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }
        public IUserRepository UserRepository { get => _userRepository; }

        public IMessageRepository MessageRepository { get => _messageRepository; }

        public void BeginTransaction()
        {
            _chatDbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _chatDbContext.Database.CommitTransaction();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _chatDbContext.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            //this.RunBeforeSave(this.dbContextScope);
            this._chatDbContext.SaveChanges();
        }
    }
}
