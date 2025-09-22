using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.DTOs.MessagesDTOs;
using TaskIT.Hubs;

namespace TaskIT.Communication.NotificationServicesImpl
{
    public class NotificationServiceImpl : NotificationService
    {
        private IHubContext<TaskItHub> context;
        public  NotificationServiceImpl(IHubContext<TaskItHub> context)
        {
            this.context = context;
        }

        public async Task NotifyGroup(string eventName, string groupName, MessageDTO message)
        {
            await context.Clients.Group(groupName).SendAsync(eventName, message);
        }

        public async Task NotifyUser(string eventName, string receiverId, MessageDTO message)
        {
            await context.Clients.Group(receiverId).SendAsync(eventName, message);
        }
    }
}
