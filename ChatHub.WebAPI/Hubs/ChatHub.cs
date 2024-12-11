using ChatHub.Application.IService;
using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.Message;
using ChatHub.Global.Shared.ViewModel.Messages;
using Microsoft.AspNetCore.SignalR;

namespace ChatHub.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        static IList<UserConnection> Users = new List<UserConnection>();

        public class UserConnection
        {
            public Guid UserId { get; set; }
            public string ConnectionId { get; set; }
            public string FullName { get; set; }
            public string Username { get; set; }
        }

        public Task SendMessageToUser(MessageRequest message)
        {
            var reciever = Users.FirstOrDefault(x => x.UserId == message.ReceiverId);
            var connectionId = reciever == null ? "offlineUser" : reciever.ConnectionId;
            _messageService.Add(message);
            return Clients.Client(connectionId).SendAsync("ReceiveDM", Context.ConnectionId, message);
        }

        public async Task DeleteMessage(MessageDeleteModel message)
        {
            var deletedMessage = await _messageService.DeleteMessage(message);
            await Clients.All.SendAsync("BroadCastDeleteMessage", Context.ConnectionId, deletedMessage);
        }

        public async Task PublishUserOnConnect(Guid id, string fullname, string username)
        {

            var existingUser = Users.FirstOrDefault(x => x.Username == username);
            var indexExistingUser = Users.IndexOf(existingUser);

            UserConnection user = new UserConnection
            {
                UserId = id,
                ConnectionId = Context.ConnectionId,
                FullName = fullname,
                Username = username
            };

            if (!Users.Contains(existingUser))
            {
                Users.Add(user);

            }
            else
            {
                Users[indexExistingUser] = user;
            }

            await Clients.All.SendAsync("BroadcastUserOnConnect", Users);

        }

        public void RemoveOnlineUser(Guid userID)
        {
            var user = Users.Where(x => x.UserId == userID).ToList();
            foreach (UserConnection i in user)
                Users.Remove(i);

            Clients.All.SendAsync("BroadcastUserOnDisconnect", Users);
        }
    }
}
