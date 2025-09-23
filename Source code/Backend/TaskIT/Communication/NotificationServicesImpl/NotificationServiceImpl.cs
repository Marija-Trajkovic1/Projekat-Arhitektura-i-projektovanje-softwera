using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.NotificationServices;
using TaskIT.DTOs.MessagesDTOs;
using TaskIT.Hubs;
using TaskIT.Repository.NotificationRepositoryF;

namespace TaskIT.Communication.NotificationServicesImpl
{
    public class NotificationServiceImpl : NotificationService
    {
        private IHubContext<TaskItHub> context;
        private readonly NotificationRepository notificationRepository;
        public  NotificationServiceImpl(IHubContext<TaskItHub> context, NotificationRepository notificationRepository)
        {
            this.context = context;
            this.notificationRepository = notificationRepository;
        }

        public async Task NotifyGroup(string eventName, string groupName, MessageDTO message)
        {
            var notification = new Notification { MessageText = message.Message, ReceiverId = groupName };
            await notificationRepository.CreateNotificationAsync(notification);
            await context.Clients.Group(groupName).SendAsync(eventName, message);
        }

        public async Task NotifyUser(string eventName, string receiverId, MessageDTO message)
        {
            var notification = new Notification { MessageText = message.Message, ReceiverId = receiverId };
            await notificationRepository.CreateNotificationAsync(notification);
            await context.Clients.Group(receiverId).SendAsync(eventName, message);
        }
    }
}
