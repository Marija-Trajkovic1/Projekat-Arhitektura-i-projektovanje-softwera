using TaskIT.DTOs.NotificationDTOs;

namespace TaskIT.Repository.NotificationRepositoryF
{
    public class NotificationRepositoryImpl : RepositoryImpl<Notification>, NotificationRepository
    {
        public NotificationRepositoryImpl(TaskITContext context):base(context)
        {
        }
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();
            return notification;
        }

        public async Task<List<string>> GetUnreadMesages(string userId)
        {
            var unreadMessages = await context.Notifications.Where(n => n.ReceiverId == userId && n.IsRead == false).Select(n=>n.MessageText).ToListAsync();
            return unreadMessages;
        }
    }
}
