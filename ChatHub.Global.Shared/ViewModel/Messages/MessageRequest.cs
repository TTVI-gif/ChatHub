using Microsoft.AspNetCore.Http;

namespace ChatHub.Global.Shared.ViewModel.Messages
{
    public class MessageRequest
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsSenderDeleted { get; set; }
        public bool IsReceiverDeleted { get; set; }
        public string FileImage { get; set; }
        public string FileImageName { get; set; }
        public bool IsImage { get; set; }
    }
}
