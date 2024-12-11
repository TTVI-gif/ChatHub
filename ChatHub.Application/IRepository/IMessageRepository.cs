using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.ViewModel.Messages;

namespace ChatHub.Application.IRepository
{
    public interface IMessageRepository : IGenericRepository<Messages>
    {
        Task<IList<Messages>> GetMessageByRecievedIdAsync(Guid receivedId);

        Task<Dictionary<Guid, MessageResponseModel>> GetMessageByLoginIdAsync(Guid loginId);
    }
}
