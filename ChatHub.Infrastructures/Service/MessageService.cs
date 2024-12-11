using Azure.Core;
using ChatHub.Application;
using ChatHub.Application.IRepository;
using ChatHub.Application.IService;
using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.Commons;
using ChatHub.Global.Shared.Message;
using ChatHub.Global.Shared.ViewModel.Messages;

namespace ChatHub.Infrastructures.Service
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        public MessageService(IUnitOfWork unitOfWork, IMessageRepository messageRepository)
        {
            _unitOfWork = unitOfWork;
            _messageRepository = messageRepository;
        }
        public void Add(MessageRequest request)
        {
            if (request == null)
            {
                return;
            }
            var extention = string.Empty;
            #region save image base64 into folder ImageMessage with file name is Id of message
            if (request.FileImage != null) 
            {
                var listInfoFIle = request.FileImage.Split(";base64,");
                if(listInfoFIle.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Image\\ImageMessage\\");
                    var filePath = path + request.Id;
                    File.WriteAllBytes(filePath, Convert.FromBase64String(listInfoFIle[1]));
                    extention = listInfoFIle[0].Replace("data:image/", string.Empty);
                }
                
            }
            #endregion
            var message = new Messages
            {
                Id = request.Id,
                Content = request.Content,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                MessageDate = request.MessageDate,
                FileImageName = request.FileImageName,
                ext = extention,
            };

            _messageRepository.AddAsync(message);
            _unitOfWork.SaveChanges();
        }

        public async Task<ApiResult<bool>> DeleteMessage(MessageDeleteModel messageDeleteModel)
        {
           
            if (messageDeleteModel != null)
            {
                var message = await _messageRepository.GetByIdAsync(messageDeleteModel.Message.Id);
                if (message != null)
                {
                    if(messageDeleteModel.DeleteType == Constant.DELETE_TYPE_ALL_USER)
                    {
                        message.IsSenderDeleted = true;
                        message.IsReceiverDeleted = true;
                    }
                    else
                    {
                        message.IsSenderDeleted = message.IsSenderDeleted || (message.SenderId == messageDeleteModel.DeletedUserId);
                        message.IsReceiverDeleted = message.IsReceiverDeleted || (message.ReceiverId == messageDeleteModel.DeletedUserId);
                    }
                    _messageRepository.Update(message);
                    var resultSave = await _unitOfWork.SaveChangeAsync();
                    if(resultSave == 1)
                        return new ApiSuccessResult<bool>();
                }
            }
            return new ApiErrorResult<bool>(Constant.DELETE_MESSAGE_FAIL);
        }

        public async Task<ApiResult<List<GetAllMessageResponseModel>>> GetAll()
        {
            var listMessage = await _messageRepository.GetAllAsync();
            if (listMessage != null)
            {
                var result = new List<GetAllMessageResponseModel>();
                foreach (var message in listMessage)
                {
                    var messageModel = new GetAllMessageResponseModel();
                    messageModel.Id = message.Id;
                    messageModel.Content = message.Content;
                    messageModel.SenderId = message.SenderId;
                    messageModel.ReceiverId = message.ReceiverId;
                    messageModel.MessageDate = message.MessageDate;
                    messageModel.IsReceiverDeleted = message.IsReceiverDeleted;
                    messageModel.IsSenderDeleted = message.IsSenderDeleted;
                    if(!string.IsNullOrEmpty(message.FileImageName))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "Image\\ImageMessage\\");
                        var filePath = path + message.Id;
                        messageModel.IsImage = true;
                        messageModel.FileImageName = message.FileImageName;
                        var imageByte = await File.ReadAllBytesAsync(filePath);
                        var base64String = Convert.ToBase64String(imageByte);
                        messageModel.FileImage = $"data:{message.ext};base64,{base64String}";
                    }
                    result.Add(messageModel);
                }
                result = result.OrderBy(x => x.MessageDate).ToList();
                return new ApiSuccessResult<List<GetAllMessageResponseModel>>(result);
            }
            return new ApiErrorResult<List<GetAllMessageResponseModel>>(Constant.GET_MESSAGE_FAIL);
        }

        public async Task<ApiResult<List<GetAllMessageResponseModel>>> GetReceivedMessages(Guid userId)
        {
            if(userId != Guid.Empty)
            {
                var listMessages = await _unitOfWork.MessageRepository.GetMessageByRecievedIdAsync(userId);
                
                if (listMessages != null)
                {
                    var result = listMessages.Select(x => new GetAllMessageResponseModel
                    {
                        Content = x.Content,
                        SenderId = x.SenderId,
                        ReceiverId = x.ReceiverId,
                        MessageDate = x.MessageDate,
                    }).ToList();
                    return new ApiSuccessResult<List<GetAllMessageResponseModel>>(result);
                }    
            }
            return new ApiErrorResult<List<GetAllMessageResponseModel>>(Constant.GET_MESSAGE_BY_RECEIVED_FAIL);
        }

      
    }
}
