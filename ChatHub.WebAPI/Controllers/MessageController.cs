using ChatHub.Application.IService;
using ChatHub.Global.Shared.Message;
using Microsoft.AspNetCore.Mvc;

namespace ChatHub.WebAPI.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages =await _messageService.GetAll();
            return Ok(messages);
        }


        [HttpGet("received-messages/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserReceivedMessages(Guid userId)
        {
            var messages =await _messageService.GetReceivedMessages(userId);
            return Ok(messages);
        }


        [HttpPost()]
        public async Task<IActionResult> DeleteMessage([FromBody] MessageDeleteModel messageDeleteModel)
        {
            var message = await _messageService.DeleteMessage(messageDeleteModel);
            return Ok(message);
        }

    }
}
