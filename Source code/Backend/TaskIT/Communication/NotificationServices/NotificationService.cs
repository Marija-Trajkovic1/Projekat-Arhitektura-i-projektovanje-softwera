using TaskIT.DTOs.MessagesDTOs;

namespace TaskIT.Communication.NotificationServices
{
    public interface NotificationService
    {
        Task NotifyUser(string eventName, string receiverId, MessageDTO message);
        Task NotifyGroup(string eventName, string groupName,  MessageDTO message);
    }
}
