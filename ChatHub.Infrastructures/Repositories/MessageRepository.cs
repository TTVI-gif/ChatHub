using ChatHub.Application.IRepository;
using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.ViewModel.Messages;
using Microsoft.EntityFrameworkCore;

namespace ChatHub.Infrastructures.Repositories
{
    internal class MessageRepository : GenericRepository<Messages>, IMessageRepository
    {
        public MessageRepository(ChatDbContext context) : base(context)
        {
        }

        public async Task<Dictionary<Guid, MessageResponseModel>> GetMessageByLoginIdAsync(Guid loginId)
        {
            var result = await _dbSet.Where(x => (x.SenderId == loginId || x.ReceiverId == loginId) && !x.IsDeleted)
                .OrderByDescending(x => x.ModificationDate)
                .GroupBy(x => x.SenderId != loginId ? (x.SenderId.HasValue ? x.SenderId.Value : Guid.Empty) : (x.ReceiverId.HasValue ? x.ReceiverId.Value : Guid.Empty))
                .ToDictionaryAsync(
                    x => x.Key,
                    p => p.OrderByDescending(x => x.ModificationDate) // Sắp xếp theo ModificationDate lớn nhất trong từng nhóm
                    .Select(x => new MessageResponseModel
                    {
                        Content = x.Content,
                        ModifiDateMessage = x.ModificationDate,
                    })                  // Lấy tin nhắn có ModificationDate lớn nhất
                    .FirstOrDefault()! // Sử dụng toán tử ! để loại bỏ nullable
                );

            return result;
        }

        public async Task<IList<Messages>> GetMessageByRecievedIdAsync(Guid receivedId)
        {
            return await _dbSet.Where(x => x.ReceiverId == receivedId && x.IsDeleted != true).ToListAsync();
        }
    }
}
