using TaskIT.DTOs.NotificationDTOs;

namespace TaskIT.Repository.NotificationRepositoryF
{
    public interface NotificationRepository: Repository<Notification>
    {
        public Task<Notification> CreateNotificationAsync(Notification notification);
        public Task<List<string>> GetUnreadMesages(string userId);
    }
}
