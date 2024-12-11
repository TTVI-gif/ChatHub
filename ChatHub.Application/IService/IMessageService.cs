using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.Commons;
using ChatHub.Global.Shared.Message;
using ChatHub.Global.Shared.ViewModel.Messages;

namespace ChatHub.Application.IService
{
    public interface IMessageService
    {
        void Add(MessageRequest message);
        Task<ApiResult<bool>> DeleteMessage(MessageDeleteModel messageDeleteModel);

        Task<ApiResult<List<GetAllMessageResponseModel>>> GetAll();

        Task<ApiResult<List<GetAllMessageResponseModel>>> GetReceivedMessages(Guid userId);

    }
}
